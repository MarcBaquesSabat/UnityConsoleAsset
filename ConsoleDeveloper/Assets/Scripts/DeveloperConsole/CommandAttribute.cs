using System;

namespace BlackRefactory.Console
{
    public class CommandAttribute : Attribute
    {
        public string commandName;
        public string commandDescription;

        public CommandAttribute(string commandName, string commandDescription = "")
        {
            this.commandName = commandName;
            this.commandDescription = commandDescription;
        }


    }
}
