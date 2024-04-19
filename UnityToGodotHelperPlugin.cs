#if TOOLS
using Godot;
using System;

[Tool]
public partial class UnityToGodotHelperPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
        Script gameObjectScript = ResourceLoader.Load("res://addons/UnityToGodotHelper/GameObject.cs") as Script;
        var gameObjectTexture = GD.Load<Texture2D>("res://addons/UnityToGodotHelper/unity_game_object_icon.svg");
		AddCustomType("GameObject", "Node3D", gameObjectScript, gameObjectTexture);
		
        Script monoBehaviourScript = ResourceLoader.Load("res://addons/UnityToGodotHelper/MonoBehaviour.cs") as Script;
        var monoBehaviourTexture = GD.Load<Texture2D>("res://addons/UnityToGodotHelper/unity_component_icon.svg");
        AddCustomType("MonoBehaviour", "Node3D", monoBehaviourScript, monoBehaviourTexture);		
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
        RemoveCustomType("GameObject");
		RemoveCustomType("MonoBehaviour");
	}
}
#endif
