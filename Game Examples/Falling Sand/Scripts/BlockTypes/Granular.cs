using UnityEngine;
using System.Collections;

public abstract class Granular : FallingSandBlock
{

    public override abstract FallingSandBlock NewBlock
    {
        get;
    }

    public override abstract Color BlockColor
    {
        get;
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (BottomBlock == null)
        {
            SetThisBlock(chunk, null);
            SetBottomBlock(chunk, NewBlock);

        }
        else if (BottomBlock is Liquid)
        {
            // If Hit water liquid disperse the grain
            switch (Random.Range(0, 20))
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    SetThisBlock(chunk, BottomBlock);
                    SetBottomBlock(chunk, this);
                    break;
                case 4:
                    SetLeftBlock(chunk, LeftBlock);
                    SetThisBlock(chunk, this);
                    break;
                case 5:
                    SetRightBlock(chunk, RightBlock);
                    SetThisBlock(chunk, this);
                    break;
            }
        }
    }

}
