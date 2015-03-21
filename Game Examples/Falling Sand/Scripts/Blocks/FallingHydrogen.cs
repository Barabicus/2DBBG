using UnityEngine;
using System.Collections;

public class FallingHydrogen : Gas
{

    public override FallingSandBlock NewBlock
    {
        get { return new FallingHydrogen(); }
    }

    public override Color BlockColor
    {
        get { return new Color(0.854902f, 0.647059f, 0.12549f); }
    }
}
