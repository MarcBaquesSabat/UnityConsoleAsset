using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandDisactiveGameObject : ConsoleCommand
    {
        //Name of the command
        public override string Name { get; protected set; }
        //Command to type on console
        public override string Command { get; protected set; }
        //Brief description of what does the command
        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandDisactiveGameObject()
        {
            Name = "Disactive";
            Command = "disactive";
            Description = "Disactive the serached object. It can be search by Tag/Name.";
            Help = "Usage: \"Disactive <search by> <object>\"";
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            GameObject go = null;

            if (args[0] == "name")
            {
                go = GameObject.Find(args[1]);
            }
            else if (args[0] == "tag")
            {
                go = GameObject.FindGameObjectWithTag(args[1]);
            }

            if (go != null)
            {
                go.SetActive(false);
                DeveloperConsole.Instance.AddMessageToConsole("The object " + args[1] + " has been disactivated.", ConsoleLogTag.LOG);
                return;
            }

            DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.GameObjectNotFoundMessage + args[1], ConsoleLogTag.WARNING);
        }
    }
}


