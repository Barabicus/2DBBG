using UnityEngine;
using System.Collections;

public class FallingSand : FallingSandCoreBlock
{

    public override Color BlockColor
    {
        get { return Color.yellow; }
    }

    public override void Update(Chunk chunk)
    {
        base.Update(chunk);
        if (BottomBlock == null)
        {
            SetThisBlock(chunk, null);
            SetBottomBlock(chunk, new FallingSand());

        }
        else if (BottomBlock is FallingWater)
        {
            // If Hit water randomly disperse the sand
            switch(Random.Range(0, 20))
            {
                case 0: case 1: case 2: case 3:
                SetThisBlock(chunk, new FallingWater());
                SetBottomBlock(chunk, new FallingSand());
                break;
                case 4:
                SetLeftBlock(chunk, new FallingSand());
                SetThisBlock(chunk, new FallingWater());
                break;
                case 5:
                SetRightBlock(chunk, new FallingSand());
                SetThisBlock(chunk, new FallingWater());
                break;
            }
        }
    }
}
