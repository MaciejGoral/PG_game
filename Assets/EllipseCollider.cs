using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class EllipseCollider : MonoBehaviour
{
    public float xRadius = 1f;
    public float yRadius = 1f;
    public int numPoints = 16; // half of the original number

    void Start()
    {
        Vector2[] points = new Vector2[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            float angle = (float)i / (numPoints - 1) * Mathf.PI; // changed from 2 * Mathf.PI to Mathf.PI
            points[i] = new Vector2(Mathf.Cos(angle) * xRadius, Mathf.Sin(angle) * yRadius);
        }

        GetComponent<EdgeCollider2D>().points = points;
    }
}
