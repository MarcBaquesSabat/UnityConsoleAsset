using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandDescriptionCommand : ConsoleCommand
    {
        //Name of the command
        public override string Name { get; protected set; }
        //Command to type on console
        public override string Command { get; protected set; }
        //Brief description of what does the command
        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandDescriptionCommand()
        {
            Name = "Description";
            Command = "description";
            Description = "Displays the description of a command.";
            Help = "Usage: \"Description <(command)>\"";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            if (DeveloperConsoleUtils.noValidArguments(args))
            {
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.MissingCommandArgumentsMessage, ConsoleLogTag.ERROR);
                return;
            }
            
            if (DeveloperConsole.isValidCommand(args[0]))
            {
                DeveloperConsole.Instance.AddMessageToConsole(args[0] + " description: " + DeveloperConsole.Commands[args[0]].Description, ConsoleLogTag.LOG);
            }
            else
            {
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.UnrecognizedCommandMessage, ConsoleLogTag.ERROR);
            }
            
        }

        public static CommandDescriptionCommand CreateCommand()
        {
            return new CommandDescriptionCommand();
        }
    }
}
