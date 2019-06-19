using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public abstract class ConsoleCommand
    {
        //Name of the command
        public abstract string Name { get; protected set; }
        //Command to type on console
        public abstract string Command { get; protected set; }
        //Brief description of what does the command
        public abstract string Description { get; protected set; }

        public abstract string Help { get; protected set; }

        //Add automatically the command to the console
        public void AddCommandToConsole()
        {
            string addMesage = " command has been added to the console.";

            DeveloperConsole.AddCommadsToConsole(Command, this);
            Debug.Log(Name + addMesage);
        }

        //Logic of the command
        public abstract void RunCommand(string[] args);
    }
}
