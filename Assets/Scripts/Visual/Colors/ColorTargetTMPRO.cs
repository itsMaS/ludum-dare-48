using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ColorTargetTMPRO : ColorTarget<TextMeshProUGUI>
{
    public override void SetColors(Color[] color)
    {
        target.color = color[0];
    }
}
