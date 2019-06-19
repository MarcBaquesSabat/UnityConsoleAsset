using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Console
{
    public class CommandClearConsole : ConsoleCommand
    {
        //Name of the command
        public override string Name { get; protected set; }
        //Command to type on console
        public override string Command { get; protected set; }
        //Brief description of what does the command
        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandClearConsole()
        {
            Name = "Clear";
            Command = "clear";
            Description = "Clear the console.";
            Help = "Use this commands with no arguments to clear the console!.";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            DeveloperConsole.Instance.consoleInput.text = DeveloperConsoleMessages.DefaultConsoleInputMessage;
            DeveloperConsole.Instance.consoleText.text = DeveloperConsoleMessages.DefaultConsoleTextMessage + "\n";
        }

        public static CommandClearConsole CreateCommand()
        {
            return new CommandClearConsole();
        }
    }
}
