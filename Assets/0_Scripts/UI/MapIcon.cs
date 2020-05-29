using UnityEngine;

public class MapIcon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void ChangeColor(int _colorIndex)
    {
        spriteRenderer.color = HelperClass.GetColor(_colorIndex);
    }
}
