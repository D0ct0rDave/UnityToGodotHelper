using Godot;
using System;
using UnityToGodotHelper;


// In Unity Monobehaviors are leaf nodes which don't allow other objects be parented with them. (AFAIK)
// The game object, exposes all the component (monobehaviors) properties, so we can make prefab and still 
// be able to modify internal properties.
// In Godot, in order to modify the internal properties we should activate "Edit Children" which expands
// the object tree, and it becomes less convenient, since the scene tree becomes "poluted" with many 
// other objects parented with the GameObject, apart of the monobehaviours, and not needed.
// It would be desirable to expose the properties of all the monobehaviour components in this object,
// This way we could get rid of MonoBehaviors derived from Node3D, and it's child object could be placed 
// in the parent GameObject.

// This can probably be achieved currently, but it's not the purpose of this develpment. For this reason,
// we'll keep Node3D as the base for MonoBehaviour

[Tool]
public partial class GameObject : Node3D
{
	private string m_name;
	public string name { get { return m_name; } }
    
    public string fullQualifiedName { 
        get 
        {
            return Debug.GetFullyQualifiedName(this);
        } 
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_name = Name;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_Time.deltaTime = (float)delta;
	}
		
	public T GetComponent<T>() where T : Node
	{
        for (int i=0;i<GetChildCount();i++)
        {
            Node node = GetChild(i);
            if (node != null)
            {
                T component = node as T;
                if (component != null)
                {
                    return component;
                }
            }
        }

        return default(T);
	}

	public T AddComponent<T>() where T : Node , new()
	{
		T newComponent = new T();
		AddChild(newComponent);
		return newComponent;
	}

    private Node [] m_childObjects = null;

	public void SetActive(bool active)
	{
        if (active)
        {
            if (m_childObjects != null)
            {
                for (int i=0; i<m_childObjects.Length; i++)
                {
                    AddChild(m_childObjects[i]);
                    m_childObjects[i] = null;
                }

                m_childObjects = null;
            }            
        }
        else
        {
            if (m_childObjects == null)
            {
                m_childObjects = new Node[GetChildCount()];

                for (int i=0; i<GetChildCount(); i++)
                {
                    m_childObjects[i] = GetChild(i);
                }

                for (int i=0; i<m_childObjects.Length; i++)
                {
                    RemoveChild(m_childObjects[i]);
                }
            }
        }
	}
}
