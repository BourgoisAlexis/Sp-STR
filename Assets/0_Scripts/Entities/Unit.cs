using UnityEngine;
using UnityEngine.AI;

public enum UnitType
{
    Default,
    Soldier,
    Miner
}


public class Unit : Selectable
{
    #region Variables
    [SerializeField] protected UnitType unitType;

    protected NavMeshAgent _navMesh;
    protected bool arrived = true;
    protected BezierCurve _bezierCurve;
    protected bool inrange;
    protected Transform _transform;
    protected Entity target;
    protected int range;
    protected int speed;


    //Accessors
    public UnitType UnitType => unitType;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.speed = speed;
        _transform = transform;
        _bezierCurve = GetComponentInChildren<BezierCurve>();
    }


    public void SetDestination(Vector3 _desti, bool _fromNet)
    {
        _navMesh.SetDestination(_desti);
        _navMesh.isStopped = false;
        arrived = false;

        if(!_fromNet)
            GlobalManager.Instance.OnlineManager.SetUnitDestination(indexInGame, _desti);
    }

    public virtual void SetTarget(Entity _target, bool _fromNet)
    {
        if (target != null)
            target.RemoveTargetter(this);

        if (_target != null)
        {
            _target.AddTargetter(this);
            _bezierCurve.Activation(true, _target.transform);
        }
        else
            _bezierCurve.Activation(false, null);

        target = _target;

        if(!_fromNet)
            GlobalManager.Instance.OnlineManager.SetUnitTarget(indexInGame, _target == null ? -1 : _target.Index);
    }

    public override void Destroyed(bool _fromNet)
    {
        if (target != null)
            target.RemoveTargetter(this);

        base.Destroyed(_fromNet);
    }
}
