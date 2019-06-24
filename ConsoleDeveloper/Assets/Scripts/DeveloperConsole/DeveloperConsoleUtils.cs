using UnityEngine;
using UnityEngine.EventSystems;

namespace Console
{
    public enum ConsoleLogTag { LOG, WARNING, ERROR, NONE, MAX};

    public static class DeveloperConsoleUtils
    {
        public static bool noValidArguments(string[] args)
        {
            if (args == null || !(args.Length > 0)) return true;
            if (args[0] == null || args[0] == "") return true;
            return false;
        }

        public static bool isInputInvalid(string[] _input)
        {
            return (_input.Length == 0 || _input == null);
        }

        public static bool IsEventSystemOnScene()
        {
            return (Object.FindObjectOfType<EventSystem>() != null);
        }

        public static void CreateEventSystem(Transform parent)
        {
            Debug.LogWarning(DeveloperConsoleMessages.MissingAndCreateEventSystemMessage);
            GameObject go = new GameObject();
            go.name = "EventSystem";
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
            go.transform.SetParent(parent);
        }

        public static ConsoleLogTag LogTypeToMessageType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return ConsoleLogTag.ERROR;
                case LogType.Assert:
                    return ConsoleLogTag.ERROR;
                case LogType.Warning:
                    return ConsoleLogTag.WARNING;
                case LogType.Log:
                    return ConsoleLogTag.LOG;
                case LogType.Exception:
                    return ConsoleLogTag.ERROR;
                default:
                    return ConsoleLogTag.MAX;
            }
        }

        public static string CreateTagString(ConsoleLogTag tag)
        {
            return (tag == ConsoleLogTag.NONE || tag == ConsoleLogTag.MAX) ? "" : "[" + tag.ToString() + "]";
        }
    }
}

