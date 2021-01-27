namespace BlackRefactory.Console
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
                DeveloperConsole.Instance.AddMessageToConsole(help, ConsoleLogTag.LOG);

                return;
            }
            
            if(DeveloperConsole.IsValidCommand(args[0])){
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsole.Commands[args[0]].Help, ConsoleLogTag.LOG);
            }
            else
            {
                DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.UnrecognizedCommandMessage, ConsoleLogTag.WARNING);
            }
        }

    }
}

