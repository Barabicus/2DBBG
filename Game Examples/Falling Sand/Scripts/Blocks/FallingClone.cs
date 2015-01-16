using UnityEngine;
using System.Collections;
using System;

public class FallingClone : FallingSandCoreBlock {

    public Type cloneBlock;

    public override Color BlockColor
    {
        get { return Color.grey; }
    }

    public override void Update(Chunk chunk)
    {
        base.Update(chunk);
        if (TopBlock != null && !(TopBlock is FallingClone))
            cloneBlock = TopBlock.GetType();

        if (cloneBlock != null && BottomBlock == null)
            SetBottomBlock(chunk, (Block)Activator.CreateInstance(cloneBlock));
        else if (BottomBlock is FallingClone)
            ((FallingClone)BottomBlock).cloneBlock = cloneBlock;
    }

}
