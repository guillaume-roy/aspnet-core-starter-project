namespace ServerCore.Kernel.Commands;

public class CommandResult
{
    public static CommandResult Completed => new CommandResult();

    private CommandResult()
    {
    }
}