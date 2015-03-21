using UnityEngine;
using System.Collections;

public class FallingVirus : Liquid
{

    public int life = 200;

    private System.Type[] _acceptedTypes = new System.Type[] { typeof(FallingVirus), typeof(FallingSteel) };

    public override Color BlockColor
    {
        get { return Color.green; }
    }

    public override FallingSandBlock NewBlock
    {
        get { return new FallingVirus(); }
    }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        life += Random.Range(0, 3);
    }

    private FallingVirus VirusProg
    {
        get
        {
            FallingVirus fv = new FallingVirus();
            fv.life = life - 1;
            return fv;

        }
    }

    protected override void Update(Chunk chunk)
    {
        life--;
        if (life <= 0)
        {
            SetThisBlock(chunk, null);
            return;
        }


        switch (Random.Range(0, 4))
        {
            case 0:
                if (TopBlock != null && !(IsOfType(TopBlock, _acceptedTypes)))
                    SetTopBlock(chunk, VirusProg);
                break;
            case 1:
                if (LeftBlock != null && !(IsOfType(LeftBlock, _acceptedTypes)))
                    SetLeftBlock(chunk, VirusProg);
                break;
            case 2:
                if (RightBlock != null && !(IsOfType(RightBlock, _acceptedTypes)))
                    SetRightBlock(chunk, VirusProg);
                break;
        }

        if (Random.Range(0, 10) == 0)
        {
            if (BottomBlock != null && !(IsOfType(BottomBlock, _acceptedTypes)))
                SetBottomBlock(chunk, VirusProg);
        }

        base.Update(chunk);

    }

}
