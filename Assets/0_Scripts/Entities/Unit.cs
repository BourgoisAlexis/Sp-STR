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

    protected Transform _transform;
    protected Entity target;
    protected int attackDamage = 1;
    protected int range = 3;
    protected int speed = 5;
    protected float attackRate = 0.5f;
    #endregion


    protected override void Awake()
    {
        base.Awake();
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.speed = speed;
        _transform = transform;
    }

    protected virtual void Start()
    {
        StartCoroutine(AutoAttack());
    }

    private void Update()
    {
        if (_navMesh.remainingDistance < 1)
            arrived = true;

        if (target == null && arrived)
                DetectTarget();
        else if (target)
        {
            Vector3 direction = target.transform.position - transform.position;
            float magnitude = Vector3.Magnitude(direction);

            if (magnitude <= range)
            {
                _transform.LookAt(target.transform.position);
                inrange = true;
                arrived = true;
                SetDestination(_transform.position);
            }
            else
            {
                inrange = false;
                arrived = false;
                SetDestination(target.transform.position);
            }
        }
    }


    public void SetDestination(Vector3 _target)
    {
        _navMesh.SetDestination(_target);
        arrived = false;
    }

    public void SetTarget(Entity _target)
    {
        target = _target;
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
                SetTarget(ent);
                oldDist = magnitude;
            }
        }
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            if(target != null)
                if(inrange)
                    Attack();

            yield return new WaitForSeconds(attackRate);
        }
    }

    protected virtual void Attack()
    {
        if (target.UpdateHealth(-attackDamage) <= 0)
        {
            target.Destroyed();
            target = null;
            return;
        }

        Ray ray = new Ray(_transform.position, _transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            GlobalManager.Instance.FXImpact(hit.point);
    }
}
