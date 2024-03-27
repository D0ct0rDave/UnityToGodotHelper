using System;
using Godot;
using UnityToGodotHelper;

[Tool]
public partial class MonoBehaviour : Node
{
    private GameObject m_gameObject;
    public GameObject gameObject { get { return m_gameObject; } }
    
    private bool m_enabled = true;
    public bool enabled { get { return m_enabled; } }

    private bool m_started = false;
    private Callable ScriptChangedCallable;

    public MonoBehaviour()
    {
        Name = GetType().ToString();
        // ScriptChangedCallable = new Callable(this, nameof(OnScriptChanged));
        // Connect(Godot.GodotObject.SignalName.ScriptChanged, ScriptChangedCallable);
        if (Engine.IsEditorHint())
        {   
            ScriptChanged += OnScriptChanged;
        }

        ChangeNodeName();
    }

    ~MonoBehaviour()
    {
        if (Engine.IsEditorHint())
        {
            TreeEntered -= OnTreeEntered;
            TreeExiting -= OnTreeExiting;
            ScriptChanged -= OnScriptChanged;
        }
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
    private void ChangeNodeName()
    {
        if (Engine.IsEditorHint())    
        {
            Script script = (Script)GetScript();
            if (script != null)
            {
                Name = script.ResourceName;
            }
        }
    }
    protected virtual void OnTreeExiting()
    {        
        if (Engine.IsEditorHint())    
        {
            Debug.Log("OnTreeExiting()");
        }
    }

    protected virtual void OnTreeEntered()
    {        
        if (Engine.IsEditorHint())
        {
            Debug.Log("OnTreeEntered()");
            ChangeNodeName();
        }
    }
    
    protected void OnScriptChanged()
    {
        if (Engine.IsEditorHint())
        {
            Debug.Log("OnScriptChanged()");
            ChangeNodeName();
        }
    }

    /// Node Godot lifetime methods
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            Debug.Log("_Ready()");

            TreeEntered += OnTreeEntered;
            TreeExiting += OnTreeExiting;
            

            ChangeNodeName();

            // Connect("script_changed", ScriptChangedCallable);
        }

        base._Ready();
        m_gameObject = GetParent() as GameObject;

        Assert.IsTrue(m_gameObject != null, "This MonoBehaviour component does not belong to any parent GameObject");
        Awake();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (! m_started)
        {
            try
            {
                Start();
            }
            catch
            {

            }
            
            m_started = true;
        }
        else
        {          
            try
            {
            Update();
            }
            catch
            {
                
            }
        }
    }
}
