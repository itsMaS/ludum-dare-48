using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorTargetImage : ColorTarget<Image>
{
    public override void SetColors(Color[] color)
    {
        target.color = color[0];
    }
}
