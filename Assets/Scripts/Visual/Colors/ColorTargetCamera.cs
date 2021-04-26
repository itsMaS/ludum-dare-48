using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ColorTargetCamera : ColorTarget<Camera>
{
    public override void SetColors(Color[] colors)
    {
        target.backgroundColor = colors[0];
    }
}
