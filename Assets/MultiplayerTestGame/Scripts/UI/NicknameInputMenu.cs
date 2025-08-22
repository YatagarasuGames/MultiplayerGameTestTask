using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class NicknameInputMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private Button _submitButton;
    private PlayerNicknameLoader _nicknameLoader;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _nicknameLoader = NetworkClient.localPlayer.transform.Find("Nickname").GetComponent<PlayerNicknameLoader>();
        _submitButton.onClick.AddListener(HandleSubmitButtonClicked);
    }

    private void HandleSubmitButtonClicked()
    {
        if (_nicknameInput.text != string.Empty) _nicknameLoader.Nickname = _nicknameInput.text;
        gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _submitButton.onClick.RemoveAllListeners();
    }
}
