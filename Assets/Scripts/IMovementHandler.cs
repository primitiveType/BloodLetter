public interface IMovementHandler
{
    void AddMovementModifier(MovementModifierHandle handle);
    
    bool IsGrounded { get; }
}