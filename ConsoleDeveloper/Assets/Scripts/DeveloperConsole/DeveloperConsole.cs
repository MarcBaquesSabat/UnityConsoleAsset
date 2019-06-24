using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{
    public enum ConsoleState { ACTIVE, DISACTIVE};

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }
        public static List<GameObject> consoleLogList { get; private set; }

        public ConsoleConfiguration configuration = null;

        [Header("UI Components")]
        [SerializeField]
        private Canvas consoleCanvas = null;
        [SerializeField]
        private TMPro.TextMeshProUGUI consoleText = null;
        [SerializeField]
        private Text inputText = null;
        [SerializeField]
        private InputField consoleInput = null;

        
        [Header("Internal Configuration")]
        [SerializeField]
        private Transform newLogParent = null;
        [SerializeField]
        private GameObject logPrefab = null;
        [SerializeField]
        private int maxLogsOnConsole = 40;
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
            consoleLogList = new List<GameObject>();
            commandsHistoryList = new List<string>();

            DontDestroyOnLoad(this.gameObject);
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

            if (!DeveloperConsoleUtils.IsEventSystemOnScene())
            {
                DeveloperConsoleUtils.CreateEventSystem(this.transform);
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
                        AddMessageToConsole(inputText.text, ConsoleLogTag.NONE);

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

        /// <summary>
        /// Add a message to console with a specific tag.
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="tag">Tag of the message, by default it's defined to ConsoleLogTag.LOG</param>
        public void AddMessageToConsole(string msg, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            GameObject go = CreateLog(msg, tag);

            //Control del numero de logs en la consola
            consoleLogList.Add(go);
            if (consoleLogList.Count > maxLogsOnConsole)
            {
                Destroy(consoleLogList[0]);
                consoleLogList.Remove(consoleLogList[0]);
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
            return Commands.ContainsKey(command.ToLower());
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
                AddMessageToConsole(DeveloperConsoleMessages.UnrecognizedCommandMessage, ConsoleLogTag.WARNING);
            }
            else
            {
                IEnumerable<string> args = _input.Skip(1).Take(_input.Length - 1);
                Commands[_input[0]].RunCommand(args.ToArray());
            }
        }

        //Parse the input by spaces with the first word non case sensitive
        private string[] ParseInput(string input)
        {
            string[] _input = input.Split(null);

            _input[0] = _input[0].ToLower();

            return _input;
        }

        //Modify this function adding all your own created commands
        private void CreateCommands()
        {
            //User commands
            

            //Unity functionality commands
            CommandLoadScene.CreateCommand();
            CommandTimeScale.CreateCommand();
            CommandDisactiveGameObject.CreateCommand();

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


        private GameObject CreateLog(string msg, ConsoleLogTag tag)
        {
            GameObject go = Instantiate(logPrefab, newLogParent);
            ConsoleLog log = go.GetComponent<ConsoleLog>();
            log.SetLog(msg, tag);
            return go;
        }

        //Handles Debug. message of Unity
        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            AddMessageToConsole(logMessage, DeveloperConsoleUtils.LogTypeToMessageType(type));
        }

        //Getters and setters
        public TMPro.TextMeshProUGUI getInputText()
        {
            return consoleText;
        }

        public InputField getInputField()
        {
            return consoleInput;
        }
    }
}