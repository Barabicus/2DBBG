using UnityEngine;
using System.Collections;

public class FallingWater : Liquid
{

    public override Color BlockColor
    {
        get { return new Color(0.15f, 0.4f, 0.65f); }
    }


    public override FallingSandBlock NewBlock
    {
        get { return new FallingWater(); }
    }
}
