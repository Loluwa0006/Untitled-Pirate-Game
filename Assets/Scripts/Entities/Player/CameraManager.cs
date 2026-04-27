using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{

    [SerializeField] Transform lookTarget;
    [SerializeField] Transform playerTransform;
    [SerializeField] InputManager inputManager;

    Vector2 lookDirection;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        var lookInput = inputManager.GetLookDirection();

        lookDirection += new Vector2(lookInput.x * inputManager.Sensitivity.x, lookInput.y * inputManager.Sensitivity.y);

        lookDirection.y = Mathf.Clamp(lookDirection.y, -90, 90);
        
    }

    private void FixedUpdate()
    {
        playerTransform.localRotation = Quaternion.Euler(0, lookDirection.x, 0);
        lookTarget.localRotation = Quaternion.Euler(-lookDirection.y, lookDirection.x, 0);
    }
}