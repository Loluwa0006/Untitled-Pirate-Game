using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    protected virtual void ApplyGravity(float gravity, bool clamp = true)
    {
        if (clamp)
        {
            if (Player.RigidBody.linearVelocity.y < Player.PlayerStats.MaxFallSpeed)
            {
                var speedNormalized = Player.RigidBody.linearVelocity.normalized;
                var extraSpeed = Vector3.Dot(Vector3.down * gravity, speedNormalized);
                if (extraSpeed > 0)
                {
                    gravity = 0.0f;
                }
            }
        }
        Player.RigidBody.AddForce(gravity * Vector3.down, ForceMode.Acceleration);

    }

    protected void AirborneMovement(Vector2 movementDirection, float acceleration)
    {
        Vector3 moveDirection = movementDirection.x * viewCamera.transform.right + movementDirection.y * viewCamera.transform.forward;

        if (moveDirection.magnitude < MOVEMENT_DEADZONE) return;

        Vector2 currentSpeed = new Vector2(Player.RigidBody.linearVelocity.x, Player.RigidBody.linearVelocity.z);
        float lateralSpeed = currentSpeed.magnitude;
        Vector2 lateralAddition = new(moveDirection.x * acceleration, moveDirection.z * acceleration);


        var turnAngle = Vector2.Angle(currentSpeed, currentSpeed + lateralAddition);

        bool turning = (turnAngle <= Player.PlayerStats.TurnAngle);

        if (!turning) //too sharp, decelerating
        {
            var value01 = Mathf.InverseLerp(Player.PlayerStats.TurnAngle + 0.001f, 180, turnAngle);
            var scaler = Player.PlayerStats.TurnAngleSpeedLostCurve.Evaluate(value01);
            lateralAddition *= scaler;
        }

        if (currentSpeed.magnitude >= Player.PlayerStats.MoveSpeed)
        {
            var speedNormalized = currentSpeed.normalized;
            var extraSpeed = Vector2.Dot(lateralAddition, speedNormalized);
            if (extraSpeed > 0)
            {
                lateralAddition -= extraSpeed * speedNormalized;
            }
        }

        Player.RigidBody.AddForce(new Vector3(lateralAddition.x, 0, lateralAddition.y), ForceMode.VelocityChange);
    }
}
public static class WormStateUtilities
{

    public static RaycastHit raycastResult;

    public static RaycastHit RaycastResult { get => raycastResult; private set => raycastResult = value; }

    
    public static bool AimingAtWorm(PlayerController Player, LayerMask swingMask)
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Player.PlayerStats.MaxRodRange, swingMask, QueryTriggerInteraction.Collide))
        {
            
            raycastResult = hitInfo;
            if (hitInfo.collider.gameObject.layer == LayerMask.GetMask("Worm"))
            {
                raycastResult.point = hitInfo.collider.bounds.center;
            }
            return true;
        }
        return false;
    }
}

