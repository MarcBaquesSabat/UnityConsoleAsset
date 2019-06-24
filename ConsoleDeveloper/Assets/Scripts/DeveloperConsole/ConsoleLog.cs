using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class ConsoleLog : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI logText = null;
        private string logMessage = "";
        private ConsoleLogTag logTag;

        public ConsoleLog(string text, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            logTag = tag;
            logMessage = text;
            logText.text = DeveloperConsoleUtils.CreateTagString(logTag) + logMessage;
        }

        public void SetLog(string text, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            logMessage = text;
            logTag = tag;
            logText.text = DeveloperConsoleUtils.CreateTagString(logTag) + logMessage;
        }

        public string GetLogMessage() => logMessage;
        public string GetLog() => DeveloperConsoleUtils.CreateTagString(logTag) + logMessage;
    }
}

