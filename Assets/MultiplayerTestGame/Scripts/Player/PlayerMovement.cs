using System.Collections;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField, SyncVar] private float _speed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;


    public override void OnStartClient()
    {
        base.OnStartClient();
        //if (isLocalPlayer) foreach (var meshRenderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()) meshRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isOwned) return;

        HandleInputAndAnimation();

        if (isClient)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (input != Vector2.zero) CmdSendMovementInput(input);
        }
    }

    private void HandleInputAndAnimation()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _animator.SetBool("IsWalking", input.magnitude >= Mathf.Epsilon);
    }

    [Command]
    private void CmdSendMovementInput(Vector2 input)
    {
        ServerMove(input);
    }

    [Server]
    private void ServerMove(Vector2 input)
    {
        Vector3 movementDirection = new Vector3(input.x * _speed, input.y * _speed);
        Vector3 movementDirection3 = transform.rotation * new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.y);

        if (movementDirection3.magnitude > _speed)
            movementDirection3 = movementDirection3.normalized * _speed;

        _rb.velocity = movementDirection3;

        RpcSyncMovement(_rb.velocity);
    }

    [ClientRpc]
    private void RpcSyncMovement(Vector3 velocity)
    {
        if (isOwned) SmoothCorrectVelocity(velocity);
    }

    private IEnumerator SmoothCorrectVelocity(Vector3 targetVelocity)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startVelocity = _rb.velocity;

        while (elapsed < duration)
        {
            _rb.velocity = Vector3.Lerp(startVelocity, targetVelocity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rb.velocity = targetVelocity;
    }



}
