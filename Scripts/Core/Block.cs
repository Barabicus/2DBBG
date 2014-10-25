using UnityEngine;
using System.Collections;

public abstract class Block
{
    private byte _blockData;

    public byte BlockData
    {
        get { return _blockData; }
        set { _blockData = value; }
    }

    public abstract Color BlockColor { get; }

    public virtual void OnCreate(Chunk chunk) { }
    public virtual void OnDestroy(Chunk chunk) { }

}
