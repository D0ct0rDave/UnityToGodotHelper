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
		public static void Log(string _message)
		{
            #if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Log(0,"Log:", _message + "\n");
            }
            else
            {
			    GD.Print(_message);
            }
            #endif
		}
        
        public static void LogError(string _message)
        {
            #if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Log(2,"Error:", _message + "\n");
            }
            else
            {
			    GD.PrintErr(_message);
            }
            #endif
        }
        
        public static void LogException(Exception ex)
        {
            LogError($"Exception: {ex.Message}\n{ex.StackTrace}");            
            Debugger.Break();
        }

        public static string GetFullyQualifiedName(Node _node)
        {
            if (_node == null)
            {
                return "(Null)";
            }

            string _fullyQualifiedName = _node.Name;
            Node parentNode = _node.GetParent();
            while (parentNode != null)
            {
                _fullyQualifiedName = parentNode.Name + "/" + _fullyQualifiedName;
                parentNode = parentNode.GetParent();
            }
            
           return "/" + _fullyQualifiedName;
        }
	}

	public static class Assert
	{
		public static void IsTrue(bool _condition, string _message)
		{
            // System.Diagnostics.Debug.Assert(_condition, _message);

            #if DEBUG
            if (!_condition)
            {
                Debugger.Log(1,"Assert failed:", _message);
                GD.PushError(1,"Assert failed:", _message);
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
        public static GameObject GetGameObjectParent(GodotObject _godotObject)
        {
            Node node = _godotObject as Node;
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
