using UnityEngine;
using System.Collections;

public class FallingWater : FallingSandCoreBlock
{

    public override Color BlockColor
    {
        get { return new Color(0.15f, 0.4f, 0.65f); }
    }


    public override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (BottomBlock == null)
        {
            SetThisBlock(chunk, null);
            SetBottomBlock(chunk, new FallingWater());
        }
        else if (RightBlock == null || LeftBlock == null)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    if (RightBlock == null)
                    {
                        SetThisBlock(chunk, null);
                        SetRightBlock(chunk, new FallingWater());
                    }
                    break;
                case 1:
                    if (LeftBlock == null)
                    {
                        SetThisBlock(chunk, null);
                        SetLeftBlock(chunk, new FallingWater());
                    }
                    break;
            }
        }
    }

}
