using System.Collections;
using UnityEngine;

public class UnitMiner : Unit
{
    #region Variables
    [SerializeField] private float harvestRate;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        unitType = UnitType.Miner;
    }

    private void Start()
    {
        StartCoroutine(AutoHarvest());
    }

    private void Update()
    {
        if (_navMesh.remainingDistance <= range)
            arrived = true;

        if (target != null)
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


    private IEnumerator AutoHarvest()
    {
        while (true)
        {
            if (inrange)
                if (target != null)
                    Harvest();

            yield return new WaitForSeconds(harvestRate);
        }
    }

    private void Harvest()
    {
        Mine targ = (Mine)target;
        
        targ.Harvest();

        Ray ray = new Ray(_transform.position, _transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            GlobalManager.Instance.FXImpact(hit.point);
    }
}
