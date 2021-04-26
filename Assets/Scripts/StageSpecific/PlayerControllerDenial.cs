using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerControllerDenial : PlayerController
{
    public UnityEvent onHit;

    [SerializeField] StageDefinition.Monologue HitMonologue;

    [SerializeField] private float hitPenalty;

    bool hit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StoryManager.Instance.ImpedeStory(hitPenalty);
        DenialEnemy enemy = collision.GetComponentInParent<DenialEnemy>();
        enemy.Kill();

        onHit.Invoke();

        if(!hit)
            //MonologueController.Instance.AddMonologue(HitMonologue);
        hit = true;

        CameraShake();
        AudioManager.PlaySound("Hurt", 0.5f, 0.3f);
        SetEmotion(Emotions.Sad);
        DOVirtual.DelayedCall(1, () => SetEmotion(Emotions.Normal));
    }

    [Header("Main goal")]
    [SerializeField] float stageTime = 60;
    [SerializeField] float incrementInterval = 0.5f;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(incrementInterval);
            StoryManager.Instance.AdvanceStage(1 / stageTime * incrementInterval);
        }
    }
}
