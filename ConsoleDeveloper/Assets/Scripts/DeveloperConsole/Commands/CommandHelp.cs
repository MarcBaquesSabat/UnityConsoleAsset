﻿using System.Collections;
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
            Description = "Shows all the available commands.";
            Help = "Use this commands with no arguments to show all the commands available or followed by the command to show the help of the command.";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            if(DeveloperConsoleUtils.noValidArguments(args))
            {
                DeveloperConsole.Instance.AddMessageToConsole("Avilable commands:");

                foreach (ConsoleCommand command in DeveloperConsole.Commands.Values)
                {
                    DeveloperConsole.Instance.AddMessageToConsole("- " + command.Name);
                }

                return;
            }
            
            if(DeveloperConsole.isValidCommand(args[0])){
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsole.Commands[args[0]].Help);
            }
            else
            {
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.UnrecognizedCommandMessage);
            }
        }

        public static CommandHelp CreateCommand()
        {
            return new CommandHelp();
        }

    }
}

