using UnityEngine;
using System.Collections;
using System;

public class FallingClone : Solid {

    public FallingSandBlock cloneBlock;

    public override Color BlockColor
    {
        get { return Color.grey; }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingClone(); }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);
        if (TopBlock != null && !(TopBlock is FallingClone))
            cloneBlock = (FallingSandBlock)TopBlock;

        if (cloneBlock != null && BottomBlock == null)
            SetBottomBlock(chunk, cloneBlock.NewBlock);
        else if (BottomBlock is FallingClone)
            ((FallingClone)BottomBlock).cloneBlock = cloneBlock;
    }

}
