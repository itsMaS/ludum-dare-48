using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAnger : PlayerController
{
    float baseSpeed;

    [SerializeField] private float hitPenalty;
    [SerializeField] private float killAward = 0.15f;
    [SerializeField] private float dashAmount = 5;

    [SerializeField] float explosionRadius;
    [SerializeField] float ExplosionThreshold;

    [SerializeField] ParticleSystem ExplosionParticle;
    [SerializeField] SpriteRenderer ExplosionRing;
    [SerializeField] Vector2 ringBounds = new Vector2(0.2f, 2);

    [SerializeField] AnimationCurve ringCurve;
    [SerializeField] AnimationCurve opacityCurve;
    [SerializeField] ContactFilter2D filter;
    [SerializeField] SpriteRenderer attackAreaSr;
    [SerializeField] Color areaFlashColor;

    float explosionAccum = 0;
    bool inProgress = true;
    private void Start()
    {
        baseSpeed = speed;
        ExplosionRing.GetComponent<ColorTargetSpriteRenderer>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StoryManager.Instance.ImpedeStory(hitPenalty);
        EnemyAnger enemy = collision.GetComponentInParent<EnemyAnger>();
        AudioManager.PlaySound("Hurt", 0.5f, 0.3f);
        enemy.Kill();
    }
    protected override void Update()
    {
        base.Update();
        if(StoryManager.Instance.StageProgression < 1)
        {
            ExplosionBehavior();
        }
        else if(inProgress)
        {
            inProgress = false;
            StopBehavior();
        }
    }
    private void StopBehavior()
    {
        ExplosionRing.gameObject.SetActive(false);
        attackAreaSr.gameObject.SetActive(false);
    }
    private void ExplosionBehavior()
    {
        explosionAccum += Time.deltaTime;

        float lerp = Mathf.InverseLerp(0, ExplosionThreshold, explosionAccum);

        Color color = ExplosionRing.color;
        color.a = opacityCurve.Evaluate(lerp);
        ExplosionRing.color = color;

        ExplosionRing.transform.localScale = Vector2.one * Mathf.Lerp(explosionRadius * 0.2f, ringBounds.x, ringCurve.Evaluate(lerp));
        if(explosionAccum >= ExplosionThreshold)
        {
            explosionAccum = 0;
            Explode();
        }
    }

    private void Explode()
    {
        CameraShake();
        AudioManager.PlaySound("Explosion", 0.5f, 0.1f);
        ExplosionParticle.Play();
        List<Collider2D> Results = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, explosionRadius/2, filter, Results);

        attackAreaSr.transform.localScale = Vector3.one * explosionRadius;
        attackAreaSr.color = areaFlashColor;
        attackAreaSr.DOFade(0, 1);

        bool awarded = false;
        Results.FindAll(item => item.tag != "Player").ForEach(item =>
        {
            if(!awarded)
            {
                StoryManager.Instance.AdvanceStage(killAward);
                awarded = true;
            }
            item.GetComponent<EnemyAnger>().Kill();
        });
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
