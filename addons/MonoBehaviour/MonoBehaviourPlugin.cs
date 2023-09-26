#if TOOLS
using Godot;
using System;
 
[Tool]
public partial class MonoBehaviourPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
        Script script = ResourceLoader.Load("res://addons/MonoBehaviour/MonoBehaviour.cs") as Script;
        var texture = GD.Load<Texture2D>("res://addons/MonoBehaviour/unity_component_icon.svg");
        AddCustomType("MonoBehaviour", "Node", script, texture);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
        RemoveCustomType("MonoBehaviour");
	}
}
#endif
