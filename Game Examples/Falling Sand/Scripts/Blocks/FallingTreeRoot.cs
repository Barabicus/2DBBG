using UnityEngine;
using System.Collections;

public class FallingTreeRoot : Solid {

    public int direction = 0;

    public override Color BlockColor
    {
        get { return new Color(0.823529f, 0.411765f, 0.117647f); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingTreeRoot() { direction = this.direction }; }
    }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        direction = Random.Range(-1, 2);
    }


    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (!(TopBlock is FallingWater) && !(BottomBlock is FallingWater) && !(RightBlock is FallingWater) && !(LeftBlock is FallingWater))
        {
            SetThisBlock(chunk, new FallingTree());
            return;
        }

        if (Random.Range(0, 50) == 0)
        {
            ChangeDirection();
        }

        if (Random.Range(0, 80) == 0)
            Grow(chunk);
    }

    private void Grow(Chunk chunk)
    {
        switch (direction)
        {
            case -1:
                if (LeftBlock is FallingWater)
                {
                    SetLeftBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new FallingTree());
                }
                break;
            case 0:
                if (BottomBlock is FallingWater)
                {
                    SetBottomBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new FallingTree());
                }
                break;
            case 1:
                if (RightBlock is FallingWater)
                {
                    SetRightBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new FallingTree());
                }
                break;
        }
    }

    private void ChangeDirection()
    {
        direction = Random.Range(-2, 1);
    }
}
