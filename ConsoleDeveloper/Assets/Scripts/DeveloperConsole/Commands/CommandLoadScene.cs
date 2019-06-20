
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
            Description = "Load the scene if it's on the build in additive mode.";
            Help = "Use this commands followed to the scene name to load it in additive mode.";

            AddCommandToConsole();
        }

        //Logic of the command
        public override void RunCommand(string[] args)
        {
            if (!Application.CanStreamedLevelBeLoaded(args[0]))
            {
                Debug.LogWarning(DeveloperConsoleMessages.NotFoundSceneMessage);
                return;
            }

            SceneManager.LoadScene(args[0], LoadSceneMode.Additive);
        }

        public static CommandLoadScene CreateCommand()
        {
            return new CommandLoadScene();
        }
    }
}
