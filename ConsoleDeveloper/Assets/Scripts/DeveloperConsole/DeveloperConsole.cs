using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace Console
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
            consoleCanvas.SetActive(false);

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
                        CreateBaseLog(inputText.text);
                        
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
        public void AddMessageToConsole( string msg, ConsoleLogTag tag = ConsoleLogTag.NONE)
        {
            consoleLogList[consoleLogList.Count - 1].GetComponent<ConsoleLog>().SetLog(msg, tag);
            
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
            consoleCanvas.SetActive(true);
            consoleState = ConsoleState.ACTIVE;
            consoleInput.ActivateInputField();
        }

        public void CloseDeveloperConsole()
        {
            consoleCanvas.SetActive(false);
            consoleState = ConsoleState.DISACTIVE;
            if (consoleConfigurationCanvas.activeInHierarchy)
                consoleConfigurationCanvas.SetActive(false);
        }

        public bool ConsoleIsActive()
        {
            return consoleState == ConsoleState.ACTIVE;
        }

        public void ToggleConfigurationPanel()
        {
            consoleConfigurationCanvas.SetActive(!consoleConfigurationCanvas.activeInHierarchy);
        }


        private void ProcessCommand( string[] _input)
        {
            //Command doesnt exist on dictionary
            bool valid = DeveloperConsoleUtils.isInputInvalid(_input);
            Debug.Log(_input[0]);
            bool valid2 = !Commands.ContainsKey(_input[0]);

            if (DeveloperConsoleUtils.isInputInvalid(_input) || !Commands.ContainsKey(_input[0]))
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

        //Modify this function adding all your own created commands
        private void CreateCommands()
        {
            var allCommandsTypes = Assembly.GetAssembly(typeof(ConsoleCommand)).GetTypes().Where(t => typeof(ConsoleCommand).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var commandType in allCommandsTypes)
            {
                ConsoleCommand command = Activator.CreateInstance(commandType) as ConsoleCommand;
                command.AddCommandToConsole();
            }

            Debug.Log(Commands);
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

        //Handles Debug. message of Unity
        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            if()
            CreateBaseLog();
            AddMessageToConsole(logMessage, DeveloperConsoleUtils.LogTypeToMessageType(type));
        }

        //Getters and setters
        public TMPro.TextMeshProUGUI getInputText()
        {
            return consoleText;
        }
        public TMPro.TMP_InputField getInputField()
        {
            return consoleInput;
        }
    }
}