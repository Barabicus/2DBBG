using UnityEngine;
using System.Collections;

public class FallingTree : Solid
{

    System.Type[] acceptedType = new System.Type[] { typeof(FallingWater) };

    public override Color BlockColor
    {
        get { return new Color(0.823529f, 0.411765f, 0.117647f); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingTree(); }
    }


    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);
        if (IsOfType(BottomBlock, acceptedType) && !(LeftBlock is FallingTreeRoot) && !(RightBlock is FallingTreeRoot))
        {
            CreateRoot(chunk);
        }
        if (Random.Range(0, 5) == 0)
            CreateGrowth(chunk);
    }

    private void CreateGrowth(Chunk chunk)
    {
        if (LeftBlock is FallingWater || RightBlock is FallingWater || BottomBlock is FallingWater)
            SetThisBlock(chunk, new FallingTreeGrowth());
    }

    private void CreateRoot(Chunk chunk)
    {
        if (Random.Range(0, 1000) == 0)
            SetThisBlock(chunk, new FallingTreeRoot());
    }

}
