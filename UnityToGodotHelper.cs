using Godot;
using System;
using System.Diagnostics;

// File to simplify Unity -> Godot porting
namespace UnityToGodotHelper
{
	public class PlayableAsset
	{
	}

	static public class _Time
	{
		static public float deltaTime;
        
	}
	public static class Debug
	{
		public static void Log(string message)
		{
			GD.Print(message);

            #if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Log(2,"Log:", message);
            }
            #endif
		}
	}
	public static class Assert
	{
		public static void IsTrue(bool condition,string message)
		{
            System.Diagnostics.Debug.Assert(condition, message);
            
            #if DEBUG
            if (!condition)
            {
                Debugger.Log(1,"Assert failed:", message);
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            #endif
		}
	}
    public static class Utils
    {
        public static GameObject GetGameObjectParent(GodotObject godotObject)
        {
            Node node = godotObject as Node;
            if (node != null)
            {   do
                {
                    Node parent = node.GetParent();
                    GameObject gameObject = parent as GameObject;
                
                    if (gameObject != null)
                    {
                        return gameObject; 
                    }

                    node = parent;
                } while (node != null);
            }

            return null;
        }
    }
}
