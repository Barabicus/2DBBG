using UnityEngine;
using System.Collections;

public class FallingIce : Solid
{
    public override Color BlockColor
    {
        get { return Color.cyan; }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingIce(); }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);


        switch (Random.Range(0, 50))
        {
            case 0:
                if (TopBlock is FallingWater)
                    SetTopBlock(chunk, NewBlock);
                break;
            case 1:
                if (BottomBlock is FallingWater)
                    SetBottomBlock(chunk, NewBlock);
                break;
            case 2:
                if (LeftBlock is FallingWater)
                    SetLeftBlock(chunk, NewBlock);
                break;
            case 3:
                if (RightBlock is FallingWater)
                    SetRightBlock(chunk, NewBlock);
                break;
        }

        if (TopBlock is FallingFire || BottomBlock is FallingFire || RightBlock is FallingFire || LeftBlock is FallingFire)
        {
            //for (int x = -5; x < 5; x++)
            //    for (int y = -5; y < 5; y++)
            //    {
            //        Block fb = chunk.GetBlockAtIndex(indexX + x, indexY + y);
            //        if (fb != null && fb is FallingIce)
            //            chunk.SetBlockAtIndex(indexX + x, indexY + y, new FallingWater());
            //    }

            for (int i = 0; i < 50; i++)
                Melt(indexX, indexY, i, chunk);

        }
    }

    public void Melt(int x0, int y0, int radius, Chunk ch)
    {
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            if (ch.GetBlockAtIndex(x + x0, y + y0) is FallingIce)
                ch.SetBlockAtIndex(x + x0, y + y0, new FallingWater());
            if (ch.GetBlockAtIndex(y + x0, y + y0) is FallingIce)
                ch.SetBlockAtIndex(y + x0, y + y0, new FallingWater());
            if (ch.GetBlockAtIndex(-x + x0, y + y0) is FallingIce)
                ch.SetBlockAtIndex(-x + x0, y + y0, new FallingWater());
            if (ch.GetBlockAtIndex(-y + x0, x + y0) is FallingIce)
                ch.SetBlockAtIndex(-y + x0, x + y0, new FallingWater());
            if (ch.GetBlockAtIndex(-x + x0, -y + y0) is FallingIce)
                ch.SetBlockAtIndex(-x + x0, -y + y0, new FallingWater());
            if (ch.GetBlockAtIndex(-y + x0, -x + y0) is FallingIce)
                ch.SetBlockAtIndex(-y + x0, -x + y0, new FallingWater());
            if (ch.GetBlockAtIndex(x + x0, -y + y0) is FallingIce)
                ch.SetBlockAtIndex(x + x0, -y + y0, new FallingWater());
            if (ch.GetBlockAtIndex(y + x0, -x + y0) is FallingIce)
                ch.SetBlockAtIndex(y + x0, -x + y0, new FallingWater());
            y++;
            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
    }

}
