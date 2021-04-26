using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] CanvasGroup overlayImage;
    [SerializeField] float blinkLength = 0.4f;

    private void Awake()
    {
        Instance = this;
    }
    public void Blink(Action onMiddle = null, Action onFinish = null)
    {
        overlayImage.DOFade(1, blinkLength / 2).OnComplete(() =>
        {
            onMiddle?.Invoke();
            overlayImage.DOFade(0, blinkLength / 2)
                .OnComplete(() => onFinish?.Invoke());
        });
    }
    public void Ending()
    {
        overlayImage.gameObject.SetActive(true);
    }
}
