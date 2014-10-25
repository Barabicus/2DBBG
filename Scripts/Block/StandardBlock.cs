using UnityEngine;
using System.Collections;

public class StandardBlock : Block
{
    public override Color BlockColor
    {
        get
        {
            return new Color(0.4f, 0.4f, 0.4f);
        }
    }
}
