namespace SFInput {
public interface IInputController
{
    bool Enabled { get; }
    IInputPredicateManager PredicateManager { get; }

    void Disable();
    void Enable();
}}