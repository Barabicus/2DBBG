using UnityEngine;
using System.Collections;

public class FallingVirus : FallingSandCoreBlock
{
    public System.Type blockType;

    public override Color BlockColor
    {
        get { return Color.green; }
    }

    public override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (BottomBlock == null)
        {
            SetThisBlock(chunk, null);
            SetBottomBlock(chunk, new FallingVirus());
        }
        else if (RightBlock == null || LeftBlock == null)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    if (RightBlock == null)
                    {
                        SetThisBlock(chunk, null);
                        SetRightBlock(chunk, new FallingVirus());
                    }
                    break;
                case 1:
                    if (LeftBlock == null)
                    {
                        SetThisBlock(chunk, null);
                        SetLeftBlock(chunk, new FallingVirus());
                    }
                    break;
            }
        }

    }

}
