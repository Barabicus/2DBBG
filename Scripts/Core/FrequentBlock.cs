using UnityEngine;
using System.Collections;

/// <summary>
/// This is optimized for a frequently updating block. It has a high memory overhead in comparision
/// to a standard block but the reduced method calls allow it to work faster
/// </summary>
public abstract class FrequentBlock : Block
{

    public Block TopBlock { get; set; }
    public Block RightBlock { get; set; }
    public Block BottomBlock { get; set; }
    public Block LeftBlock { get; set; }

    public Vector2 TopBlockIndex { get { return new Vector2(indexX, indexY + 1); } }
    public Vector2 RightBlockIndex { get { return new Vector2(indexX + 1, indexY); } }
    public Vector2 BottomBlockIndex { get { return new Vector2(indexX, indexY - 1); } }
    public Vector2 LeftBlockIndex { get { return new Vector2(indexX -1, indexY); } }

    public bool CanUpdate
    {
        get { return _canUpdate; }
        set { _canUpdate = value; }
    }

    private bool _canUpdate = false;

    public override void OnCreate(Chunk chunk)
    {
        base.OnCreate(chunk);
        UpdateNeighbourBlocks(chunk);
    }

    public override void UpdateBlock(Chunk chunk)
    {
        if (!_canUpdate)
        {
            // Prevent blocks just created to update within the same tick
            _canUpdate = true;
            return;
        }
        Update(chunk);
    }

    protected override void Update(Chunk chunk)
    {
        base.Update(chunk);
         UpdateNeighbourBlocks(chunk);
    }

    public override void OnNeighbourChange(Chunk chunk, BlockNeighbour blockChanged)
    {
        switch (blockChanged)
        {
            case BlockNeighbour.Top:
                TopBlock = chunk.GetBlockAtIndex(indexX, indexY + 1);
                break;
            case BlockNeighbour.Right:
                TopBlock = chunk.GetBlockAtIndex(indexX + 1, indexY);
                break;
            case BlockNeighbour.Bottom:
                TopBlock = chunk.GetBlockAtIndex(indexX, indexY - 1);
                break;
            case BlockNeighbour.Left:
                TopBlock = chunk.GetBlockAtIndex(indexX - 1, indexY);
                break;
        }
    }

    public virtual void UpdateNeighbourBlocks(Chunk chunk)
    {
        TopBlock = chunk.GetBlockAtIndex(indexX, indexY + 1);
        RightBlock = chunk.GetBlockAtIndex(indexX + 1, indexY);
        BottomBlock = chunk.GetBlockAtIndex(indexX, indexY - 1);
        LeftBlock = chunk.GetBlockAtIndex(indexX - 1, indexY);

        //if (TopBlock != null)
        //    TopBlock.OnNeighbourChange(chunk, BlockNeighbour.Bottom);
        //if (RightBlock != null)
        //    RightBlock.OnNeighbourChange(chunk, BlockNeighbour.Left);
        //if (BottomBlock != null)
        //    BottomBlock.OnNeighbourChange(chunk, BlockNeighbour.Top);
        //if (LeftBlock != null)
        //    LeftBlock.OnNeighbourChange(chunk, BlockNeighbour.Right);

    }

}
