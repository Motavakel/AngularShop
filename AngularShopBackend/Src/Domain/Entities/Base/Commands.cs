namespace Domain.Entities.Base;

public interface ICommands
{
    public bool IsActive { get; set; }
}

public class Commands : ICommands
{
    public bool IsActive { get; set; }
}