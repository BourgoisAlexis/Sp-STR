using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingArc : MonoBehaviour
{
    public Transform destination;
    public float velocity;
    public float angle;
    public int resolution;

    private float gravity = 1;
    private float radiantAngle;

    private LineRenderer _lineRenderer;
    private Transform _transform;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        RenderArc();
    }



    //Set LineRenderer Settings
    private void RenderArc()
    {
        _lineRenderer.positionCount = resolution + 1;
        _lineRenderer.SetPositions(CalculateArcArray());
    }

    private Vector3[] CalculateArcArray()
    {
        Vector3[] array = new Vector3[resolution + 1];
        
        radiantAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radiantAngle)) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            array[i] = CalculateArcPoint(t, maxDistance);
        }

        return array;
    }

    private Vector3 CalculateArcPoint(float _t, float _maxDistance)
    {
        float x = _maxDistance * _t;
        float y = x * Mathf.Tan(radiantAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radiantAngle) * Mathf.Cos(radiantAngle)));

        return new Vector3(x, y);
    }
}
