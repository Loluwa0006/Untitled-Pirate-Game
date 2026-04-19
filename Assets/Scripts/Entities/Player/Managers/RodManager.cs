using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class RodManager : MonoBehaviour
{
    [SerializeField] LineRenderer rodLine;
    [SerializeField] LayerMask swingMask;
    [SerializeField] PlayerController player;


   

    Camera gameCamera;

    Vector3 middlePointOfViewport = new(0.5f, 0.5f);


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
    private void Start()
    {
        gameCamera = Camera.main;
        RetractRod();
    }

    public void StartSwing()
    {
        if (WormStateUtilities.raycastResult.collider != null)
        {
            grappleInfo.collider = WormStateUtilities.raycastResult.collider;
            grappleInfo.offset = Vector3.zero;

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

    public void EnableRod()
    {
        if (WormStateUtilities.raycastResult.collider != null)
        {
            grappleActive = true;
            rodLine.enabled = true;

            grappleInfo.collider = WormStateUtilities.raycastResult.collider;
            grappleInfo.offset = Vector3.zero;
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
