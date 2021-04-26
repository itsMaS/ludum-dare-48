using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenialEnemy : Enemy
{
    [SerializeField] private AnimationCurve EnemySpeedPerDifficulty;

    public float speed = 20;
    public bool duplicated = false;

    private Vector3 moveDirection;
    private void Start()
    {
        if(!duplicated)
            moveDirection = (CameraController.CursorWorldPosition - (Vector2)transform.position).normalized;
        Destroy(gameObject, 10);
        speed = EnemySpeedPerDifficulty.Evaluate(difficulty);
    }
    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        //if (!duplicated && Vector2.Distance(transform.position, player.position) < 3)
        //{
        //    Duplicate();
        //}
    }

    private void Duplicate()
    {
        duplicated = true;
        for (int i = 0; i < 4; i++)
        {
            DenialEnemy enemy = Instantiate(transform.gameObject, transform.position, Quaternion.identity, transform.parent).GetComponent<DenialEnemy>();
            enemy.moveDirection = Random.insideUnitCircle.normalized;
        }
        Destroy(gameObject);
    }

    public void Kill()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.gameObject.transform.parent = null;
        ps.Play();
        Destroy(gameObject);
    }
}
