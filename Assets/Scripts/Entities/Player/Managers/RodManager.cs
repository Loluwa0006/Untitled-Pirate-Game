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
        EndSwing();
    }

    public void FireRod()
    {
        var cameraRay = gameCamera.ViewportPointToRay(middlePointOfViewport);
        var raycast = Physics.Raycast(cameraRay, out var hitInfo, player.PlayerStats.MaxRodRange, swingMask, QueryTriggerInteraction.Collide);
        if (raycast)
        {
            grappleInfo.collider = hitInfo.collider;
            grappleInfo.offset = hitInfo.point - hitInfo.collider.bounds.center;

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


    public void EndSwing()
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
