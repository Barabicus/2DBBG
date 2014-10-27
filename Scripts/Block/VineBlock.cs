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
            
            CreateVine(chunk, index);
            if (Random.Range(0, 50) == 0)
                CreateVine(chunk, index);
        }
    }

    void CreateVine(Chunk chunk, Vector2 index)
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                chunk.SetBlockAtIndex(index + new Vector2(-1, 0), new VineBlock());
                break;
            case 1:
                chunk.SetBlockAtIndex(index + new Vector2(1, 0), new VineBlock());
                break;
            case 2:
                chunk.SetBlockAtIndex(index + new Vector2(0, 1), new VineBlock());
                break;
        }
    }

}
