using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] fields;
    [SerializeField] private GameObject returnButton;

    private OnlineManagerLOBBY _onlineManager;
    private LobbyError _error;
    #endregion


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _onlineManager = GetComponent<OnlineManagerLOBBY>();
        _error = GetComponent<LobbyError>();

        Base();
    }


    public void Base()
    {
        foreach (GameObject b in buttons)
            b.SetActive(true);

        foreach (GameObject f in fields)
            f.SetActive(false);

        returnButton.SetActive(false);
    }

    public void DisplayInputField(int _index)
    {
        foreach (GameObject b in buttons)
            b.SetActive(false);

        fields[_index].SetActive(true);

        returnButton.SetActive(true);
    }


    public void CreateRoom(TextMeshProUGUI _text)
    {
        _onlineManager.CreateRoom(_text.text);
    }

    public void JoinRoom(TextMeshProUGUI _text)
    {
        _onlineManager.JoinRoom(_text.text);
    }

    public void RoomError(string _roomName, bool _create)
    {
        if (_create)
            _error.SetText("The name '" + _roomName + "' is already used by another player.");
        else
            _error.SetText("The room '" + _roomName + "' does not exist.");
    }
}
