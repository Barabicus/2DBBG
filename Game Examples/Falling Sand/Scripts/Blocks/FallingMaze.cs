using UnityEngine;
using System.Collections;

public class FallingMaze : Solid
{

    int xDir = 0, yDir = 0;
    bool dirSet = false;

    System.Type[] acceptedTypes = new System.Type[] { typeof(FallingMaze), typeof(FallingSteel), typeof(WallBlock) };

    public override Color BlockColor
    {
        get { return new Color(0.156863f, 0.156863f, 0.156863f); }
    }

    public override FallingSandBlock NewBlock
    {
        get
        {
            return new FallingMaze() { xDir = this.xDir, yDir = this.yDir, dirSet = this.dirSet };
        }
    }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        if (!dirSet)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    xDir = Random.Range(-1, 2);
                    break;
                case 1:
                    yDir = Random.Range(-1, 2);
                    break;
            }

            dirSet = true;
        }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (Random.Range(0, 250) == 0)
        {
            xDir = Random.Range(-1, 2);
            yDir = 0;
        }
        if (Random.Range(0, 250) == 0)
        {
            yDir = Random.Range(-1, 2);
            xDir = 0;
        }



        switch (xDir)
        {
            case 1:
                if (!(IsOfType(RightBlock, acceptedTypes)) || RightBlock != null)
                {
                    SetRightBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new WallBlock());
                }
                break;
            case -1:
                if (!(IsOfType(LeftBlock, acceptedTypes)) || LeftBlock != null)
                {
                    SetLeftBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new WallBlock());
                }
                break;
        }
        switch (yDir)
        {
            case 1:
                if (!(IsOfType(TopBlock, acceptedTypes))|| TopBlock != null)
                {
                    SetTopBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new WallBlock());
                }
                break;
            case -1:
                if (!(IsOfType(BottomBlock, acceptedTypes)) || BottomBlock != null)
                {
                    SetBottomBlock(chunk, NewBlock);
                    SetThisBlock(chunk, new WallBlock());
                }
                break;
        }

    }

}
