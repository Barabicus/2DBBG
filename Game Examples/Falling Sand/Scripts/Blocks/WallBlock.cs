using UnityEngine;
using System.Collections;

public class WallBlock : Solid
{

    public override Color BlockColor
    {
        get { return Color.black; }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new WallBlock(); }
    }

}
