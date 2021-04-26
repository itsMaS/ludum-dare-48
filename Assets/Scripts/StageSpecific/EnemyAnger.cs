using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnger : Enemy
{
    [SerializeField] Vector2 speedDistribution = new Vector2(10,15);

    float speed;
    private void Start()
    {
        speed = Random.Range(speedDistribution.x, speedDistribution.y);
    }

    private void Update()
    {
        Vector2 movement = (player.position - transform.position).normalized;

        transform.Translate(movement * speed * Time.deltaTime);
    }
    public void Kill()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.gameObject.transform.parent = null;
        ps.Play();
        Destroy(gameObject);
        AudioManager.PlaySound("EnemyHit", 0.5f, 0.3f);
    }
}
