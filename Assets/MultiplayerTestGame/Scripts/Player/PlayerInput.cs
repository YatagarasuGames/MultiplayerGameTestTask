using UnityEngine;
using Mirror;

public class PlayerInput : NetworkBehaviour
{
    [SerializeField] private GameObject _cubeToSpawn;
    [SerializeField] private Transform _camera;
    [SerializeField] private PlayerNicknameLoader _playerNicknameLoader;

    private void Update()
    {
        if (!isOwned) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isServer) RpcSendMessage(_playerNicknameLoader.Nickname);
            else CmdSendMessage();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isServer) CreateCube();
            else CmdCreateCube();
        }
    }

    [Command]
    private void CmdCreateCube()
    {
        CreateCube();
    }

    [Server]
    private void CreateCube()
    {
        var temp = Instantiate(_cubeToSpawn);
        temp.transform.position = _camera.position + _camera.forward;
        NetworkServer.Spawn(temp);
    }


    [Command]
    private void CmdSendMessage()
    {
        RpcSendMessage(_playerNicknameLoader.Nickname);
    }

    [ClientRpc]
    private void RpcSendMessage(string senderNickname)
    {
        Debug.Log($"Привет от {senderNickname}");
    }
}
