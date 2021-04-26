using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class ColorManager : MonoBehaviour
{
    public enum BaseColors { A, B, C, D, E }

    public static ColorManager Instance;

    [SerializeField] private ColorStyle startingStyle;

    private ColorStyle currentStyle;

    private void Awake()
    {
        Instance = this;
        currentStyle = Instantiate(startingStyle);
    }
    public void SwitchScheme(ColorStyle style, float duration = 10, Action onComplete = null)
    {
        ColorStyle start = Instantiate(currentStyle);
        ColorStyle end = Instantiate(style);

        DOVirtual.Float(0, 1, duration, value => 
        LerpStyles(start, end, value, ref currentStyle)).OnComplete(() => onComplete?.Invoke());
    }
    private void LerpStyles(ColorStyle a, ColorStyle b, float lerp, ref ColorStyle changing)
    {
        changing.A = Color.Lerp(a.A, b.A, lerp);
        changing.B = Color.Lerp(a.B, b.B, lerp);
        changing.C = Color.Lerp(a.C, b.C, lerp);
        changing.D = Color.Lerp(a.D, b.D, lerp);
        changing.E = Color.Lerp(a.E, b.E, lerp);
    }

    public static Color GetColor(BaseColors type)
    {
#if UNITY_EDITOR
        if (!Instance) Instance = FindObjectOfType<ColorManager>();

        if (!Application.isPlaying) Instance.currentStyle = Instance.startingStyle;
#endif
        // Performance could be improved with a dictionary
        switch (type)
        {
            case BaseColors.A:
                return Instance.currentStyle.A;
            case BaseColors.B:
                return Instance.currentStyle.B;
            case BaseColors.C:
                return Instance.currentStyle.C;
            case BaseColors.D:
                return Instance.currentStyle.D;
            case BaseColors.E:
                return Instance.currentStyle.E;
            default:
                break;
        }
        return Color.black;
    }
}
