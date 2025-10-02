public interface ICommand
{
    void Execute(PlayerController controller);
}

// Example commands
public class AttackCommand : ICommand
{
    public void Execute(PlayerController controller)
    {
        controller.AbilitySystem.TryActivate("Attack");
    }
}

public class DashCommand : ICommand
{
    public void Execute(PlayerController controller)
    {
        controller.AbilitySystem.TryActivate("Dash");
    }
}
