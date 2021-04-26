using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] float dampSpeed;

    CanvasGroup cg;

    Slider slider;

    float velocity;
    float target;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        cg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StoryManager.Instance.onUpdateProgression.AddListener(UpdateSlider);
    }
    private void OnDisable()
    {
        StoryManager.Instance.onUpdateProgression.RemoveListener(UpdateSlider);
    }
    private void UpdateSlider(float progress)
    {
        target = progress;
        if(progress >= 1)
        {
            cg.DOFade(0, 0.5f);
        }
        if(progress <= 0)
        {
            DOVirtual.DelayedCall(1, () => cg.DOFade(1, 3f));
        }
    }
    private void Update()
    {
        slider.value = Mathf.SmoothDamp(slider.value, target, ref velocity, 1 / dampSpeed);
    }
}
