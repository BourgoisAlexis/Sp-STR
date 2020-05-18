using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region Variables
    protected float maxHP = 30;
    protected TeamManager _teamManager;

    private float currentHP;
    private bool selected;
    private Color teamColor;
    private e_Teams team;
    private MapIcon _mapIcon;
    private MeshRenderer _meshRenderer;
    private HealthBar _healthBar;
    private UIManager _uiManager;

    //Accessors
    public bool Selected => selected;
    public e_Teams Team => team;
    #endregion


    protected virtual void Awake()
    {
        currentHP = maxHP;
        _meshRenderer = GetComponent<MeshRenderer>();
        if (!_meshRenderer)
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _mapIcon = GetComponentInChildren<MapIcon>();
    }


    public void SetupTeam(e_Teams _team, TeamManager _tm, HealthBar _hp)
    {
        team = _team;
        _teamManager = _tm;
        _healthBar = _hp;

        teamColor = HelperClass.GetColor((int)team);
        _meshRenderer.material.color = teamColor;
        _mapIcon.ChangeColor((int)team);
    }


    public float UpdateHealth(int _value)
    {
        currentHP += _value;
        _healthBar.UpdateGraph((currentHP / 100) /  (maxHP / 100));
        return currentHP;
    }

    public void Destroyed()
    {
        Destroy (_healthBar.gameObject);
        GlobalManager.Instance.FXExplosion(transform.position);
        _teamManager.RemoveEntity(this);
        Destroy(gameObject);
    }


    public virtual void Select()
    {
        selected = true;
        _meshRenderer.material.color = HelperClass.GetColor(100);
        _mapIcon.ChangeColor(100);
    }

    public void UnSelect()
    {
        selected = false;
        _meshRenderer.material.color = teamColor;
        _mapIcon.ChangeColor((int)team);
    }
}
