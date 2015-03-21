using UnityEngine;
using System.Collections;

public class FallingVirusSolid : Liquid
{


    System.Type[] acceptedTypes = new System.Type[] { typeof(FallingVirusSolid), typeof(FallingSteel) };

    public FallingSandBlock prevBlock;
    public int life = 200;

    public override Color BlockColor
    {
        get { return new Color(1f, 0.0784314f, 0.576471f); }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingVirusSolid(); }
    }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
    }


    protected override void Update(Chunk chunk)
    {

        if (life <= 0)
        {
            if (prevBlock != null)
                SetThisBlock(chunk, prevBlock.NewBlock);
            else
                SetThisBlock(chunk, null);

            return;
        }
        else if (Random.Range(0, 5) == 0)
            life -= 1;

        switch (Random.Range(0, 5))
        {
            case 0:
                if (TopBlock != null && !(IsOfType(TopBlock, acceptedTypes)))
                    SetTopBlock(chunk, new FallingVirusSolid() { prevBlock = TopBlock as FallingSandBlock, life = this.life - 1 });
                break;
            case 1:
                if (LeftBlock != null && !(IsOfType(LeftBlock, acceptedTypes)))
                    SetLeftBlock(chunk, new FallingVirusSolid() { prevBlock = LeftBlock as FallingSandBlock, life = this.life - 1 });
                break;
            case 2:
                if (RightBlock != null && !(IsOfType(RightBlock, acceptedTypes)))
                    SetRightBlock(chunk, new FallingVirusSolid() { prevBlock = RightBlock as FallingSandBlock, life = this.life - 1 });
                break;
            case 3:
                if (BottomBlock != null && !(IsOfType(BottomBlock, acceptedTypes)))
                    SetBottomBlock(chunk, new FallingVirusSolid() { prevBlock = BottomBlock as FallingSandBlock, life = this.life - 1 });
                break;
        }

        base.Update(chunk);

    }
}
