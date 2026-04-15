using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{

    [SerializeField] Transform lookTarget;
    [SerializeField] Transform playerTransform;
    [SerializeField] InputActionReference aim;
    [SerializeField] float sensitivity;

    Vector2 lookDirection;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        lookDirection += aim.action.ReadValue<Vector2>();

        lookDirection.y = Mathf.Clamp(lookDirection.y, -90, 90);
        playerTransform.localRotation = Quaternion.Euler(0, lookDirection.x, 0);
        lookTarget.localRotation = Quaternion.Euler(-lookDirection.y, 0, 0);
    }
}
