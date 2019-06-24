using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandHelp : ConsoleCommand
    {
        //Name of the command
        public override string Name { get; protected set; }
        //Command to type on console
        public override string Command { get; protected set; }
        //Brief description of what does the command
        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandHelp()
        {
            Name = "Help";
            Command = "help";
            Description = "Shows all the available commands or the usage of a command if it's followed by a command.";
            Help = "Usage: \"Help <(command)>";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            if(DeveloperConsoleUtils.noValidArguments(args))
            {
                string help = "";
                help += "Avilable commands:\n";
                foreach (ConsoleCommand command in DeveloperConsole.Commands.Values)
                {
                    help += "-" + command.Name + "\n";
                }
                DeveloperConsole.Instance.AddMessageToConsole(help);

                return;
            }
            
            if(DeveloperConsole.isValidCommand(args[0])){
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsole.Commands[args[0]].Help, ConsoleLogTag.LOG);
            }
            else
            {
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.UnrecognizedCommandMessage, ConsoleLogTag.WARNING);
            }
        }

        public static CommandHelp CreateCommand()
        {
            return new CommandHelp();
        }

    }
}

