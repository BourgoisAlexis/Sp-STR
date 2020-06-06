using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Selectable : Entity
{
    #region Variables
    [SerializeField] protected float maxHP;

    protected TeamManager _teamManager;
    protected HealthBar _healthBar;


    private float currentHP;
    private bool selected;
    private Color teamColor;
    private e_Teams team;
    private MapIcon _mapIcon;
    private MeshRenderer _meshRenderer;

    //Accessors
    public bool Selected => selected;
    public e_Teams Team => team;
    #endregion


    protected virtual void Awake()
    {
        currentHP = maxHP;

        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _mapIcon = GetComponentInChildren<MapIcon>();
    }

    private void Start()
    {
        StartCoroutine(ManualUpdate());
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


    public void UpdateHealth(int _value)
    {
        currentHP += _value;
        _healthBar.UpdateGraph((currentHP / 100) /  (maxHP / 100));
        
        if (currentHP <= 0)
            Destroyed(false);
    }

    public void CorrectHealth(int _value)
    {
        currentHP = _value;
        UpdateHealth(0);
    }


    private IEnumerator ManualUpdate()
    {
        GlobalManager.Instance.OnlineManager.UpdateHealth(Index, (int)currentHP);
        yield return new WaitForSeconds(1);
    }


    public override void Destroyed(bool _fromNet)
    {
        List<Unit> toClean = new List<Unit>(targetedBy);
        for (int i = 0; i < toClean.Count; i++)
            toClean[i].SetTarget(null, false);

        Destroy(_healthBar.gameObject);
        GlobalManager.Instance.FXExplosion(transform.position);
        _teamManager.RemoveEntity(this);
        GlobalManager.Instance.EntityManager.RemoveEntity(indexInGame);

        if (!_fromNet)
            GlobalManager.Instance.OnlineManager.DestroyUnit(Index);

        Destroy(gameObject);
    }


    public virtual void Select()
    {
        selected = true;
        _meshRenderer.material.color = HelperClass.GetColor(100);
        _mapIcon.ChangeColor(100);
    }

    public virtual void UnSelect()
    {
        selected = false;
        _meshRenderer.material.color = teamColor;
        _mapIcon.ChangeColor((int)team);
    }
}
