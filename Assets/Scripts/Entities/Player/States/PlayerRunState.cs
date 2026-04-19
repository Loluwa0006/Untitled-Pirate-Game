

public class PlayerRunState : PlayerGroundedMovementState
{
    public override bool StateAvailable()
    {
        return PlayerGrounded && Player.PlayerInput.GetMovementDirection().magnitude > MOVEMENT_DEADZONE;
    }
}

