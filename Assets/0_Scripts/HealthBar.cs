using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void UpdateGraph(float _value)
    {
        _image.fillAmount = _value;
    }
}
