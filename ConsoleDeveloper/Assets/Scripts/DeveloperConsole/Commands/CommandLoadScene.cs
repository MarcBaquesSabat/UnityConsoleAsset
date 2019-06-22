﻿
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Console
{
    public class CommandLoadScene : ConsoleCommand
    {
        //Name of the command
        public override string Name { get; protected set; }
        //Command to type on console
        public override string Command { get; protected set; }
        //Brief description of what does the command
        public override string Description { get; protected set; }

        public override string Help { get; protected set; }

        public CommandLoadScene()
        {
            Name = "LoadScene";
            Command = "loadscene";
            Description = "Load the scene if it's on the build.";
            Help = "Usage: \"LoadScene <sceneName> <(LoadModeType)>\"";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            if (!Application.CanStreamedLevelBeLoaded(args[0]))
            {
                DeveloperConsole.Instance.AddTagedMessageToConsole(DeveloperConsoleMessages.NotFoundSceneMessage + args[0], MessageTag.ERROR);
                return;
            }
            
            SceneManager.LoadScene(args[0]);
            DeveloperConsole.Instance.AddTagedMessageToConsole(DeveloperConsoleMessages.SceneLoadedMessage + args[0]);
        }

        public static CommandLoadScene CreateCommand()
        {
            return new CommandLoadScene();
        }
    }
}
