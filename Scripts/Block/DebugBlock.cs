using UnityEngine;
using System.Collections;

public class DebugBlock : Block, ITickable {

    public override Color BlockColor
    {
        get { return Color.cyan; }
    }

    public void Update(Chunk chunk)
    {
        //Vector2 index;
        //if (chunk.GetIndexFromBlock(this, out index))
        //{
        //    chunk.SetBlockAtIndex(index, null);
        //    chunk.SetBlockAtIndex(index + new Vector2(1, 0), new DebugBlock());
        //}
    }
}
