using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[DefaultExecutionOrder(-10)]
public class StoryManager : MonoBehaviour
{
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    bool playing1 = false;

    public enum Stage { Denial = 0, Anger = 1, Bargaining = 2, Depression = 3, Acceptance = 4, Testing = -1}

    public Stage startingStage = Stage.Denial;

    [SerializeField] AnimationCurve DifficultyCurve;
    [SerializeField] AnimationCurve HandicapCurve;

    [SerializeField] float fallSpeed;
    [SerializeField] float fallInitiateTime;

    [SerializeField] StageDefinition[] Stages;

    public StageDefinition currentStage;


    public bool transition = false;

    private int index = 0;
    public float Difficulty 
    { 
        get 
        { 
            return DifficultyCurve.Evaluate(StageProgression) - HandicapCurve.Evaluate(Time.timeSinceLevelLoad); 
        } 
    }

    public UnityEvent<float> onUpdateProgression;

    public static StoryManager Instance;
    private void Awake()
    {
        Instance = this;
        index = (int)startingStage;
        if(index >= 0)
        currentStage = Stages[index];
    }
    private void Start()
    {
        source1.DOFade(0.5f, 5);
        if(index >= 0)
            LoadNewStage(currentStage, Vector2.zero);
        SwitchMusic(currentStage);
    }
    public float StageProgression { get; private set; }

    public void AdvanceStage(float amount, bool postStage = false)
    {
        if (transition && !postStage) return;
        StageProgression += amount;
        onUpdateProgression.Invoke(StageProgression);
        if(!postStage && StageProgression >= 1)
        {
            StartCoroutine(NextStage());
        }
    }
    private IEnumerator NextStage()
    {
        CameraController.Instance.SetCamera(false);
        transition = true;
        DOVirtual.Float(0, index != (int)Stage.Depression ? fallSpeed : -fallSpeed, fallInitiateTime, value => PlayerController.Instance.DownwardsPull = value);

        float time = 0;
        while(time < currentStage.postGameTime)
        {
            AdvanceStage(Time.deltaTime, true);
            time += Time.deltaTime;
            yield return null;
        }
        Transition();
    }

    private void Ending()
    {
        UIManager.Instance.Ending();
    }

    private void Transition()
    {
        Debug.Log($"Progression : {index}/{Stages.Length}");
        if (Stages.Length <= index) return;
        Debug.Log($"TRANSITION");

        currentStage = Stages[++index];
        SwitchMusic(currentStage);
        ColorManager.Instance.SwitchScheme(currentStage.colorStyle, 10, () =>
        {
            if (index != (int)Stage.Acceptance)
            {
                UnloadStage(currentStage);
            }
            else
            {
                DOVirtual.DelayedCall(currentStage.postGameTime, () => Ending());
            }
        });
    }
    private void UnloadStage(StageDefinition stage)
    {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        PlayerController.Instance.speed = 0;
        DOVirtual.Float(PlayerController.Instance.DownwardsPull, 0, fallInitiateTime, value => PlayerController.Instance.DownwardsPull = value).OnComplete(() =>
        {
            transition = false;
            StageProgression = 0;
            Destroy(StoryController.Instance.gameObject);
            LoadNewStage(stage, playerPosition);
        });
    }

    void SwitchMusic(StageDefinition stage)
    {
        if(playing1)
        {
            source2.enabled = true;
            source2.clip = stage.music;
            source2.Play();
            source1.DOFade(0, 5).OnComplete(() => source1.enabled = false);
            source2.DOFade(1, 5);
            playing1 = false;
        }
        else
        {
            source1.enabled = true;
            source1.clip = stage.music;
            source1.Play();
            source2.DOFade(0, 5).OnComplete(() => source2.enabled = false); ;
            source1.DOFade(1, 5);
            playing1 = true;
        }
    }
    private void LoadNewStage(StageDefinition stage, Vector2 playerPosition)
    {

        //ColorManager.Instance.SwitchScheme(stage.colorStyle, 0);
        Instantiate(stage.StagePrefab, transform.position, Quaternion.identity);
        PlayerController.Instance.gameObject.transform.position = playerPosition;
        onUpdateProgression.Invoke(StageProgression);
    }

    public void ImpedeStory(float amount)
    {
        if (transition) return;
        StageProgression -= amount;
        StageProgression = Mathf.Max(0, StageProgression);
        onUpdateProgression.Invoke(StageProgression);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F1) || Input.GetKey(KeyCode.KeypadPlus))
        {
            AdvanceStage(Time.deltaTime * 0.4f);
        }
    }
}
