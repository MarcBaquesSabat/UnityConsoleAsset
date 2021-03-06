﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Console
{
    public enum ConsoleState { ACTIVE, DISACTIVE};

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        [Header("UI Components")]
        public Canvas consoleCanvas;
        public ScrollRect scrollRect;
        public TMPro.TextMeshProUGUI consoleText;
        public Text inputText;
        public InputField consoleInput;

        [SerializeField]
        private int maxCommandsListSize = 20;

        private List<string> commandsHistoryList;
        private int actualIndexCommandSelected = 0;
        private ConsoleState consoleState = ConsoleState.DISACTIVE;

        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
            commandsHistoryList = new List<string>();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void Start()
        {
            actualIndexCommandSelected = 0;
            consoleText.text += "\n";
            consoleCanvas.gameObject.SetActive(false);

            if (!IsEventSystemOnScene())
            {
                CreateEventSystem();
            }

            CreateCommands();
        }

        private void Update()
        {
            //Activation and desactivation
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (ConsoleIsActive())
                    CloseDeveloperConsole();
                else
                    OpenDeveloperConsole();
            }

            //Console active
            if (ConsoleIsActive())
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (inputText.text != "")
                    {
                        AddMessageToConsole(inputText.text);

                        RegisterCommandOnCommandsHistoryList(inputText.text);
                        ProcessCommand(ParseInput(inputText.text));

                        ResetConsole();
                    }
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && commandsHistoryList.Count > 0 && consoleInput.isFocused)
                {
                    consoleInput.text = commandsHistoryList[actualIndexCommandSelected];

                    if (actualIndexCommandSelected < commandsHistoryList.Count - 1)
                    {
                        actualIndexCommandSelected++;
                    }
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && commandsHistoryList.Count > 0 && consoleInput.isFocused)
                {
                    consoleInput.text = commandsHistoryList[actualIndexCommandSelected];

                    if (actualIndexCommandSelected > 0)
                    {
                        actualIndexCommandSelected--;
                    }
                }

            }
        }

        //Add the command to the dictionary from the consoleCommand
        public static void AddCommadsToConsole(string _name, ConsoleCommand _command)
        {
            if (!Commands.ContainsKey(_name))
            {
                Commands.Add(_name, _command);
            }
        }

        public static bool isValidCommand(string command)
        {
            return Commands.ContainsKey(command);
        }

        public void OpenDeveloperConsole()
        {
            consoleCanvas.gameObject.SetActive(true);
            consoleState = ConsoleState.ACTIVE;
        }

        public void CloseDeveloperConsole()
        {
            consoleCanvas.gameObject.SetActive(false);
            consoleState = ConsoleState.DISACTIVE;
        }

        public bool ConsoleIsActive()
        {
            return consoleState == ConsoleState.ACTIVE;
        }

        private void ProcessCommand( string[] _input)
        {
            //Command doesnt exist on dictionary
            if (DeveloperConsoleUtils.isInputInvalid(_input) || !Commands.ContainsKey(_input[0]))
            {
                Debug.LogWarning(DeveloperConsoleMessages.UnrecognizedCommandMessage);
            }
            else
            {
                IEnumerable<string> args = _input.Skip(1).Take(_input.Length - 1);
                Commands[_input[0]].RunCommand(args.ToArray());
            }
        }

        public void AddMessageToConsole(string msg)
        {
            consoleText.text += msg + "\n";
        }

        private string[] ParseInput(string input)
        {
            input = input.ToLower();

            string[] _input = input.Split(null);

            return _input;
        }

        //Modify this function adding all your own created commands
        private void CreateCommands()
        {
            //Created commands
            
            //Unity functionality commands
            CommandLoadScene.CreateCommand();

            //Basic commands
            CommandHelp.CreateCommand();
            CommandDescriptionCommand.CreateCommand();
            CommandClearConsole.CreateCommand();
            CommandQuit.CreateCommand();
        }

        //Register commands whether are valid or not
        private void RegisterCommandOnCommandsHistoryList(string _command)
        {
            if(commandsHistoryList.Count >= maxCommandsListSize)
            {
                commandsHistoryList.RemoveAt(0);
            }
            commandsHistoryList.Add(_command);
        }

        private void ResetConsole()
        {
            consoleInput.ActivateInputField();
            actualIndexCommandSelected = commandsHistoryList.Count - 1;
        }

        //Handles Debug. message of Unity
        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            string _message = "[" + type.ToString() + "] " + logMessage;
            AddMessageToConsole(_message);
        }

        private bool IsEventSystemOnScene()
        {
             return (FindObjectOfType<EventSystem>() != null);
        }

        private void CreateEventSystem()
        {
            Debug.LogWarning(DeveloperConsoleMessages.MissingAndCreateEventSystemMessage);
            GameObject go = new GameObject();
            go.name = "EventSystem";
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }

        
    }
}