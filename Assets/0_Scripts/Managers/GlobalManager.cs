using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    #region Variables
    [SerializeField] private TeamManager[] teamManagers;
    
    private FXManager _fxManager;
    private OnlineManagerVERSUS _onlineManager;
    private EntityManager _entityManager;
    private UIManager _uiManager;
    private CameraController _cameraController;
    private UnitSpawner _unitSpawner;
    
    private int playersAlive;
    private bool lost;

    //Accessors
    public TeamManager[] TeamManagers => teamManagers;
    public OnlineManagerVERSUS OnlineManager => _onlineManager;
    public EntityManager EntityManager => _entityManager;
    public UnitSpawner UnitSpawner => _unitSpawner;
    #endregion


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        _fxManager = GetComponent<FXManager>();
        _onlineManager = GetComponent<OnlineManagerVERSUS>();
        _entityManager = GetComponent<EntityManager>();
        _uiManager = GetComponent<UIManager>();
        _cameraController = GetComponent<CameraController>();
        _unitSpawner = GetComponent<UnitSpawner>();

        _entityManager.enabled = false;
        _unitSpawner.enabled = false;
        _cameraController.enabled = false;
    }


    public void FXExplosion(Vector3 _position)
    {
        _fxManager.InstantiateFX(_fxManager.Explosion, _position);
    }

    public void FXImpact(Vector3 _position)
    {
        _fxManager.InstantiateFX(_fxManager.Impact, _position);
    }


    public void SetupPlayerNumber(int _value)
    {
        playersAlive = _value;

        if (_value == 2)
        {
            _uiManager.EndWaitingScreen();
            _entityManager.enabled = true;
            _unitSpawner.enabled = true;
            //_cameraController.enabled = true;
        }
    }


    public void NexusDestroyed(int _index)
    {
        if (_entityManager.teamIndex == _index)
        {
            _entityManager.enabled = false;
            lost = true;
        }

        playersAlive--;

        if (playersAlive <= 1 && !lost)
            _uiManager.Victory();
        else
            _uiManager.Deafeat();
    }
}
