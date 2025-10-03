public interface IState
{
    void Enter();
    void Update();
    void Exit();
    bool CanProcess(ICommand command); // key: abilities decide if allowed
}
