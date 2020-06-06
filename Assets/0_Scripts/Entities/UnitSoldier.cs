using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoldier : Unit
{
    #region Variables
    [SerializeField] protected float attackRate;
    [SerializeField] protected int attackDamage;
    #endregion


    protected override void Awake()
    {
        base.Awake();
        unitType = UnitType.Soldier;
    }

    private void Start()
    {
        StartCoroutine(AutoAttack());
    }

    private void Update()
    {
        if (_navMesh.remainingDistance <= range)
            arrived = true;

        if (target == null && arrived)
            DetectTarget();
        else if (target != null)
        {
            Vector3 direction = target.transform.position - _transform.position;
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


    public void DetectTarget()
    {
        List<Selectable> nears = new List<Selectable>();
        Collider[] all = Physics.OverlapSphere(_transform.position, range);

        foreach (Collider col in all)
            if (col.GetComponent<Selectable>())
                if (col.GetComponent<Selectable>().Team != Team)
                    nears.Add(col.GetComponent<Selectable>());

        float oldDist = range;

        foreach (Selectable ent in nears)
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
        Selectable targ = (Selectable)target;

        targ.UpdateHealth(-attackDamage);

        Ray ray = new Ray(_transform.position, _transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            GlobalManager.Instance.FXImpact(hit.point);
    }
}
