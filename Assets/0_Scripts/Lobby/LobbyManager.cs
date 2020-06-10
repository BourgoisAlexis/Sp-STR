using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject createB;
    [SerializeField] private GameObject joinB;
    [SerializeField] private GameObject validB;
    [SerializeField] private GameObject returnB;
    [SerializeField] private TMP_InputField field;

    private OnlineManagerLOBBY _onlineManager;
    private LobbyError _error;
    private bool create;
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
        createB.SetActive(true);
        joinB.SetActive(true);

        returnB.SetActive(false);
        validB.SetActive(false);
        field.gameObject.SetActive(false);
    }

    public void DisplayInputField(bool _create)
    {
        create = _create;

        createB.SetActive(false);
        joinB.SetActive(false);

        returnB.SetActive(true);
        validB.SetActive(true);
        field.gameObject.SetActive(true);
    }

    public void Valid()
    {
        if (create)
            _onlineManager.CreateRoom(field.text);
        else
            _onlineManager.JoinRoom(field.text);
    }

    public void RoomError(string _roomName, bool _create)
    {
        if (_create)
            _error.SetText("The name '" + _roomName + "' is already used by another player.");
        else
            _error.SetText("The room '" + _roomName + "' does not exist.");
    }
}
