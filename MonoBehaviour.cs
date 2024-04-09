using System;
using System.IO;
using Godot;
using UnityToGodotHelper;
using Godot.NativeInterop;
using System.Runtime.CompilerServices;

[Tool]
public partial class MonoBehaviour : Node
{
    private GameObject m_gameObject;
    public GameObject gameObject { get { return m_gameObject; } }
    
    private bool m_enabled = true;
    public bool enabled { get { return m_enabled; } }

    private bool m_started = false;
    private bool m_scriptChanged = false;

    public MonoBehaviour()
    {
        // Debug.Log("MonoBehaviour ctor ");
        Name = GetType().ToString();

        if (Engine.IsEditorHint())
        {   
            // ScriptChanged += OnScriptChanged;
            TreeEntered += OnTreeEntered;
        }
    }

    ~MonoBehaviour()
    {
        if (Engine.IsEditorHint())
        {
            // ScriptChanged -= OnScriptChanged;
        }
    }

    private void OnScriptChanged()
    {
        Debug.Log("OnScriptChanged");
        m_scriptChanged = true;
    }
    private void OnTreeEntered()
    {
        Debug.Log("OnScriptChanged");
        ChangeNodeName();
    }
    private void ChangeNodeName()
    {
        Debug.Log("ChangeNodeName");

        if (Engine.IsEditorHint())    
        {
            Script script = (Script)GetScript();
            if (script != null)
            {
                if (script.ResourceName != "")
                {
                    Debug.Log("OnScriptChanged " + script.ResourceName);
                    Name = script.ResourceName;
                }
                else if (script.ResourcePath != "")
                {
                    string scriptFileName = Path.GetFileNameWithoutExtension(script.ResourcePath);
                }
            }
        }
    }
    // Component methods
    // Retrieves a 'sibling' componen inside the parent
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        m_gameObject = GetParent() as GameObject;

        Assert.IsTrue(m_gameObject != null, "This MonoBehaviour component does not belong to any parent GameObject");
        Awake();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Engine.IsEditorHint())
        {   
            if (m_scriptChanged)
            {
                ChangeNodeName();
                m_scriptChanged = false;
            }
        }
        else
        {
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

    public bool HasStarted()
    {
        return m_started;
    }
}
