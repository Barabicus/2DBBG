using UnityEngine;
using System.Collections;

public abstract class Block
{
    protected int indexX, indexY;

    public void SetIndex(int x, int y)
    {
        indexX = x;
        indexY = y;
    }

    public abstract Color BlockColor { get; }
    public virtual void OnCreate(Chunk chunk) { }
    public virtual void OnDestroy(Chunk chunk) { }
    public virtual void OnNeighbourChange(BlockNeighbour blockChanged) { }

    public virtual void Update(Chunk chunk)
    {

    }

    #region HelperMethods

    public void SetTopBlock(Chunk chunk, Block block)
    {
        chunk.SetBlockAtIndex(indexX, indexY + 1, block);
    }
    public void SetRightBlock(Chunk chunk, Block block)
    {
        chunk.SetBlockAtIndex(indexX + 1, indexY, block);
    }
    public void SetBottomBlock(Chunk chunk, Block block)
    {
        chunk.SetBlockAtIndex(indexX, indexY - 1, block);
    }
    public void SetLeftBlock(Chunk chunk, Block block)
    {
        chunk.SetBlockAtIndex(indexX - 1, indexY, block);
    }
    public void SetThisBlock(Chunk chunk, Block block)
    {
        chunk.SetBlockAtIndex(indexX, indexY, block);
    }

    #endregion

}

public enum BlockNeighbour
{
    Top,
    Right,
    Bottom,
    Left
}
