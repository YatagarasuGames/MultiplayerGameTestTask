using UnityEngine;
using Mirror;

public class PlayerCameraController : NetworkBehaviour
{
    [SerializeField] private GameObject character;
    public float Sensitivity = 1;
    public float Smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    [SyncVar(hook = nameof(OnRotationChanged))]
    private Vector2 syncVelocity;

    private void Start()
    {
        if (!isOwned)
        {
            GetComponent<AudioListener>().enabled = false;
            GetComponent<Camera>().enabled = false;
        }
    }

    void Update()
    {
        if (!isOwned) return;
        RotateCamera();

    }

    private void RotateCamera()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * Sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / Smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -80, 80);
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        if (isClient)
        {
            CmdUpdateRotation(velocity);
        }  
    }

    [Command]
    private void CmdUpdateRotation(Vector2 newVelocity)
    {
        syncVelocity = newVelocity;
    }

    private void OnRotationChanged(Vector2 oldValue, Vector2 newValue)
    {
        if (!isOwned)
        {
            transform.localRotation = Quaternion.AngleAxis(-newValue.y, Vector3.right);
            character.transform.localRotation = Quaternion.AngleAxis(newValue.x, Vector3.up);
        }
    }
}
