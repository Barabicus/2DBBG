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
            chunk.SetBlockAtIndex(index, null);
            Block block = chunk.GetBlockAtIndex(index + new Vector2(1, 0));
            if (block == null || block.GetType() != typeof(DebugBlock))
            chunk.SetBlockAtIndex(index + new Vector2(1, 0), new DebugBlock());
          //  if (chunk.GetBlockAtIndex(index - new Vector2(1, 0)).GetType() != typeof(DebugBlock))
          //  chunk.SetBlockAtIndex(index - new Vector2(1, 0), new DebugBlock());
        }
    }
}
