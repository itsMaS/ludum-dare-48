using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorTargetSpriteRenderer : ColorTarget<SpriteRenderer>
{
    public override void SetColors(Color[] colors)
    {
        target.color = colors[0];
    }
}
