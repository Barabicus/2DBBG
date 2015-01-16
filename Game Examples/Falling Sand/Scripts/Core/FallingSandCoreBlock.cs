using UnityEngine;
using System.Collections;

/// <summary>
/// A block class created specificially to work well with falling sand. 
/// This is optimized for the game type. It has a high memory overhead in comparision
/// to a standard block but the reduced method calls allow the game to work faster
/// </summary>
public abstract class FallingSandCoreBlock : Block
{

    public Block TopBlock { get; set; }
    public Block RightBlock { get; set; }
    public Block BottomBlock { get; set; }
    public Block LeftBlock { get; set; }

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        UpdateNeighbourBlocks(chunk);
    }

    public override void Update(Chunk chunk)
    {
        base.Update(chunk);
        UpdateNeighbourBlocks(chunk);
    }

    private void UpdateNeighbourBlocks(Chunk chunk)
    {
        TopBlock = chunk.GetBlockAtIndex(indexX, indexY + 1);
        RightBlock = chunk.GetBlockAtIndex(indexX + 1, indexY);
        BottomBlock = chunk.GetBlockAtIndex(indexX, indexY - 1);
        LeftBlock = chunk.GetBlockAtIndex(indexX - 1, indexY);
    }

}
