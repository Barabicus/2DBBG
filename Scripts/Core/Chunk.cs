using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{

    #region Fields
    public int blockSize = 10;
    public int chunkSize = 16;
    public float threshhold = 0.5f;
    public float zoom = 1f;
    public float size = 64;
    public float tickTime = 2f;

    /// <summary>
    /// How many blocks fit into one unit
    /// </summary>
    private float _blockToUnitRatio;
    /// <summary>
    /// What percent a block is to a unit
    /// </summary>
    private float _unitToBlockRatio;
    private bool _isDirty = false;
    private bool _isSpriteSetup = false;
    private float _lastTickTime;
    private World _world;
    private Vector2IndexWrapper _chunkIndex;

    SpriteRenderer chunkSpriteRend;

    Block[,] blocks;
    BoxCollider2D[,] boxColliders;
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

    public Vector2IndexWrapper ChunkPosition
    {
        get { return _chunkIndex; }
    }

    #endregion

    #region Initialization

    public void Init()
    {
        if (1 % ((chunkSize * blockSize) / pixelUnitSize) != 0)
        {
            Debug.Log(1 % (chunkSize * blockSize) + " : " + (chunkSize * blockSize));
            Debug.LogError("Chunk Index must increase in multiples of 1");
            return;
        }

        _chunkIndex = new Vector2IndexWrapper(transform.position.ToVector2());
        // How many blocks in a chunk is based on the chunksize
        blocks = new Block[chunkSize, chunkSize];
        boxColliders = new BoxCollider2D[chunkSize, chunkSize];
        updateables = new List<IUpdateable>();
        updateableBuffer = new List<IUpdateable>();
        _lastTickTime = Time.deltaTime;

        chunkSpriteRend = gameObject.GetComponent<SpriteRenderer>();

        _blockToUnitRatio = pixelUnitSize / blockSize;
        _unitToBlockRatio = blockSize / pixelUnitSize;


        // Set the size of the texture based on the block size and the chunk size
        texture = new Texture2D(blockSize * chunkSize, blockSize * chunkSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;

    }

    /// <summary>
    /// Generate chunk map
    /// </summary>
    public void GenerateChunk()
    {
        Vector2 position = transform.position.ToVector2();
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (Mathf.PerlinNoise(((position.x * _blockToUnitRatio) + x) / (size * zoom), ((position.y * _blockToUnitRatio) + y) / (size * zoom)) > threshhold)
                {
                    blocks[x, y] = new StandardBlock();
                }

            }
        }
    }

    /// <summary>
    /// Generate initial chunk colliders
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetupBoxColliders()
    {
        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                if (GetBlockAtIndex(x, y) != null && !IsBlockContained(x, y))
                {
                    CreateCollider(x, y);
                }
            }
        }
    }

    /// <summary>
    /// Render entire chunk
    /// </summary>
    public void RenderChunk()
    {
        // Create the sprite
        chunkSpriteRend.sprite = Sprite.Create(texture, new Rect(0, 0, blockSize * chunkSize, blockSize * chunkSize), Vector2.zero);

        for (int x = 0; x < blocks.GetLength(0); x++)
        {
            for (int y = 0; y < blocks.GetLength(1); y++)
            {
                DrawBlock(x, y, blocks[x, y], texture);
            }
        }

        texture.Apply();

        _isSpriteSetup = true;

    }

    #endregion

    #region Collision

    private void CreateCollider(int x, int y)
    {
        if (boxColliders[x, y] == null)
        {
            boxColliders[x, y] = gameObject.AddComponent<BoxCollider2D>();
            boxColliders[x, y].center = new Vector2(((x * blockSize) / pixelUnitSize) + (blockSize / pixelUnitSize) / 2, ((y * blockSize) / pixelUnitSize) + (blockSize / pixelUnitSize) / 2);
            boxColliders[x, y].size = new Vector2(blockSize / pixelUnitSize, blockSize / pixelUnitSize);
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


        //Debug.Log("other");

        //Vector2 position = transform.position.ToVector2();
        //if (World.GetBlockFromPosition(position + new Vector2(x + 1, y)) == null || World.GetBlockFromPosition(position + new Vector2(x, y + 1)) == null || World.GetBlockFromPosition(position + new Vector2(x - 1, y)) == null || World.GetBlockFromPosition(position + new Vector2(x, y - 1)) == null)
        //    return false;
        //else
        //    return false;


    }

    /// <summary>
    /// Update neighbouring blocks at index x, y. If the block is not null, create a collider for it
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void UpdateNeighbourBlocks(int x, int y)
    {
        //Update Top Block
        if (GetBlockAtIndex(x, y + 1) != null)
        {
            CreateCollider(x, y + 1);
        }
        //Update Right Block
        if (GetBlockAtIndex(x + 1, y) != null)
        {
            CreateCollider(x + 1, y);
        }
        //Update Bottom Block
        if (GetBlockAtIndex(x, y - 1) != null)
        {
            CreateCollider(x, y - 1);
        }
        //Update Left Block
        if (GetBlockAtIndex(x - 1, y) != null)
        {
            CreateCollider(x - 1, y);
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
    /// Get block from block index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Block GetBlockAtIndex(int x, int y)
    {
        if (x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
        {
            return null;
        }

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
        return ChunkPosition + (index * _unitToBlockRatio);
    }

    /// <summary>
    /// Sets the block at index. Set block to null to destroy it
    /// </summary>
    /// <param name="index"></param>
    public void SetBlockAtIndex(int x, int y, Block block)
    {
        if (x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
        {
            return;
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
            if (block == null)
            {
                Destroy(boxColliders[x, y]);
                UpdateNeighbourBlocks(x, y);
            }
            else
            {
                if (block is IUpdateable)
                    updateableBuffer.Add(block as IUpdateable);
                CreateCollider(x, y);
            }

            if (block != null)
            {
                block.OnCreate(this);
            }

            DrawBlock(x, y, block, texture);
        }

        IsDirty = true;
    }

    // TODO
    public void SetBlockAtContact(ContactPoint2D contact)
    {

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
            IsDirty = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer != 9)
            return;
        foreach (ContactPoint2D contact in coll.contacts)
        {
            // Destroy block at contact point + the normal devided by the offset times 2 which
            // will place the position in the center of the contact point's block.
            SetBlockAtIndex(WorldPositionToBlockIndex(contact.point + contact.normal / (_blockToUnitRatio * 2)), null);
            //   SetBlock(WorldPositionToBlockIndex(contact.point), new StandardBlock());

        }

        Destroy(coll.gameObject);
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
