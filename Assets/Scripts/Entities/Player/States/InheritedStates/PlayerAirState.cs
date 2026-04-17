using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    protected PlayerStats.JumpInfo jumpInfo;

    protected void ApplyGravity(float gravity)
    {
        var current = Player.VelocityComponent.GetInternalSpeed();
        current.y -= gravity * Time.fixedDeltaTime;
        Player.VelocityComponent.OverwriteInternalSpeed(current);
    }

    protected void AirborneMovement()
    {
        Vector2 movementDirection = Player.PlayerInput.GetMovementDirection();
   
        movementDirection = movementDirection.normalized;
        Vector3 newSpeed = Player.VelocityComponent.GetInternalSpeed();
        float lateralMovement = new Vector2(newSpeed.x, newSpeed.z).magnitude;

        if (lateralMovement <= Player.PlayerStats.MoveSpeed + 0.001f || Vector3.Angle(movementDirection, newSpeed) > Player.PlayerStats.TurnAngle)
        {
            Vector3 moveDirection = movementDirection.x * Player.transform.right + movementDirection.y * Player.transform.forward;
            newSpeed += new Vector3(moveDirection.x * Player.PlayerStats.AirAcceleration, 0, moveDirection.z * Player.PlayerStats.AirAcceleration);
            newSpeed = Vector3.ClampMagnitude(newSpeed, Player.PlayerStats.MoveSpeed);
            Player.VelocityComponent.OverwriteInternalSpeed(newSpeed);
        }
    }
}
