using UnityEngine;
using System.Collections;

public class MiningExtractionBlock : FrequentBlock
{

    public int miningPower = 0;

    public override Color BlockColor
    {
        get { return Color.cyan; }
    }

    public MiningExtractionBlock(int miningPower)
    {
        this.miningPower = Mathf.Max(miningPower, 0);
    }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        if (miningPower == 0)
        {
            SetThisBlock(chunk, null);
            return;
        }
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);

        // Add a litle randomness
        if (Random.Range(0, 2) != 0)
            return;

        if (TopBlock is MiningAsteroidBlock)
            SetTopBlock(chunk, new MiningExtractionBlock(miningPower - 1));
        if (BottomBlock is MiningAsteroidBlock)
            SetBottomBlock(chunk, new MiningExtractionBlock(miningPower - 1));
        if (RightBlock is MiningAsteroidBlock)
            SetRightBlock(chunk, new MiningExtractionBlock(miningPower - 1));
        if (LeftBlock is MiningAsteroidBlock)
            SetLeftBlock(chunk, new MiningExtractionBlock(miningPower - 1));
        SetThisBlock(chunk, null);
    }

}
