using UnityEngine;
using UnityEngine.EventSystems;

namespace Console
{
    public enum MessageTag { LOG, WARNING, ERROR, MAX};

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

        public static MessageTag LogTypeToMessageType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return MessageTag.ERROR;
                case LogType.Assert:
                    return MessageTag.ERROR;
                case LogType.Warning:
                    return MessageTag.WARNING;
                case LogType.Log:
                    return MessageTag.LOG;
                case LogType.Exception:
                    return MessageTag.ERROR;
                default:
                    return MessageTag.MAX;
            }
        }
    }
}

