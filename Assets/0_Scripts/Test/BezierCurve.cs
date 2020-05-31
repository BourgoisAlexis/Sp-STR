using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    #region Variables
    public Transform PointB;

    [SerializeField] private int resolution;
    [SerializeField] private float height;

    private Vector3 BezierHandle;
    private Transform PointA;
    private int current;
    private LineRenderer _lr;
    #endregion


    private void Awake()
    {
        PointA = transform;
        _lr = GetComponentInChildren<LineRenderer>();

        if(PointB != null)
            Activation(true, PointB);
    }

    private void Update()
    {
        if (PointB != null)
        {
            _lr.positionCount = resolution + 1;
            _lr.SetPositions(CreateCurve());
        }

    }

    private Vector3[] CreateCurve()
    {
        BezierHandle = Vector3.Lerp(PointA.position, PointB.position, 0.5f);
        BezierHandle += Vector3.up * height;
        List<Vector3> positions = new List<Vector3>();

        float increment = 1f / resolution;

        for (float i = 0 ; i <= resolution; i ++)
        {
            Vector3 posA = Vector3.Lerp(PointA.position, BezierHandle, i * increment);
            Vector3 posB = Vector3.Lerp(BezierHandle, PointB.position, i * increment);
            Vector3 point = Vector3.Lerp(posA, posB, i * increment);

            positions.Add(point);
        }

        return positions.ToArray();
    }

    public void Activation(bool _active, Transform _pointB)
    {
        if (_active)
        {
            _lr.gameObject.SetActive(true);
            PointB = _pointB;
        }
        else if (!_active)
            _lr.gameObject.SetActive(false);
    }



    private void OnDrawGizmos()
    {
        if(PointB != null)
        {
            PointA = transform;
            Vector3[] po = CreateCurve();
            Gizmos.color = Color.green;
            for (int i = 0; i < po.Length; i++)
            {
                Gizmos.DrawWireSphere(po[i], 0.1f);
            }
        }
    }
}
