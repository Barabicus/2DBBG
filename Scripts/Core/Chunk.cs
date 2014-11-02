using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;

public class Chunk : MonoBehaviour
{

    #region Fields
    public int blockSize = 10;
    public int chunkSize = 16;
    public float threshhold = 0.5f;
    public float zoom = 1f;
    public float size = 64;
    public float tickTime = 2f;
    public ChunkGenerator generator;

    /// <summary>
    /// How many blocks fit into one unity unit.
    /// i.e. pixelsize 100 and block size 10 means it takes 10 blocks to move from point 0 to point 1
    /// </summary>
    private float _blockToUnitRatio;
    /// <summary>
    /// What space a single block occupies in unity space
    /// i.e. pixelsize 100 and block size 10 means each block is 0.1 points in size
    /// This is like how much world space a block occupies
    /// </summary>
    private float _unitToBlockRatio;
    /// <summary>
    /// The index increment size of each chunk.
    /// i.e. if the block size is 10, the chunk size is 10 and the pixel unit size is 100 each chunk will occupy 1 unit in space
    /// </summary>
    private float _chunkIndexSize;
    private bool _isDirty = true;
    private bool _isSpriteSetup = false;
    private float _lastTickTime;
    private World _world;
    private Vector2 _chunkIndex;

    SpriteRenderer chunkSpriteRend;

    Block[,] blocks;
    //BoxCollider2D[,] boxColliders;
    public BoxCollider2D[] boxColliders;

    Texture2D texture;

    List<IUpdateable> updateables;
    List<IUpdateable> updateableBuffer;

    public float pixelUnitSize = 100f;

    #endregion

    #region Properties

    public bool IsDirty
    {
        get { return _isDirty; }
        set { _isDirty = value; }
    }

    public World World
    {
        get { return _world; }
        set { _world = value; }
    }

    public Vector2 ChunkIndex
    {
        get { return _chunkIndex; }
    }

    public float ChunkIndexSize
    {
        get { return (blockSize * chunkSize) / pixelUnitSize; }
    }

    /// <summary>
    /// How many blocks fit into one unity unit.
    /// i.e. pixelsize 100 and block size 10 means it takes 10 blocks to move from point 0 to point 1
    /// </summary>
    public float BlockToUnitRatio
    {
        get { return pixelUnitSize / blockSize; }
    }

    /// <summary>
    /// What space a single block occupies in unity space
    /// i.e. pixelsize 100 and block size 10 means each block is 0.1 points in size
    /// This is like how much world space a block occupies
    /// </summary>
    public float UnitToBlockRatio
    {
        get { return blockSize / pixelUnitSize; }
    }

    #endregion

