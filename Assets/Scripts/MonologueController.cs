using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using DG.Tweening;


public class MonologueController : MonoBehaviour
{
    public static MonologueController Instance;
    [SerializeField] private StageDefinition stage;

    [SerializeField] private TextMeshProUGUI monologueText;
    [SerializeField] private float fadeOutDuration = 0.2f;

    List<StageDefinition.Monologue> Monologues = new List<StageDefinition.Monologue>();
    private void Awake()
    {
        stage = StoryManager.Instance.currentStage;
        Instance = this;
        Monologues = new List<StageDefinition.Monologue>(stage.Monologues);
    }
    private void Start()
    {
        UpdateProgression(0);
        StartCoroutine(DisplayMonologue());
    }
    private void OnEnable()
    {
        StoryManager.Instance.onUpdateProgression.AddListener(UpdateProgression);
    }
    private void OnDisable()
    {
        StoryManager.Instance.onUpdateProgression.RemoveListener(UpdateProgression);
    }
    private void UpdateProgression(float progress)
    {
        StageDefinition.Monologue monologue = Monologues.Find(mon => mon.storyThreshold <= progress);
        if(monologue != null)
        {
            Monologues.Remove(monologue);
            AddMonologue(monologue);
        }
    }
    Queue<StageDefinition.Monologue> MonologueQueue = new Queue<StageDefinition.Monologue>();

    public void AddMonologue(StageDefinition.Monologue monologue)
    {
        MonologueQueue.Enqueue(monologue);
    }

    public IEnumerator DisplayMonologue()
    {
        while(true)
        {
            if(MonologueQueue.Count > 0)
            {
                var monologue = MonologueQueue.Dequeue();
                foreach (var item in monologue.Lines)
                {
                    DOTweenAnimation anim = monologueText.gameObject.GetComponent<DOTweenAnimation>();
                    anim.DORestartAllById("Monologue_Start");
                    anim.DORestartAllById(item.effect.ToString());
                    monologueText.SetText(item.line);
                    PlayerController.Instance.SetEmotion(item.emotion);
                    yield return new WaitForSeconds(item.duration);
                    anim.DORestartAllById("Monologue_End");
                    DOTween.Pause(gameObject);
                    yield return new WaitForSeconds(monologueText.DOFade(0, fadeOutDuration).Duration());
                }
            }
            yield return null;
        }
    }
}
