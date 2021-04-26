using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StoryController : MonoBehaviour
{
    public static StoryController Instance;

    private void Awake()
    {
        Instance = this;
    }


    [System.Serializable]
    public class StoryEvent
    {
        public UnityEvent onEvent;
        public float storyThreshold;
        public bool completed = false;
    }

    [SerializeField] private List<StoryEvent> Events = new List<StoryEvent>();

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
        List<StoryEvent> executedEvents = Events.FindAll(eve => !eve.completed && eve.storyThreshold >= progress);
        executedEvents.ForEach(eve =>
        {
            eve.completed = true;
            eve.onEvent.Invoke();
        });
    }
}
