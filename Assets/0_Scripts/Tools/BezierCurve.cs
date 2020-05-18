using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    #region Variables
    public Transform Destination;
    public Transform particles;

    [SerializeField] private int resolution;
    [SerializeField] private int height;

    private Transform starting;
    private Vector3 pointC; //Bezier Handle
    private int current;
    #endregion

    private void Awake()
    {
        starting = transform;
        GenerateCurve();
    }


    private Vector3[] CreateCurve()
    {
        pointC = Vector3.Lerp(starting.position, Destination.position, 0.5f);
        pointC += Vector3.up * height;
        List<Vector3> positions = new List<Vector3>();

        float increment = 1f / resolution;

        for (float i = 0 ; i <= resolution; i ++)
        {
            Vector3 posA = Vector3.Lerp(starting.position, pointC, i * increment);
            Vector3 posB = Vector3.Lerp(pointC, Destination.position, i * increment);
            Vector3 point = Vector3.Lerp(posA, posB, i * increment);

            positions.Add(point);
        }

        return positions.ToArray();
    }

    public void GenerateCurve()
    {
        StartCoroutine(MoveAlong());
    }

    private IEnumerator MoveAlong()
    {
        current = 0;

        while (current < resolution + 1)
        {
            particles.position = CreateCurve()[current];
            current++;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        if (Destination != null)
            StartCoroutine(MoveAlong());
    }




    private void OnDrawGizmos()
    {
        starting = transform;
        Vector3[] po = CreateCurve();
        Gizmos.color = Color.cyan;
        for (int i = 0; i < po.Length; i++)
        {
            Gizmos.DrawWireSphere(po[i], 0.1f);
        }
    }
}
