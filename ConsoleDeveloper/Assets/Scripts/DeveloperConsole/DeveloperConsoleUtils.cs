using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return (_input.Length == 0 || _input == null );
    }

}
