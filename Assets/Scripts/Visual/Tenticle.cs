using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Tenticle : MonoBehaviour
{
    [SerializeField] private int points;
    [SerializeField] private Transform targetTr;

    LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lr.positionCount = points;
        for (int i = 1; i < points; i++)
        {

        }
    }
}
