using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void UpdateGraph(float _value)
    {
        _image.fillAmount = _value;
    }
}
