using UnityEngine;
using System.Collections;

public abstract class Liquid : FallingSandBlock {

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
            SetBottomBlock(chunk, this);
        }
        else if (RightBlock == null || LeftBlock == null)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    if (RightBlock == null)
                    {
                        SetThisBlock(chunk, null);
                        SetRightBlock(chunk, this);
                    }
                    break;
                case 1:
                    if (LeftBlock == null)
                    {
                        SetThisBlock(chunk, null);
                        SetLeftBlock(chunk, this);
                    }
                    break;
            }
        }
    }


}
