#if TOOLS
using Godot;
using System;

[Tool]
public partial class GameObjectPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
        Script script = ResourceLoader.Load("res://addons/GameObject/GameObject.cs") as Script;

		// var script = GD.Load<Script>("res:://scripts/UnityPort/GameObject.cs"); 	// <--- For some reason I cannot make this work
        var texture = GD.Load<Texture2D>("res://addons/GameObject/unity.svg");
        AddCustomType("GameObject", "Node3D", script, texture);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
        RemoveCustomType("GameObject");
	}
}
#endif
