using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Heartbeat : MonoBehaviour
{
    [SerializeField] int points = 100;
    [SerializeField] float length = 10;
    [SerializeField] float updateInterval = 0.05f;
    [SerializeField] float heartRate = 60;
    [SerializeField] float heartbeatLength = 0.1f;
    [SerializeField] float heartbeatAmplitude = 0.5f;

    public float pointer;

    LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    IEnumerator Start()
    {
        StartCoroutine(HeartRate());

        while(true)
        {
            lr.positionCount = points;
            for (int i = 0; i < points; i++)
            {
                lr.SetPosition(i, new Vector3(i*(length/points), i+1 < points ? lr.GetPosition(i+1).y : pointer,0));
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }
    IEnumerator HeartRate()
    {
        while(true)
        {
            yield return new WaitForSeconds(60 / heartRate);
            StartCoroutine(HeartBeat());
        }
    }
    IEnumerator HeartBeat()
    {
        pointer = heartbeatAmplitude * 0.1f;
        yield return new WaitForSeconds(heartbeatLength*0.1f);
        pointer = 0;
        yield return new WaitForSeconds(heartbeatLength*0.1f);
        pointer = heartbeatAmplitude;
        yield return new WaitForSeconds(heartbeatLength*0.2f);
        pointer = -heartbeatAmplitude * 0.2f;
        yield return new WaitForSeconds(heartbeatLength*0.1f);
        pointer = 0;
        yield return new WaitForSeconds(heartbeatLength*0.4f);
        pointer = heartbeatAmplitude * 0.15f;
        yield return new WaitForSeconds(heartbeatLength*0.2f);
        pointer = heartbeatAmplitude * 0f;
    }
}
