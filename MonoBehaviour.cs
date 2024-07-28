using System;
using System.IO;
using Godot;
using UnityToGodotHelper;
using Godot.NativeInterop;
using System.Runtime.CompilerServices;

[Tool]
public partial class MonoBehaviour : Node3D 
// Setting the base as Node3D instead of Node, allows MonoBehaviour to have 3D children.
// Not strictly good, but done for convenience.
{
    private GameObject m_gameObject;
    public GameObject gameObject { get { return m_gameObject; } }
    
    private bool m_enabled = true;
    public bool enabled { get { return m_enabled; } }

    private bool m_started = false;
    private bool m_scriptChanged = false;
    // ------------------------------------------------------------------------
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
            TreeEntered -= OnTreeEntered;
        }
    }

    private void OnScriptChanged()
    {
        Debug.Log("OnScriptChanged");
        m_scriptChanged = true;
    }
    private void OnTreeEntered()
    {
        Debug.Log("OnTreeEntered");
    }
    private string ComputeNodeName()
    {
        Script script = (Script)GetScript();
        if (script != null)
        {
            if (script.ResourceName != "")
            {
                Debug.Log("Name =" + script.ResourceName);
                return script.ResourceName;
            }
            else if (script.ResourcePath != "")
            {
                return Path.GetFileNameWithoutExtension(script.ResourcePath);
            }
        }
        
        return Name;
    }
    private void ChangeNodeName()
    {
        Debug.Log("ChangeNodeName");

        if (Engine.IsEditorHint())    
        {
            Script script = (Script)GetScript();
            if (script != null)
            {
                Name = ComputeNodeName();
            }
        }
    }
    // ------------------------------------------------------------------------
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        m_gameObject = GetParent() as GameObject;

        Assert.IsTrue(m_gameObject != null, "This MonoBehaviour component does not belong to any parent GameObject");
        DoAwake();

        #if !DO_DEFERRED_START
        DoStart();
        #endif

        // Debug.Log("_Ready called for component " + ComputeNodeName() + "("+ Name +") in object " + gameObject.Name + "\n" + gameObject.fullQualifiedName + "\n");
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
            #if DO_DEFERRED_START
            if (! HasStarted())
            {
                DoStart();
            }
            else
            #endif
            {
                DoUpdate();
            }
        }
    }
    // ------------------------------------------------------------------------
    // Component public methods:
    // ------------------------------------------------------------------------    
    // Retrieves a 'sibling' component inside the parent
    // ------------------------------------------------------------------------    
    public T GetComponent<T>() where T : Node
    {
        return m_gameObject.GetComponent<T>();
    } 
    // ------------------------------------------------------------------------
    public bool HasStarted()
    {
        return m_started;
    }
    
    // ------------------------------------------------------------------------
    // Wrapper methods
    // ------------------------------------------------------------------------
    private void DoAwake()
    {
        try
        {
            Awake();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            throw; // Rethrow the exception to halt the application
        }
    }

    private void DoStart()
    {
        try
        {
            Start();
            m_started = true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            throw; // Rethrow the exception to halt the application
        }
    }

    private void DoUpdate()
    {
        try
        {
            Update();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            throw; // Rethrow the exception to halt the application
        }
    }

    // ------------------------------------------------------------------------
    /// Component lifetime methods

    protected void Destroy(GameObject i_gameObject)
    {
        i_gameObject.Dispose();
    }

    protected void DontDestroyOnLoad(GameObject i_gameObject)
    {
    }
    
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }
    
    protected virtual void Update()
    {        
    }    
}
