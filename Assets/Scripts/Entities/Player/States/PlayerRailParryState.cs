using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using System;
public class PlayerRailParryState : PlayerBaseState
{
    [SerializeField] LayerMask railLayer;
    [SerializeField] SphereCollider railCollider;
    [SerializeField] SplineAnimate splineAnimator;

    SplineContainer splineToFollow;

    float splineDirection;


    float splineLength;

    public override Type[] statesToAttemptToTransitionTo
    {
        get => new Type[]
        {
           typeof(PlayerShadowstepState), 
        };
    }

    public override void InitializeState(EntityStateMachine stateMachine, Transform owner)
    {
        base.InitializeState(stateMachine, owner);
        if (splineAnimator == null) splineAnimator = Player.GetComponent<SplineAnimate>();
        splineAnimator.AnimationMethod = SplineAnimate.Method.Speed;
        splineAnimator.enabled = false;
    }
    public override void Enter(Dictionary<string, object> message = null)
    {
        base.Enter(message);
        

        if (splineToFollow == null)
        {
            StateMachine.TransitionTo<PlayerFallState>();
            return;
        }
        Player.CameraManager.ControlPlayerRotation = false;
        splineAnimator.enabled = true;

        InitializeSplineMovement();
        Player.PlayerGrounded = true;
        Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Parry].Consume();
        Player.RigidBody.isKinematic = false;
    }

    void InitializeSplineMovement()
    {
        var pointInLocalSpace = splineToFollow.transform.InverseTransformPoint(Player.Collider.bounds.center);
        SplineUtility.GetNearestPoint(splineToFollow.Spline, pointInLocalSpace, out float3 startPosition, out float time);
        Vector3 tangent = Vector3.Normalize(SplineUtility.EvaluateTangent(splineToFollow.Spline, time));
        Vector2 lateralSpeed = new Vector2(Player.RigidBody.linearVelocity.x, Player.RigidBody.linearVelocity.z);
        var velocityProjectedOntoSpline = Vector3.Dot(tangent, lateralSpeed.normalized);
        var facingProjectedOntoSpline = Vector3.Dot(tangent, viewCamera.transform.forward);
        splineDirection = Mathf.Sign(velocityProjectedOntoSpline);
        splineAnimator.Container = splineToFollow;
        splineAnimator.MaxSpeed = Mathf.Abs(
            Mathf.Max(lateralSpeed.magnitude * Player.PlayerStats.PreviousSpeedToRailSpeedRatio * velocityProjectedOntoSpline,
            Player.PlayerStats.RailParryMinimumSpeed)) ;
        splineAnimator.NormalizedTime = time;
        splineLength = splineToFollow.CalculateLength();
    }

    public override void PhysicsProcess()
    {
        float delta = splineAnimator.MaxSpeed / splineLength;
        float timeToAdd = (delta * splineDirection) * Time.fixedDeltaTime;
        splineAnimator.NormalizedTime = Mathf.Clamp01(splineAnimator.NormalizedTime + timeToAdd);
        SplineUtility.Evaluate(splineToFollow.Spline, splineAnimator.NormalizedTime, out float3 splinePoint, out float3 tangent, out float3 upVector);
        Player.RigidBody.MovePosition(splineToFollow.transform.TransformPoint(splinePoint));
        viewCamera.transform.rotation = Quaternion.Euler(tangent.x, -tangent.y, 0);

        if (splineAnimator.NormalizedTime > 0.999f || splineAnimator.NormalizedTime < 0.001f || !Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Parry].ActionPressed)
        {
            StateMachine.TransitionTo<PlayerFallState>();
        }
    }

    Vector3 CalculateExitVelocity(Vector3 tangent)
    {
        Vector3 normalizedTangent = Vector3.Normalize(tangent);

        Vector3 exitVelocity = splineAnimator.MaxSpeed * normalizedTangent * splineDirection;

        if (exitVelocity.y < Player.PlayerStats.RailParryMinimumJump) exitVelocity.y = Player.PlayerStats.RailParryMinimumJump;

        return exitVelocity;
    }
    public override void Exit()
    {
        base.Exit();
        splineAnimator.enabled = false;
        Player.RigidBody.isKinematic = false;
        SplineUtility.Evaluate(splineToFollow.Spline, splineAnimator.NormalizedTime, out float3 position, out float3 tangent, out float3 upVector);
        Player.RigidBody.linearVelocity = CalculateExitVelocity(tangent);
        Player.AnarchyManager.GenerateAnarchy(ScaledGenerationMethod.RailParry);
        Player.CameraManager.ControlPlayerRotation = true;
        viewCamera.transform.rotation = Quaternion.LookRotation(-tangent, upVector);
    }
    public override bool StateAvailable()
    {
        if (Player.PlayerInput.BufferRegistry[InputManager.BufferableInputs.Parry].ActionPressed)
        {
            var overlap = Physics.OverlapSphere(railCollider.bounds.center, railCollider.radius, railLayer, QueryTriggerInteraction.Collide);
            if (overlap.Length > 0)
            {
                splineToFollow = overlap[0].GetComponent<SplineContainer>();
                if (splineToFollow == null) splineToFollow = overlap[0].transform.parent.GetComponent<SplineContainer>();
                return true;
            }
        }
        return false;
    }
}
