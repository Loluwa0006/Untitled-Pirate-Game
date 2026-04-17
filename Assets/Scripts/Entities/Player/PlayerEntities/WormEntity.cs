using UnityEngine;

public class WormEntity : MonoBehaviour
{
    [SerializeField] int gravityFreeTime = 7 * 60;
    [SerializeField] float distanceWhereTargetConsideredReached = 8.0f;
    [SerializeField] float flySpeed = 60.0f;
    [SerializeField] float gravity = 8.0f;
    [SerializeField] float maxFallSpeed = 30.0f;

    [SerializeField] MeshRenderer model;
    [SerializeField] Collider swingbox;
    [SerializeField] VelocityComponent velocityManager;

    Vector3 target;
    int gravityTracker = 0;
    bool wormActive = false;
    bool reachedTarget = false;
   public void Fire(Vector3 target, Vector3 startingLocation)
    {
        this.target = target;
        gravityTracker = gravityFreeTime;
        reachedTarget = false;
        transform.position = startingLocation;
        Vector3 direction = (target - startingLocation).normalized;
        velocityManager.OverwriteInternalSpeed(direction * flySpeed);

        wormActive = true;
        model.enabled = true;
        swingbox.enabled = true;
    }

    public void Deactivate()
    {
        swingbox.enabled = false;
        model.enabled = false;
        velocityManager.OverwriteInternalSpeed(Vector3.zero);
        wormActive = false;
    }

    private void FixedUpdate()
    {
        if (!wormActive) return;
        GravityLogic();
        TargetLogic();
    }

    void GravityLogic()
    {
        if (gravityTracker > 0)
        {
            gravityTracker--;
        }
        if (gravityTracker == 0)
        {
            Vector3 currentSpeed = velocityManager.GetInternalSpeed();
            currentSpeed.y -= gravity * Time.fixedDeltaTime;
            if (currentSpeed.y < -Mathf.Abs(maxFallSpeed))
            {
                currentSpeed.y = -Mathf.Abs(maxFallSpeed);
            }
            velocityManager.OverwriteInternalSpeed(currentSpeed);
        }
    }

    void TargetLogic()
    {
        if (reachedTarget) return;
        if (Vector3.Distance(transform.position, target) <= distanceWhereTargetConsideredReached)
        {
            velocityManager.OverwriteInternalSpeed(Vector3.zero);
            reachedTarget = true;
        }
    }


}
