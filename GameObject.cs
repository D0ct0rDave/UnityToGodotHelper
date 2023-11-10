using Godot;
using System;
using UnityToGodotHelper;

[Tool]
public partial class GameObject : Node3D
{
	private string m_name;
	public string name { get { return m_name; } }
	
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
