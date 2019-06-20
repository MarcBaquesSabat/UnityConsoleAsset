using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public static class DeveloperConsoleMessages
    {
        public static string DefaultConsoleInputMessage = "Enter text...";
        public static string DefaultConsoleTextMessage = "Developer console...";
        public static string UnrecognizedCommandMessage = "Command not recognized.";
        public static string MissingCommandArgumentsMessage = "No arguments found.";
        public static string MissingAndCreateEventSystemMessage = "Can't find an EventSystem. An EventSystem has been created.";
        public static string TimeScaleChangedMessage = "TimeScale changed to ";

        public static string NotFoundSceneMessage = "The scene you are trying to load doesn't exist or it isn't on the build list.";
    }
}
