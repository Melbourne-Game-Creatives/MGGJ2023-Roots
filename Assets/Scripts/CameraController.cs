using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera controlledCamera;
    public Vector3 lookAngle = new Vector3(30,0,0);
    public Vector3 lookDistance = new Vector3(0,0,-24);
    public float maxDistance = -60;
    public float minDistance = -4;
    public float moveSpeed = 10f;
    public float moveBoostSpeed = 20f;
    public float zoomSpeedMultiplier = 2f;
    public bool canRotate = true;
    
    public void Awake()
    {
        var cameraTransform = controlledCamera.transform;
        cameraTransform.localPosition = Vector3.zero;
        cameraTransform.localRotation = Quaternion.identity;
        cameraTransform.Rotate(lookAngle);
        cameraTransform.Translate(lookDistance);
    }

    public void Update()
    {
        if (IsRightMouseButtonDown() && canRotate)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        if (IsRightMouseButtonUp() && canRotate)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (IsCameraRotationAllowed())
        {
            var mouseMovement = GetInputLookRotation();// * (Time.deltaTime * 5);
            mouseMovement.y = -mouseMovement.y;

            const float mouseSensitivityFactor = 0.75f;

            transform.Rotate(new Vector3(0, mouseMovement.x * mouseSensitivityFactor, 0));
            lookAngle.x = math.max(math.min(lookAngle.x + mouseMovement.y * mouseSensitivityFactor, 85f), 20f);
        }

        var scrollDelta = Input.mouseScrollDelta;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            scrollDelta.x = scrollDelta.y = 0;
        }

        lookDistance.z = math.max(math.min(lookDistance.z + scrollDelta.y * zoomSpeedMultiplier, minDistance), maxDistance);

        var translation = GetInputTranslationDirection() * Time.deltaTime;

        if (IsBoostPressed())
        {
            translation *= moveBoostSpeed;
        }
        else
        {
            translation *= moveSpeed;
        }
        
        transform.Translate(translation);

        var cameraTransform = controlledCamera.transform;
        cameraTransform.localPosition = Vector3.zero;
        cameraTransform.localRotation = Quaternion.identity;
        cameraTransform.Rotate(lookAngle);
        cameraTransform.Translate(lookDistance);
    }
    
    private bool IsRightMouseButtonDown()
    {
        return Input.GetMouseButtonDown(1);
    }

    private bool IsRightMouseButtonUp()
    {
        return Input.GetMouseButtonUp(1);
    }
    
    private bool IsCameraRotationAllowed()
    {
        return Input.GetMouseButton(1) && canRotate;
    }

    private bool IsBoostPressed()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private Vector2 GetInputLookRotation()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * 10;
    }
    
    private Vector3 GetInputTranslationDirection()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        return direction;
    }
}