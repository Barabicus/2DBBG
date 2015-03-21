using UnityEngine;
using System.Collections;

public abstract class Gas : FallingSandBlock
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

        switch (Random.Range(0, 3))
        {
            case 0:
                if (TopBlock == null)
                {
                    SetTopBlock(chunk, NewBlock);
                    SetThisBlock(chunk, null);
                }
                break;
            case 1:
                if (LeftBlock == null)
                {
                    SetLeftBlock(chunk, NewBlock);
                    SetThisBlock(chunk, null);
                }
                break;
            case 2:
                if (RightBlock == null)
                {
                    SetRightBlock(chunk, NewBlock);
                    SetThisBlock(chunk, null);
                }
                break;
        }

    }

}
