using UnityEngine;

public class RodManager : MonoBehaviour
{
    [SerializeField] LineRenderer rodLine;
    [SerializeField] LayerMask grappleMask;
    [SerializeField] PlayerController player;

    bool grappleActive = true;

    SpringJoint grappleJoint;

    public struct GrappleData
    {
        public Collider collider;
        public Vector3 offset;

        public Vector3 GrapplePosition { get => collider.bounds.center + offset;  }
    }

    GrappleData grappleInfo;

    public GrappleData GrappleInfo { set => grappleInfo = value; get => grappleInfo; }
    public LayerMask GrappleMask { get => grappleMask; }
    private void Start()
    {
        RetractRod();
    }

    public void StartSwing()
    {
        if (GrappleUtilities.RaycastResult.collider != null)
        {
            grappleInfo.collider = GrappleUtilities.RaycastResult.collider;
            grappleInfo.offset = GrappleUtilities.RaycastResult.point - grappleInfo.collider.bounds.center;

            grappleJoint = player.gameObject.AddComponent<SpringJoint>();
            grappleJoint.autoConfigureConnectedAnchor = false;
            grappleJoint.connectedAnchor = grappleInfo.GrapplePosition;

            grappleJoint.massScale = player.PlayerStats.RodSwingMassScale;
            grappleJoint.spring = player.PlayerStats.RodSpring;
            grappleJoint.damper = player.PlayerStats.RodDamper;

            var distance = Vector3.Distance(grappleInfo.GrapplePosition, player.Collider.bounds.center);

            grappleJoint.maxDistance = player.PlayerStats.RodMaxDistanceWithNoSpring * distance;
            grappleJoint.minDistance = player.PlayerStats.RodMinDistanceWithNoSpring * distance;

            grappleActive = true;
            rodLine.enabled = true;
        }
    }

    public void StartDash()
    {
        if (GrappleUtilities.RaycastResult.collider != null)
        {
            grappleActive = true;
            rodLine.enabled = true;

            grappleInfo.collider = GrappleUtilities.RaycastResult.collider;
            grappleInfo.offset = GrappleUtilities.RaycastResult.point - grappleInfo.collider.bounds.center;
        }
    }
    private void FixedUpdate()
    {
        if (grappleJoint != null)
        {
            grappleJoint.connectedAnchor = GrappleInfo.GrapplePosition;
        }
    }


    public void RetractRod()
    {
        grappleActive = false;
        rodLine.enabled = false;
        Destroy(grappleJoint);
    }

    private void LateUpdate()
    {
        if (grappleActive)
        {
            rodLine.SetPosition(0, player.transform.position);
            rodLine.SetPosition(1, grappleInfo.GrapplePosition);
        }
    }
}
