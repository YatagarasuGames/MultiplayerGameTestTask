using UnityEngine;
using Mirror;
using TMPro;

public class PlayerNicknameLoader : NetworkBehaviour
{
    [SyncVar(hook = "OnNicknameChanged")]
    public string Nickname;
    [SerializeField] private TMP_Text _nicknameObject;

    private void OnNicknameChanged(string oldNick, string newNick)
    {
        _nicknameObject.text = newNick;
    }

    public override void OnStartAuthority()
    {
        base.OnStartClient();
        Nickname = string.Format("User{0}", Random.Range(10_000_000, 99_999_999));
    }
}
