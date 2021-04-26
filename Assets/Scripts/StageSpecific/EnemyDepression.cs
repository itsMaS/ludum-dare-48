using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDepression : Enemy
{
    [SerializeField] Line lc;

    [SerializeField] Vector2 speedDistribution = new Vector2(10, 15);

    [SerializeField] float cableDistance = 1;

    [SerializeField] Vector2 enemyRadius = new Vector2(1,3);

    Vector2 playerOffset;

    float speed;
    bool connected = false;

    [SerializeField] float swingAmplitude = 1;
    [SerializeField] float swingFreq = 1;
    [SerializeField] float reactiveness = 2;

    [SerializeField] Vector2 lineLength = new Vector2(3,5);

    Vector2 velocity;
    float seed;

    public static int EnemyCount { get; private set; }

    private void Start()
    {
        seed = Random.value * 10000;
        EnemyCount++;
        speed = Random.Range(speedDistribution.x, speedDistribution.y);
        playerOffset = Random.insideUnitCircle.normalized * Random.Range(enemyRadius.x, enemyRadius.y);
        lc.length = Random.Range(lineLength.x, lineLength.y);
    }
    bool line = false;

    public static bool ending = false;
    private void Update()
    {
        if(line)
        {
            lc.point2.position = player.position;
        }
        transform.position = lc.point1.position;
        if(!ending)
        {
            Vector2 defaultPos = (Vector2)player.position + playerOffset;
            Vector2 target;
            if(line)
            {
                target = (Vector2)player.position + (defaultPos - (Vector2)player.position).normalized * lc.length + Vector2.up * Mathf.Sin(seed + Time.time * swingFreq) * swingAmplitude;
            }
            else
            {
                target = defaultPos;
            }
            transform.position = Vector2.SmoothDamp(transform.position, target, ref velocity, reactiveness, speed);

            if(connected || Vector2.Distance(transform.position, player.position) < cableDistance)
            {
                if(!connected)
                {
                    connected = true;
                    Connect();
                }
            }
        }
        else
        {
            if (!line) DestroyImmediate(gameObject);

            if(!end)
            {
                end = true;
                DOVirtual.DelayedCall(Random.Range(0, 0.5f), () => End());
                End();
            }
        }
    }
    private void End()
    {
        TrailRenderer tr = GetComponentInChildren<TrailRenderer>();
        DOVirtual.Float(tr.time, 0, 1, value => tr.time = value);
        Vector2 currentPos = transform.position;
        DOVirtual.Float(lc.maxDrop, 0, 0.5f, value => lc.maxDrop = value).SetEase(Ease.InOutSine).OnComplete(() =>
        {
        DOVirtual.Float(0, 1, 2f, value => transform.position = Vector2.Lerp(currentPos, player.position, value)).OnComplete(() =>
            {
                transform.parent = player;
            });
        });
    }

    bool end = false;
    private void Kill()
    {
        EnemyCount--;
    }
    private void Connect()
    {
        AudioManager.PlaySound("RopeHit");
        DOVirtual.Float(0, 1, 0.5f, value =>
        {
            lc.point2.position = Vector2.Lerp(transform.position, player.position, value);

        }).OnComplete(() =>
        {
            ((PlayerControllerDepression)PlayerController.Instance).Connect(this);
            DOVirtual.Float(0, 3, 1f, value => lc.maxDrop = value).SetEase(Ease.InOutSine);
            line = true;
        });
    }
}
