using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTargetLineRenrerer : ColorTarget<LineRenderer>
{
    public override void SetColors(Color[] color)
    {
        target.startColor = color[0];
        target.endColor = color[1];
    }
    protected override int ColorCount() => 2;
}
