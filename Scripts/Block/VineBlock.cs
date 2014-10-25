using UnityEngine;
using System.Collections;

public class VineBlock : Block, IUpdateable {

    public override Color BlockColor
    {
        get { return Color.green; }
    }

    public void Update(Chunk chunk)
    {
        Vector2 index;
        if (chunk.GetIndexFromBlock(this, out index))
        {
            chunk.SetBlockAtIndex(index, new BranchBlock());
            CreateVine(Random.Range(0, 5) == 0, Random.Range(0, 5) == 0, chunk, index);
        }
    }

    void CreateVine(bool left, bool right, Chunk chunk, Vector2 index)
    {
        chunk.SetBlockAtIndex(index + new Vector2(0, 1), new VineBlock());
        if(left)
            chunk.SetBlockAtIndex(index + new Vector2(-1, 0), new VineBlock());
        if(right)
            chunk.SetBlockAtIndex(index + new Vector2(1, 0), new VineBlock());

    }

}
