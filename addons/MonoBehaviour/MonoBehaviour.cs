using System;
using Godot;
using UnityPortHelper;

public partial class MonoBehaviour : Node
{
    private GameObject m_gameObject;
    public GameObject gameObject { get { return m_gameObject; } }
    
    private bool m_enabled = true;
    public bool enabled { get { return m_enabled; } }

    private bool m_started = false;
    public MonoBehaviour()
    {
        Name = GetType().ToString();
    }
    // Component methods
    // Retrieves a 'sibling' components inside the parent
    public T GetComponent<T>() where T : Node
    {
        return m_gameObject.GetComponent<T>();
    } 

    protected void Destroy(GameObject i_gameObject)
    {
        i_gameObject.Dispose();
    }
    protected void DontDestroyOnLoad(GameObject i_gameObject)
    {
    }

    /// Component lifetime methods
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }
    
    protected virtual void Update()
    {        
    }
    
    /// Node Godot lifetime methods
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        m_gameObject = GetParent() as GameObject;
        Awake();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (! m_started)
        {
            Start();
            m_started = true;
        }
        else
        {          
            Update();
        }
    }
}
