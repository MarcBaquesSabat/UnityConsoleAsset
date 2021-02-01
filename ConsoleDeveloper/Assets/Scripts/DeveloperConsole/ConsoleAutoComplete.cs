using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackRefactory.Console
{
    public class ConsoleAutoComplete : MonoBehaviour
    {
        [SerializeField] 
        private List<ConsoleAutoCompleteCommand> commandsToShow = new List<ConsoleAutoCompleteCommand>();
        [SerializeField] 
        private ConsoleAutoCompleteCommand previewCommandPrefab;

        [SerializeField] 
        private Image background = null;
        public void ShowAutoCompleteCommands(List<string> commandsKeyToShow)
        {
            if (commandsKeyToShow.Count <= 0)
            {
                CleanPreviews();
                return;
            }

            CleanPreviews();
            background.enabled = true;

            foreach (var commandKey in commandsKeyToShow)
            {
                var newCommand = Instantiate(previewCommandPrefab, this.transform);
                newCommand.Build(commandKey,"Description");
                commandsToShow.Add(newCommand);
            }

            if (commandsToShow.Count > 0)
            {
                commandsToShow[0].Select();
            }
        }

        public void CleanPreviews()
        {
            foreach (var commandPreview in commandsToShow)
            {
                Destroy(commandPreview.gameObject);
            }
            
            commandsToShow.Clear();
            background.enabled = false;
        }
    }
}