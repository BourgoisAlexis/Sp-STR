using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    #region Variables
    private FXManager _fxManager;
    #endregion


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        _fxManager = GetComponent<FXManager>();
    }


    public void FXExplosion(Vector3 _position)
    {
        _fxManager.InstantiateFX(_fxManager.Explosion, _position);
    }

    public void FXImpact(Vector3 _position)
    {
        _fxManager.InstantiateFX(_fxManager.Impact, _position);
    }
}
