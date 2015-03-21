using UnityEngine;
using System.Collections;

public class FallingTreeGrowth : Solid
{

    public override Color BlockColor
    {
        get { return new Color(0.823529f, 0.411765f, 0.117647f); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingTreeGrowth(); }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);
        MoveGrowth(chunk);
        
        if (TopBlock == null || RightBlock == null || LeftBlock == null)
        {
            GrowTree(chunk);
        }
        else
            SetThisBlock(chunk, new FallingTree());
    }

    private void MoveGrowth(Chunk chunk)
    {
        int direction = Random.Range(-1, 2);

        switch (direction)
        {
            case -1:
                if(LeftBlock is FallingTree)
                SetLeftBlock(chunk, NewBlock);
                SetThisBlock(chunk, new FallingTree());
                break;
            case 0:
                if (TopBlock is FallingTree)
                SetTopBlock(chunk, NewBlock);
                SetThisBlock(chunk, new FallingTree());
                break;
            case 1:
                if (RightBlock is FallingTree)
                SetRightBlock(chunk, NewBlock);
                SetThisBlock(chunk, new FallingTree());
                break;

        }
    }

    private void GrowTree(Chunk chunk)
    {
        int direction = Random.Range(-1, 2);
        int block = Random.Range(0, 25);

        Block b = null;

        if (block == 0)
            b = new VineBlock();
        else
            b = new FallingTree();

        switch (direction)
        {
            case -1:
                if (LeftBlock == null)
                    SetLeftBlock(chunk, b);
                break;
            case 0:
                if (TopBlock == null)
                    SetTopBlock(chunk, b);
                break;
            case 1:
                if (RightBlock == null)
                    SetRightBlock(chunk, b);
                break;
        }

        SetThisBlock(chunk, new FallingTree());
    }
}
