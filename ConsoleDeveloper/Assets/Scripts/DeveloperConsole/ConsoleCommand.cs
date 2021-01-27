namespace BlackRefactory.Console
{
    public abstract class ConsoleCommand
    {
        //Name of the command
        public abstract string Name { get; protected set; }
        //Command to type on console
        public abstract string Command { get; protected set; }
        //Brief description of what does the command
        public abstract string Description { get; protected set; }

        public abstract string Help { get; protected set; }

        //Logic of the command
        public abstract void RunCommand(string[] args);
    }
}
