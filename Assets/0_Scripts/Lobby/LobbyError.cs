using System.Collections;
using TMPro;
using UnityEngine;

public class LobbyError : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;


    private void Awake()
    {
        errorText.text = "";
    }

    public void SetText(string _error)
    {
        errorText.text = _error;
        StartCoroutine(TextDelay());
    }
    
    private IEnumerator TextDelay()
    {
        yield return new WaitForSeconds(3f);
        errorText.text = "";
    }
}
