using UnityEngine;
using System.Collections;

public class VineBlock : FallingSandCoreBlock
{

    public override Color BlockColor
    {
        get { return Color.green; }
    }

    public override void Update(Chunk chunk)
    {
        base.Update(chunk);
        if (BottomBlock is FallingWater || LeftBlock is FallingWater || RightBlock is FallingWater || TopBlock is FallingWater)
        {
            SetThisBlock(chunk, new BranchBlock());
            CreateVine(chunk);
            if (Random.Range(0, 10) == 0)
                CreateVine(chunk);
        }
    }

    void CreateVine(Chunk chunk)
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                SetLeftBlock(chunk, new VineBlock());
                break;
            case 1:
                SetRightBlock(chunk, new VineBlock());
                break;
            case 2:
                SetTopBlock(chunk, new VineBlock());
                break;
        }
    }

}
