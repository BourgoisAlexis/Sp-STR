using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Entity
{
    #region Variables
    private NavMeshAgent _navMesh;
    private bool inrange;
    private bool arrived = true;
    private BezierCurve _bezierCurve;

    protected Transform _transform;
    protected Entity target;
    protected int attackDamage;
    protected int range;
    protected int speed;
    protected float attackRate;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.speed = speed;
        _transform = transform;
        _bezierCurve = GetComponentInChildren<BezierCurve>();
    }

    private void Start()
    {
        StartCoroutine(AutoAttack());
    }

    private void Update()
    {
        if (_navMesh.remainingDistance <= 1)
            arrived = true;

        if (target == null && arrived)
                DetectTarget();
        else if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            float magnitude = Vector3.Magnitude(direction);

            if (magnitude <= range)
            {
                _transform.LookAt(target.transform.position);
                inrange = true;
                _navMesh.isStopped = true;
                arrived = true;
            }
            else
            {
                inrange = false;
                SetDestination(target.transform.position, false);
            }
        }
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


    public void DetectTarget()
    {
        List<Entity> nears = new List<Entity>();
        Collider[] all = Physics.OverlapSphere(_transform.position, range);

        foreach (Collider col in all)
            if (col.GetComponent<Entity>())
                if(col.GetComponent<Entity>().Team != Team)
                    nears.Add(col.GetComponent<Entity>());

        float oldDist = range;

        foreach(Entity ent in nears)
        {
            Vector3 direction = _transform.position - ent.transform.position;
            float magnitude = Vector3.Magnitude(direction);

            if (magnitude < oldDist)
            {
                SetTarget(ent, false);
                oldDist = magnitude;
            }
        }
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            if (inrange)
                if (target != null)
                        Attack();

            yield return new WaitForSeconds(attackRate);
        }
    }

    protected virtual void Attack()
    {
        target.UpdateHealth(-attackDamage);

        Ray ray = new Ray(_transform.position, _transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            GlobalManager.Instance.FXImpact(hit.point);
    }
}
