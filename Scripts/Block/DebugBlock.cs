using UnityEngine;
using System.Collections;

public class DebugBlock : Block, IUpdateable {

    public override Color BlockColor
    {
        get { return Color.cyan; }
    }

    public void Update(Chunk chunk)
    {
        Vector2 index;
        if (chunk.GetIndexFromBlock(this, out index))
        {

        }
    }
}
