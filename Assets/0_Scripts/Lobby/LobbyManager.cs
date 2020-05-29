using UnityEngine;
using TMPro;
using System.Collections;

public class LobbyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] fields;
    [SerializeField] private GameObject returnButton;

    [SerializeField] private TextMeshProUGUI errorText;
    #endregion


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        errorText.text = "";
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
        GetComponent<OnlineManagerLOBBY>().CreateRoom(_text.text);
    }

    public void JoinRoom(TextMeshProUGUI _text)
    {
        GetComponent<OnlineManagerLOBBY>().JoinRoom(_text.text);
    }

    public void RoomError(string _roomName, bool _create)
    {
        if(_create)
            errorText.text = "The name '" + _roomName + "' is already used by another player.";
        else
            errorText.text = "The room '" + _roomName + "' does not exist.";

        StartCoroutine(TextDelay());
    }

    private IEnumerator TextDelay()
    {
        yield return new WaitForSeconds(2f);
        errorText.text = "";
    }
}
