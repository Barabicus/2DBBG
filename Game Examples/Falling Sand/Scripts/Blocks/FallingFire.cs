using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingFire : Gas
{

    public int life = 100;

    System.Type[] _acceptedFire = new System.Type[] { typeof(VineBlock), typeof(BranchBlock), typeof(FallingGrass), typeof(FallingTree), typeof(FallingTreeGrowth), typeof(FallingTreeRoot) };

    public override Color BlockColor
    {
        get
        {
            if (Random.Range(0, 4) == 0)
                return Color.yellow;
            return Color.red;
        }
    }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        if (life == 0)
        {
            SetThisBlock(chunk, null);
            return;
        }

    }

    public override FallingSandBlock NewBlock
    {
        get
        {
            return new FallingFire() { life = Mathf.Max(this.life - 1, 0) };
        }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (life == 0)
        {
            SetThisBlock(chunk, NewBlock);
            return;
        }
        else
            life -= 1;

        if (TopBlock is FallingWater)
        {
            SetTopBlock(chunk, null);
            SetThisBlock(chunk, null);
            return;
        }

        if (IsOfType(TopBlock, _acceptedFire) && Random.Range(0, 2) == 0)
        {
            SetTopBlock(chunk, new FallingFire());
        }
        if (IsOfType(LeftBlock, _acceptedFire) && Random.Range(0, 2) == 0)
        {
            SetLeftBlock(chunk, new FallingFire());
        }
        if (IsOfType(RightBlock, _acceptedFire) && Random.Range(0, 2) == 0)
        {
            SetRightBlock(chunk, new FallingFire());
        }
        if (IsOfType(BottomBlock, _acceptedFire) && Random.Range(0, 2) == 0)
        {
            SetBottomBlock(chunk, new FallingFire());
        }

       
    }

}
