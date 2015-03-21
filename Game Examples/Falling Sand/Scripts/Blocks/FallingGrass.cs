using UnityEngine;
using System.Collections;

public class FallingGrass : Solid {

    System.Type[] acceptedTypes = new System.Type[] { typeof(FallingWater) };

    public override Color BlockColor
    {
        get { return new Color(0, 0.392157f, 0); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingGrass(); }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);

        if (IsOfType(BottomBlock, acceptedTypes))
        {
            if (LeftBlock == null)
                SetLeftBlock(chunk, NewBlock);
            if (RightBlock == null)
                SetRightBlock(chunk, NewBlock);
        }
        else if(Random.Range(0, 500) == 0)
        {
            SetThisBlock(chunk, null);
        }
                
    }
}
