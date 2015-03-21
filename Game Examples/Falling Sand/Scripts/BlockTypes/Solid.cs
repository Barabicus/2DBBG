using UnityEngine;
using System.Collections;

public abstract class Solid : FallingSandBlock
{
    public override abstract Color BlockColor
    {
        get;
    }

    public override abstract FallingSandBlock NewBlock
    {
        get;
    }
}
