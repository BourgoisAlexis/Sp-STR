using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float margin;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask groundLayer;

    private float initialY;
    private Vector3 movement;
    #endregion


    private void Awake()
    {
        initialY = cameraTransform.position.y;
    }

    private void Update()
    {
        movement = Vector3.zero;
        
        AdjustXZ();
        AdjustY();

        cameraTransform.Translate(movement.normalized * cameraSpeed * Time.deltaTime, Space.World);
    }


    private void AdjustXZ()
    {
        Vector2 mousePos = Input.mousePosition;

        //Up Down
        if (Input.GetKey(KeyCode.Z) || mousePos.y > Screen.height - margin)
            movement.z = -1;
        else if (Input.GetKey(KeyCode.S) || mousePos.y < margin)
            movement.z = 1;

        //Right Left
        if (Input.GetKey(KeyCode.D) || mousePos.x > Screen.width - margin)
            movement.x = -1;
        else if (Input.GetKey(KeyCode.Q) || mousePos.x < margin)
            movement.x = 1;
    }

    private void AdjustY()
    {
        RaycastHit hit;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            Vector3 pos = cameraTransform.position;
            cameraTransform.position = new Vector3(pos.x, hit.point.y + initialY, pos.z);
        }
    }
}