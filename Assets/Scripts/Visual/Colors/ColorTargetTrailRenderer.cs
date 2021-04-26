using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class ColorTargetTrailRenderer : ColorTarget<TrailRenderer>
{
    public override void SetColors(Color[] colors)
    {
        target.startColor = colors[0];
        target.endColor = colors[1];
    }

    protected override int ColorCount() => 2;
}
