using UnityEngine;
using System.Collections;

public class VineBlock : Solid
{

    public int growth = 0;

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        growth = Mathf.Max(growth - 1, 0);

        if (LeftBlock is FallingWater || RightBlock is FallingWater)
            growth = Random.Range(5, 20);
    }

    public override FallingSandBlock NewBlock
    {
        get { return new VineBlock(); }
    }

    public override Color BlockColor
    {
        get { return Color.green; }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);
        if (BottomBlock is FallingWater || LeftBlock is FallingWater || RightBlock is FallingWater || TopBlock is FallingWater)
        {
            SetThisBlock(chunk, new BranchBlock());
            CreateVine(chunk);
            if (Random.Range(0, 3) == 0)
                CreateVine(chunk);
        }
    }

    void CreateVine(Chunk chunk)
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                SetLeftBlock(chunk, new VineBlock() { growth = this.growth });
                break;
            case 1:
                SetRightBlock(chunk, new VineBlock() { growth = this.growth });
                break;
            case 2:
                SetTopBlock(chunk, new VineBlock() { growth = this.growth });
                break;
        }
    }

}
