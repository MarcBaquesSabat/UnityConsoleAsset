using System.Globalization;
using UnityEngine;

namespace BlackRefactory.Console
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
            Help = "Usage: \"TimeScale <num>\"";
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            float newTimeScale = float.Parse(args[0], new CultureInfo("en-US"));
            
            Time.timeScale = newTimeScale;

            DeveloperConsole.Instance.AddMessageToConsole(DeveloperConsoleMessages.TimeScaleChangedMessage + newTimeScale.ToString(), ConsoleLogTag.LOG);
        }

    }
}
