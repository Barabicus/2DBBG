using UnityEngine;
using System.Collections;

public class FallingSteel : Solid {

    public override Color BlockColor
    {
        get { return new Color(0.372549f, 0.619608f, 0.627451f); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingSteel(); }
    }

}
