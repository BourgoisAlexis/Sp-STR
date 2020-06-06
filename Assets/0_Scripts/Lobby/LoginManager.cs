using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject valid;
    [SerializeField] private GameObject createAccount;
    [SerializeField] private GameObject create;
    [Header("Fields")]
    [SerializeField] private TMP_InputField login;
    [SerializeField] private TMP_InputField pass;
    [SerializeField] private TMP_InputField vPass;

    private OnlineManagerLOBBY _onlineManager;
    private LobbyError _error;


    private void Awake()
    {
        _onlineManager = GetComponent<OnlineManagerLOBBY>();
        _error = GetComponent<LobbyError>();

        Base();
    }


    public void Base()
    {
        vPass.gameObject.SetActive(false);
        create.SetActive(false);
        valid.SetActive(true);
        createAccount.SetActive(true);
    }

    public void Creation()
    {
        vPass.gameObject.SetActive(true);
        create.SetActive(true);
        valid.SetActive(false); 
        createAccount.SetActive(false);
    }

    public void CreateAccount()
    {
        if (pass.text != vPass.text)
        {
            _error.SetText("Your valid password does not correspond to your password");
            return;
        }

        _onlineManager.Connect(login.text, pass.text, true);
    }

    public void Login()
    {
        _onlineManager.Connect(login.text, pass.text, false);
    }
}
