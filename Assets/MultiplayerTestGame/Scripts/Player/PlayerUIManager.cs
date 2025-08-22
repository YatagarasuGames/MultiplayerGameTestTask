using UnityEngine;
using Mirror;
public class PlayerUIManager : NetworkBehaviour
{
    [SerializeField] private GameObject _nicknameChangeMenu;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CreateLocalUI();
    }

    private void CreateLocalUI()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        Instantiate(_nicknameChangeMenu, canvas.transform);
    }
}
