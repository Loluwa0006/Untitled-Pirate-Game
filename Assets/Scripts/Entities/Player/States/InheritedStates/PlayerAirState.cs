using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
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
            Vector2 lateralAddition = new (moveDirection.x * Player.PlayerStats.AirAcceleration, moveDirection.z * Player.PlayerStats.AirAcceleration);
            Vector2 newLateral = new (newSpeed.x + lateralAddition.x, newSpeed.z + lateralAddition.y);
            newLateral = Vector2.ClampMagnitude(newLateral, Player.PlayerStats.MoveSpeed);
            newSpeed = new Vector3(newLateral.x, newSpeed.y, newLateral.y);
            Player.VelocityComponent.OverwriteInternalSpeed(newSpeed);
        }
    }
}
