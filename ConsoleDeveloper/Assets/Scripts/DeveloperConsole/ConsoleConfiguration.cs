using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConsoleConfiguration", menuName = ("/Console Configuration"))]
public class ConsoleConfiguration : ScriptableObject
{
    public bool showUnityLogsOnConsole = true;
    public bool showCommandsCreationMessage = false;
}
