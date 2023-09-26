using Godot;
using System;
using System.Diagnostics;

// File to simplify Unity -> Godot porting
namespace UnityPortHelper
{
	public class PlayableAsset
	{
		private int dummy;
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
		}
	}

	public static class Assert
	{
		public static void IsTrue(bool condition,string message)
		{
			// Trace.Assert(condition, message);
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
