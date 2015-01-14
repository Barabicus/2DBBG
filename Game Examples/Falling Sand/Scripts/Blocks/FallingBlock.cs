using UnityEngine;
using System.Collections;

public class FallingBlock : Block, ITickable {


    public override Color BlockColor
    {
        get { return Color.magenta; }
    }

    public void Update(Chunk chunk)
    {
        Vector2 index;
        if (chunk.GetIndexFromBlock(this, out index) )
        {
            if (chunk.GetBlockAtIndex(index + new Vector2(0, -1)) is WallBlock || chunk.GetBlockAtIndex(index + new Vector2(0, -1)) is FallingBlock)
            {
                return;
            }
            chunk.SetBlockAtIndex(index, null);
            chunk.SetBlockAtIndex(index + new Vector2(0, -1), new FallingBlock());
        }
    }
}
