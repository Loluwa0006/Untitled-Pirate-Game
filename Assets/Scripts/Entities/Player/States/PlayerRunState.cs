

public class PlayerRunState : PlayerGroundedMovementState
{
    public override bool StateAvailable()
    {
        return Player.PlayerGrounded && Player.PlayerInput.GetMovementDirection().magnitude > MOVEMENT_DEADZONE;
    }
}