    #region Initialization
    public void InitialSetup()
    {
        // How many blocks in a chunk is based on the chunksize
        blocks = new Block[chunkSize, chunkSize];
        updateables = new List<IUpdateable>();
        updateableBuffer = new List<IUpdateable>();

        chunkSpriteRend = gameObject.GetComponent<SpriteRenderer>();

        _blockToUnitRatio = pixelUnitSize / blockSize;
        _unitToBlockRatio = blockSize / pixelUnitSize;
        _chunkIndexSize = (blockSize * chunkSize) / pixelUnitSize;

        // Set the size of the texture based on the block size and the chunk size
        texture = new Texture2D(blockSize * chunkSize, blockSize * chunkSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
    }

    public void Initialize()
    {
        _chunkIndex = transform.position.ToVector2();
        _lastTickTime = Time.deltaTime;
        name = "Chunk" + ChunkIndex;
        IsDirty = true;
        GenerateChunk();
    }

    /// <summary>
    /// Generate chunk from chunk generator
    /// </summary>
    public void GenerateChunk()
    {
        Vector2 position = transform.position.ToVector2();
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                blocks[x, y] = generator.GenerateBlock(((position.x * _blockToUnitRatio) + x) / (size * zoom), ((position.y * _blockToUnitRatio) + y) / (size * zoom));
            }
        }
    }

    /// <summary>
    /// Generate chunk colliders
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [ContextMenu("Setup Box Colliders")]
    public void SetupBoxColliders()
    {
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        for (int i = colliders.Length - 1; i >= 0; i--)
        {
            DestroyImmediate(colliders[i], true);
        }

        boxColliders = new BoxCollider2D[chunkSize * chunkSize];

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                CreateCollider(x, y);
            }
        }
    }

    void IndexToCoord(float index, out int x, out int y)
    {
        x = Mathf.FloorToInt(index / chunkSize);
        y = (int)(index % chunkSize);
    }

    int CoordToIndex(int x, int y)
    {
        return (chunkSize * x) + y;
    }

    /// <summary>
    /// Render entire chunk
    /// </summary>
    public void RenderChunk()
    {
        // Create the sprite
        chunkSpriteRend.sprite = Sprite.Create(texture, new Rect(0, 0, blockSize * chunkSize, blockSize * chunkSize), Vector2.zero);

        DrawAllBlocks();

        texture.Apply();

        _isSpriteSetup = true;
    }

    #endregion

    #region Collision

    private void CreateCollider(int x, int y)
    {
        int index = CoordToIndex(x, y);
        if (boxColliders[index] == null)
        {
            boxColliders[index] = gameObject.AddComponent<BoxCollider2D>();
            boxColliders[index].center = new Vector2(((x * blockSize) / pixelUnitSize) + (blockSize / pixelUnitSize) / 2, ((y * blockSize) / pixelUnitSize) + (blockSize / pixelUnitSize) / 2);
            boxColliders[index].size = new Vector2(blockSize / pixelUnitSize, blockSize / pixelUnitSize);
            boxColliders[index].enabled = false;
        }
    }

    /// <summary>
    /// Check if the block is contained within other blocks.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsBlockContained(int x, int y)
    {
        if (GetBlockAtIndex(x + 1, y) == null || GetBlockAtIndex(x, y + 1) == null || GetBlockAtIndex(x - 1, y) == null || GetBlockAtIndex(x, y - 1) == null)
            return false;
        else
            return true;
    }

    private void UpdateColliders()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if (GetBlockAtIndex(x, y) == null || IsBlockContained(x,y))
                    SetBlockCollision(x, y, false);
                else
                    SetBlockCollision(x, y, true);
            }
        }
    }

    #endregion

    #region Drawing

    /// <summary>
    /// Draw the block at the specified index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    /// <param name="texture"></param>
    private void DrawBlock(int x, int y, Block block, Texture2D texture)
    {
        for (int xx = 0; xx < blockSize; xx++)
        {
            for (int yy = 0; yy < blockSize; yy++)
            {
                texture.SetPixel((x * blockSize) + xx, (y * blockSize) + yy, block != null ? block.BlockColor : Color.clear);
            }
        }
    }

    /// <summary>
    /// Draw the block at the specified index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="color"></param>
    /// <param name="texture"></param>
    private void DrawBlock(Vector2 index, Block block, Texture2D texture)
    {
        DrawBlock((int)index.x, (int)index.y, block, texture);
    }

    public void DrawAllBlocks()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                DrawBlock(x, y, blocks[x, y], texture);
            }
        }
    }

    #endregion

    #region Block And Indexing


    /// <summary>
    /// Get index from a block reference. Returns true if the block can be found and sets index to the index value. 
    /// Returns false if not found.s
    /// </summary>
    /// <param name="block"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetIndexFromBlock(Block block, out Vector2 index)
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (blocks[x, y] == block)
                {
                    index = new Vector2(x, y);
                    return true;
                }
            }
        }
        index = Vector2.zero;
        return false;
    }

    /// <summary>
    /// Get block from block index in relative position to this chunk. If the index is out of range of this chunk
    /// it will attempt to translate to the proper chunk and return the relative block.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Block GetBlockAtIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
        {

            int xIndex = 0;
            int yIndex = 0;

            if (x > 0)
            {
                xIndex = Mathf.FloorToInt(x / chunkSize);
                x = x % chunkSize;
            }
            if (y > 0)
            {
                yIndex = Mathf.FloorToInt(y / chunkSize);
                y = y % chunkSize;
            }
            if (x < 0)
            {
                xIndex = Mathf.CeilToInt(x / chunkSize) - 1;
                x = (x % chunkSize) + chunkSize;
            }
            if (y < 0)
            {
                yIndex = Mathf.CeilToInt(y / chunkSize) - 1;
                y = (y % chunkSize) + chunkSize;
            }


            Chunk otherChunk = World.GetChunkFromIndex(ChunkIndex + new Vector2(xIndex, yIndex));
            if (otherChunk != null)
                return otherChunk.GetBlockAtIndex(x, y);
            else return null;
        }

        else
            return blocks[x, y];
    }

    public Block GetBlockAtLocalIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
            return null;
        else
            return blocks[x, y];
    }

    /// <summary>
    /// Get block from block index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Block GetBlockAtIndex(Vector2 index)
    {
        return GetBlockAtIndex((int)index.x, (int)index.y);
    }

    /// <summary>
    /// Get block from world position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Block GetBlockFromWorldPosition(Vector2 position)
    {
        Vector2 index = WorldPositionToBlockIndex(position);
        if (index.x < 0 || index.y < 0 || index.x > blocks.GetLength(0) || index.y > blocks.GetLength(1))
        {
            return null;
        }
        else
            return blocks[(int)index.x, (int)index.y];
    }

    /// <summary>
    /// Converts a world position to a block index
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 WorldPositionToBlockIndex(Vector2 position)
    {
        position -= transform.position.ToVector2();
        Vector2 points = position;
        Vector2 mod = new Vector2(points.x % (blockSize / pixelUnitSize), points.y % (blockSize / pixelUnitSize));
        Vector2 index = points - mod;
        index *= _blockToUnitRatio;
        return index;
    }

    /// <summary>
    /// Converts a block index into a world position
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector2 IndexToWorldPosition(Vector2 index)
    {
        return ChunkIndex + (index * _unitToBlockRatio);
    }

    /// <summary>
    /// Sets the block at index. Set block to null to destroy it
    /// </summary>
    /// <param name="index"></param>
    public void SetBlockAtIndex(int x, int y, Block block)
    {
        if (x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
        {
            int xIndex = 0;
            int yIndex = 0;

            if (x > 0)
            {
                xIndex = Mathf.FloorToInt(x / chunkSize);
                x = x % chunkSize;
            }
            if (y > 0)
            {
                yIndex = Mathf.FloorToInt(y / chunkSize);
                y = y % chunkSize;
            }
            if (x < 0)
            {
                xIndex = Mathf.CeilToInt(x / chunkSize) - 1;
                x = (x % chunkSize) + chunkSize;
            }
            if (y < 0)
            {
                yIndex = Mathf.CeilToInt(y / chunkSize) - 1;
                y = (y % chunkSize) + chunkSize;
            }

            Chunk otherChunk = World.GetChunkFromIndex(ChunkIndex + new Vector2(xIndex, yIndex));
            if (otherChunk != null)
                otherChunk.SetBlockAtIndex(x % chunkSize, y % chunkSize, block);
            else return;
        }
        else
        {
            if (blocks[x, y] != null)
            {
                // If block implemented IUpdateable remove it from the list
                if (blocks[x, y] is IUpdateable)
                    updateables.Remove(blocks[x, y] as IUpdateable);
                blocks[x, y].OnDestroy(this);
            }

            blocks[x, y] = block;
            if (block != null)
            {
                if (block is IUpdateable)
                    updateableBuffer.Add(block as IUpdateable);
            }


            if (block != null)
            {
                block.OnCreate(this);
            }

            DrawBlock(x, y, block, texture);
            IsDirty = true;

            // Set neighbour chunks to dirty
            if (x == 0)
            {
                Chunk chunk = World.GetChunkFromIndex(ChunkIndex + new Vector2(-_chunkIndexSize, 0));
                if (chunk != null)
                    chunk.IsDirty = true;
            }
            if (x == blocks.GetLength(0) - 1)
            {
                Chunk chunk = World.GetChunkFromIndex(ChunkIndex + new Vector2(_chunkIndexSize, 0));
                if (chunk != null)
                    chunk.IsDirty = true;
            }
            if (y == 0)
            {
                Chunk chunk = World.GetChunkFromIndex(ChunkIndex + new Vector2(0, -_chunkIndexSize));
                if (chunk != null)
                    chunk.IsDirty = true;
            }
            if (y == blocks.GetLength(1) - 1)
            {
                Chunk chunk = World.GetChunkFromIndex(ChunkIndex + new Vector2(0, _chunkIndexSize));
                if (chunk != null)
                    chunk.IsDirty = true;
            }

        }
    }


    public void SetBlockCollision(int x, int y, bool enabled)
    {
        if (x < 0 || y < 0 || x >= chunkSize || y >= chunkSize)
            return;
        else
            boxColliders[CoordToIndex(x,y)].enabled = enabled;
    }

    /// <summary>
    /// Set at the contact point. This should not be called internally.
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="block"></param>
    public void SetBlockAtContact(ContactPoint2D contact, Vector2 normal, Block block)
    {
        SetBlockAtIndex(WorldPositionToBlockIndex(contact.point + normal / (_blockToUnitRatio * 2)), block);
    }

    public void SetBlockAtContact(ContactPoint2D contact, Block block)
    {
        SetBlockAtContact(contact, -contact.normal, block);
    }

    public Vector2 GetIndexFromContact(ContactPoint2D contact)
    {
        return WorldPositionToBlockIndex(contact.point + -contact.normal / (_blockToUnitRatio * 2));
    }

    public void SetBlockAtIndex(Vector2 index, Block block)
    {
        SetBlockAtIndex((int)index.x, (int)index.y, block);
    }

    public void SetBlockAtWorldPosition(Vector2 position, Block block)
    {
        SetBlockAtIndex(WorldPositionToBlockIndex(position), block);
    }

    #endregion

    #region Unity Methods

    protected virtual void Update()
    {
        if (Time.time - _lastTickTime >= tickTime)
            Tick();

        // Add all updateables from buffer
        for (int i = updateableBuffer.Count - 1; i >= 0; i--)
        {
            updateables.Add(updateableBuffer[i]);
            updateableBuffer.Remove(updateableBuffer[i]);
        }

        if (IsDirty && _isSpriteSetup)
        {
            texture.Apply();
            UpdateColliders();
            IsDirty = false;
        }
    }

    #endregion

    #region Chunk

    private void Tick()
    {
        // Update all updateable blocks
        for (int i = updateables.Count - 1; i >= 0; i--)
        {
            updateables[i].Update(this);
        }

        _lastTickTime = Time.time;
    }

    #endregion
}
