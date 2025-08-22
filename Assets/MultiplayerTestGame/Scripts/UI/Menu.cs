using Mirror;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _ipInput;
    private string _ip;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Screen.SetResolution(720, 480, false);
    }
    public void StartHost()
    {
        if (_ipInput.text == string.Empty) _ip = "localhost";
        else _ip = _ipInput.text;

        NetworkManager.singleton.networkAddress = _ip;
        NetworkManager.singleton.StartHost();
    }

    public void StartClient()
    {
        if (_ipInput.text == string.Empty) _ip = "localhost";
        else _ip = _ipInput.text;

        NetworkManager.singleton.networkAddress = _ip;
        NetworkManager.singleton.StartClient();
    }
}
