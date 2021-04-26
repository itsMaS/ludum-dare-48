using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTargetParticleSystem : ColorTarget<ParticleSystem>
{
    public override void SetColors(Color[] color)
    {
        target.startColor = color[0];
    }
}
