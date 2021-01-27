using UnityEngine;

namespace BlackRefactory.Console
{
    [CreateAssetMenu(fileName = "NewConsoleConfiguration", menuName = ("Developer Console/Console Configuration"))]
    public class ConsoleConfiguration : ScriptableObject
    {
        //public Color backgroundColor;
        //public Color fontColor;
        //public Font consoleFont;

        public bool showUnityLogsOnConsole = true;
        public bool showCommandsCreationMessage = false;
        public bool showStackErrorLogs = false;
    }
}

