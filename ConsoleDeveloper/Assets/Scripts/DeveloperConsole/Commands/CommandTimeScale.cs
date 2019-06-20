using System.Globalization;
using UnityEngine;

namespace Console
{
    public class CommandTimeScale : ConsoleCommand
    {
        //Name of the command
        public override string Name { get; protected set; }
        //Command to type on console
        public override string Command { get; protected set; }
        //Brief description of what does the command
        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandTimeScale()
        {
            Name = "TimeScale";
            Command = "timescale";
            Description = "Change the timescale of the game.";
            Help = "Use this commands followed to the value of the desired time scale.";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            float newTimeScale = float.Parse(args[0], new CultureInfo("en-US"));
            
            Time.timeScale = newTimeScale;

            Debug.Log(DeveloperConsoleMessages.TimeScaleChangedMessage + newTimeScale.ToString());
        }

        public static CommandTimeScale CreateCommand()
        {
            return new CommandTimeScale();
        }
    }
}
