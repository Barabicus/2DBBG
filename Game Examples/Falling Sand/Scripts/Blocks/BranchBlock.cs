using UnityEngine;
using System.Collections;

public class BranchBlock : Solid {


    public override Color BlockColor
    {
        get { return new Color(0.6f, 0.2f, 0.3f); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new BranchBlock(); }
    }
}
