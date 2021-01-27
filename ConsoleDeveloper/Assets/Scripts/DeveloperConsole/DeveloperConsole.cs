using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace BlackRefactory.Console
{
    public enum ConsoleState { ACTIVE, DISACTIVE};

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }
        public static List<GameObject> consoleLogList { get; private set; }

        [Header("UI Components")]
        [SerializeField]
        private GameObject consoleCanvas = null;
        [SerializeField]
        private GameObject consoleConfigurationCanvas = null;
        [SerializeField]
        private TMPro.TextMeshProUGUI consoleText = null;
        [SerializeField]
        private TMPro.TextMeshProUGUI inputText = null;
        [SerializeField]
        TMPro.TMP_InputField consoleInput = null;

        [Header("Internal Configuration")] 
        [SerializeField]
        private KeyCode openKey = KeyCode.Tab;
        [SerializeField]
        private KeyCode enterKey = KeyCode.Return;
        [SerializeField] 
        private bool showDebugLogsConsole = false;
        [SerializeField]
        private Transform newLogParent = null;
        [SerializeField]
        private GameObject logPrefab = null;
        [SerializeField]
        private int maxLogsOnConsole = 20;
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
            if(showDebugLogsConsole)
                Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            if(showDebugLogsConsole)
                Application.logMessageReceived -= HandleLog;
        }

        private void Start()
        {
            InitializeSetup();
            CreateCommands();
        }
        
        private void Update()
        {
            //Activation and desactivation
            if (Input.GetKeyDown(openKey))
            {
                if (ConsoleIsActive())
                    CloseDeveloperConsole();
                else
                    OpenDeveloperConsole();
            }
            
            if (ConsoleIsActive())
            {
                if (Input.GetKeyDown(enterKey))
                {
                    string commandString = DeveloperConsoleUtils.CleanStringZeroWhidthSpace(inputText.text);
                    
                    if (commandString != "")
                    {
                        CreateBaseLog(commandString);
                        
                        RegisterCommandOnCommandsHistoryList(commandString);
                        ProcessCommand(ParseInput(commandString));

                        ResetConsoleAfterProcess();
                    }
                }

                if (CanCommandHistoryMove(KeyCode.DownArrow))
                {
                    NextHistoryCommand();
                }
                if (CanCommandHistoryMove(KeyCode.UpArrow))
                {
                    PreviousHistoryCommand();
                }

            }
        }

        /// <summary>
        /// Add a message to console with a specific tag.
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="tag">Tag of the message, by default it's defined to ConsoleLogTag.LOG</param>
        public void AddMessageToConsole( string msg, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            consoleLogList[consoleLogList.Count - 1].GetComponent<ConsoleLog>().SetLog(msg, tag);
            
            if (consoleLogList.Count > maxLogsOnConsole)
            {
                Destroy(consoleLogList[0]);
                consoleLogList.Remove(consoleLogList[0]);
            }
        }

        public static bool IsValidCommand(string command)
        {
            return Commands.ContainsKey(command.ToLower());
        }
        
        
        //Private functions
        
        private void OpenDeveloperConsole()
        {
            consoleCanvas.SetActive(true);
            consoleState = ConsoleState.ACTIVE;
            consoleInput.ActivateInputField();
        }

        private void CloseDeveloperConsole()
        {
            consoleCanvas.SetActive(false);
            consoleState = ConsoleState.DISACTIVE;
            if (consoleConfigurationCanvas.activeInHierarchy)
                consoleConfigurationCanvas.SetActive(false);
        }

        private bool ConsoleIsActive()
        {
            return consoleState == ConsoleState.ACTIVE;
        }

        private void ToggleConfigurationPanel()
        {
            consoleConfigurationCanvas.SetActive(!consoleConfigurationCanvas.activeInHierarchy);
        }

        private void InitializeSetup()
        {
            actualIndexCommandSelected = 0;
            consoleText.text += "\n";
            consoleCanvas.SetActive(false);

            if (!DeveloperConsoleUtils.IsEventSystemOnScene())
            {
                DeveloperConsoleUtils.CreateEventSystem(this.transform);
            }
        }

        private void ProcessCommand( string[] _input)
        {
            if (!DeveloperConsoleUtils.IsInputValid(_input) || !Commands.ContainsKey(_input[0]))
            {
                AddMessageToConsole( DeveloperConsoleMessages.UnrecognizedCommandMessage, ConsoleLogTag.WARNING);
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

        //Add all commands to the console
        private void CreateCommands()    
        {
            var allCommandsTypes = Assembly.GetAssembly(typeof(ConsoleCommand)).GetTypes().Where(t => typeof(ConsoleCommand).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var commandType in allCommandsTypes)
            {
                ConsoleCommand command = Activator.CreateInstance(commandType) as ConsoleCommand;
                AddCommandToConsole(command);
                
                Debug.Log($"Command: {command.Name} has been added.");
            }
        }
        
        //Add the command to the dictionary from the consoleCommand
        private void AddCommandToConsole(ConsoleCommand _command)
        {
            if (!Commands.ContainsKey(_command.Command))
            {
                Commands.Add(_command.Command, _command);
            }
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
        
        private bool CanCommandHistoryMove(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode) && commandsHistoryList.Count > 0 && consoleInput.isFocused;
        } 

        private void PreviousHistoryCommand()
        {
            consoleInput.text = commandsHistoryList[actualIndexCommandSelected];

            if (actualIndexCommandSelected > 0)
            {
                actualIndexCommandSelected--;
            }
        }

        private void NextHistoryCommand()
        {
            consoleInput.text = commandsHistoryList[actualIndexCommandSelected];

            if (actualIndexCommandSelected < commandsHistoryList.Count - 1)
            {
                actualIndexCommandSelected++;
            }
        }
        
        private void CreateBaseLog(string command)
        {
            GameObject go = Instantiate(logPrefab, newLogParent);
            ConsoleLog log = go.GetComponent<ConsoleLog>();
            log.SetBaseLog(command);
            consoleLogList.Add(go);
        }
        private void CreateBaseLog()
        {
            GameObject go = Instantiate(logPrefab, newLogParent);
            consoleLogList.Add(go);
        }
        
        private void ResetConsoleAfterProcess()
        {
            consoleInput.ActivateInputField();
            actualIndexCommandSelected = commandsHistoryList.Count - 1;
        }

        //Handles Debug. message of Unity
        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            CreateBaseLog();
            AddMessageToConsole(logMessage, DeveloperConsoleUtils.LogTypeToMessageType(type));
        }
        
    }
}