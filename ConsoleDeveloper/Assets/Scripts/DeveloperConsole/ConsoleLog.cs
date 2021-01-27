using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackRefactory.Console
{
    public class ConsoleLog : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI logText = null;
        private string command = "";
        private string logMessage = "";
        private ConsoleLogTag logTag;

        public ConsoleLog(string _command, string text, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            logTag = tag;
            logMessage = text;
            command = _command;
            logText.text = command + "\n" + DeveloperConsoleUtils.CreateTagString(logTag) + logMessage;
        }

        public void SetBaseLog(string _command)
        {
            command = _command;
            logText.text = command + "\n";
        }
        public void SetLog(string text, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            logTag = tag;
            logMessage = text;
            logText.text += DeveloperConsoleUtils.CreateTagString(logTag) + logMessage;
        }

        public string GetLogMessage() => logMessage;
        public string GetLog() => DeveloperConsoleUtils.CreateTagString(logTag) + logMessage;
    }
}

