using System;
using Godot;
using UnityToGodotHelper;
using Godot.NativeInterop;

[Tool]
public partial class MonoBehaviour : Node
{
    private GameObject m_gameObject;
    public GameObject gameObject { get { return m_gameObject; } }
    
    private bool m_enabled = true;
    public bool enabled { get { return m_enabled; } }

    private bool m_started = false;
    private Callable ScriptChangedCallable;

/*
    static void Trampoline(object delegateObj, NativeVariantPtrArgs args, out godot_variant ret)
    {
        Debug.Log("Trampoline");


        if (args.Count != 1)
            throw new ArgumentException($"Callable expected 1 arguments but received" + args.Count.ToString());

        TResult res = ((Func<int, string>)delegateObj)(
            VariantConversionCallbacks.GetToManagedCallback<int>()(args[0])
        );

        ret = VariantConversionCallbacks.GetToVariantCallback<string>()(res);
    }
*/
    static void Trampoline2(MonoBehaviour _object)
    {
        Debug.Log("Trampoline2");

        /*
        if (args.Count != 1)
            throw new ArgumentException($""Callable expected {1} arguments but received {args.Count}.");

        TResult res = ((Func<int, string>)delegateObj)(
            VariantConversionCallbacks.GetToManagedCallback<int>()(args[0])
        );

        ret = VariantConversionCallbacks.GetToVariantCallback<string>()(res);
        */
    }
    public MonoBehaviour()
    {
        Name = GetType().ToString();
        if (Engine.IsEditorHint())
        {   
            // Callable callable = new Callable(this, nameof(OnScriptChanged));
            // ScriptChangedCallable = callable.Bind(this);

            // ScriptChangedCallable = Callable.CreateWithUnsafeTrampoline((int num) => "foo" + num.ToString(), &Trampoline);
            ScriptChangedCallable = Callable.From(() => Trampoline2(this));
            Connect(Godot.GodotObject.SignalName.ScriptChanged, ScriptChangedCallable);


            // Connect("script_changed", ScriptChangedCallable);

            TreeEntered += OnTreeEntered;
            TreeExiting += OnTreeExiting;
            // ScriptChanged += OnScriptChanged;
        }

        ChangeNodeName();
    }

    ~MonoBehaviour()
    {
        if (Engine.IsEditorHint())
        {
            TreeEntered -= OnTreeEntered;
            TreeExiting -= OnTreeExiting;

            Disconnect(Godot.GodotObject.SignalName.ScriptChanged, ScriptChangedCallable);
            // ScriptChanged -= OnScriptChanged;
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
    public void ChangeNodeName()
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
    
    protected virtual void OnScriptChanged(MonoBehaviour _this)
    {
        if (Engine.IsEditorHint())
        {
            Debug.Log("OnScriptChanged()");
            _this.ChangeNodeName();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            Debug.Log("_Ready()");
            ChangeNodeName();
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
