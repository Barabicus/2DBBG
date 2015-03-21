using UnityEngine;
using System.Collections;

public class FallingSand : Granular
{

    public override Color BlockColor
    {
        get { return Color.yellow; }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingSand(); }
    }

}
