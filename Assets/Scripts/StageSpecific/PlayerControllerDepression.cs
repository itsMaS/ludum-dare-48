using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerDepression : PlayerController
{
    [SerializeField] int maxEnemies = 5;

    [SerializeField] float connectionSlow = 1;

    public void Connect(EnemyDepression enemy)
    {
        speed = Mathf.Max(2, speed - connectionSlow);
        StoryManager.Instance.AdvanceStage(0.1f);
        CameraShake();
    }

    public override bool CanSpawn()
    {
        return EnemyDepression.EnemyCount < maxEnemies;
    }

    private void OnEnable()
    {
        StoryManager.Instance.onUpdateProgression.AddListener(UpdateProgression);
    }
    private void OnDisable()
    {
        StoryManager.Instance.onUpdateProgression.RemoveListener(UpdateProgression);
    }
    void UpdateProgression(float progression)
    {
        if (progression >= 0.9f)
        {

            DOVirtual.DelayedCall(4, () =>
            {
                EnemyDepression.ending = true;
                DOVirtual.DelayedCall(3, () =>
                {
                    StoryManager.Instance.AdvanceStage(0.4f);
                    PlayerController.Instance.speed = 40;
                });
            });
        }
    }
}
