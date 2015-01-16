using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

[RequireComponent(typeof(SpriteRenderer))]
public class Chunk : MonoBehaviour
{

    #region Fields
    public int blockSize = 16;
    public int chunkSize = 16;
    public bool useThreading = true;
    public bool tick = true;
    public float tickTime = 2f;
    public ChunkGenerator generator;
    public UpdateMethod updateMethod = UpdateMethod.Update;
    public bool useColliders = true;

    /// <summary>
    /// This is how much space a chunk occupies in world space either horizontally or vertically
    /// </summary>
    private float _chunkToWorldSize;
    /// <summary>
    /// What space a single block occupies in unity space
    /// i.e. pixelsize 100 and block size 10 means each block is 0.1 points in size
    /// This is how much world space a block occupies
    /// </summary>
    private float _blockToWorldSize;
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
    private bool _isInitialized;
    private bool _isGenerated;

    SpriteRenderer chunkSpriteRend;

    [HideInInspector]
    public Block[,] blocks;
    [HideInInspector]
    public BoxCollider2D[] boxColliders;

    [HideInInspector]
    public Texture2D texture;
    [HideInInspector]
    public Color[] texturePixels;

    public float pixelUnitSize = 128f;

    public float PixelUnitSize
    {
        get { return pixelUnitSize; }
    }

    #endregion

    #region Events

    public event Action<Chunk> ChunkReInitialized;

    #endregion

    #region States

    public enum UpdateMethod
    {
        Update,
        FixedUpdate
    }

    #endregion

    #region Properties

    /// <summary>
    /// Is the chunk dirty, if it is redraw the chunk and update all chunk colliders
    /// </summary>
    public bool IsDirty
    {
        get { return _isDirty; }
        set { _isDirty = value; }
    }

    public bool IsGenerated
    {
        get { return _isGenerated; }
        set
        {
            _isGenerated = true;
            DrawAllBlocks();
        }
    }

    /// <summary>
    /// The world this chunk is associated with
    /// </summary>
    public World World
    {
        get { return _world; }
        set { _world = value; }
    }

    /// <summary>
    /// The index position of this chunk
    /// </summary>
    public Vector2 ChunkIndex
    {
        get { return _chunkIndex; }
        private set { _chunkIndex = value; }
    }

    /// <summary>
    /// The chunk index increments i.e. if the chunk in the world is at position (4, 0) and the chunk index size is 2
    /// then the key to access the neighbouring chunk to the right will 4 + 2. Using this we can get the neighbouring chunk
    /// via the world.
    /// </summary>
    public float ChunkIndexSize
    {
        get { return (blockSize * chunkSize) / PixelUnitSize; }
    }

    /// <summary>
    /// This is how much space a chunk occupies in world space either horizontally or vertically
    /// </summary>
    public float ChunkToWorldSize
    {
        get { return PixelUnitSize / blockSize; }
    }

    /// <summary>
    /// What space a single block occupies in unity space
    /// i.e. pixelsize 100 and block size 10 means each block is 0.1 points in size
    /// This is how much world space a block occupies
    /// </summary>
    public float BlockToWorldSize
    {
        get { return blockSize / PixelUnitSize; }
    }

    /// <summary>
    /// Has the chunk been initialized. Chunks can be ReInitialized as many times as desired.
    /// </summary>
    public bool IsInitialized
    {
        get { return _isInitialized; }
    }

    #endregion

    #region Initialization

    public void InitialSetup()
    {
        //  rigidbody2D.isKinematic = true;

        // How many blocks in a chunk is based on the chunksize
        blocks = new Block[chunkSize, chunkSize];

        chunkSpriteRend = gameObject.GetComponent<SpriteRenderer>();

        _chunkToWorldSize = PixelUnitSize / blockSize;
        _blockToWorldSize = blockSize / PixelUnitSize;
        _chunkIndexSize = (blockSize * chunkSize) / PixelUnitSize;

        // Set the size of the texture based on the block size and the chunk size
        texture = new Texture2D(blockSize * chunkSize, blockSize * chunkSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
        texturePixels = texture.GetPixels();

    }

    /// <summary>
    /// This initializes the chunk and sets it up for use with the world.
    /// </summary>
    public void Initialize()
    {
        if (ChunkReInitialized != null)
            ChunkReInitialized(this);

        if (World == null)
            Debug.LogWarning("Chunk World was null");

        ChunkIndex = transform.position.ToVector2() - World.transform.position.ToVector2();
        _lastTickTime = Time.deltaTime;
        name = "Chunk" + ChunkIndex;
        IsDirty = true;
        _isGenerated = false;
        GenerateChunk();
        _isInitialized = true;

        if (World != null)
        {
            //// Add chunk to world if world exists
            //if (!World.AddChunk(this))
            //{
            //    Debug.LogError("Chunk could not be added to the world, world already has chunk index  " + ChunkIndex);
            //    Destroy(gameObject);
            //}
            World.AddChunk(this);
        }

    }

    /// <summary>
    /// Generate chunk from chunk generator
    /// </summary>
    private void GenerateChunk()
    {
        Vector2 position = ChunkIndex;
        if (generator != null)
        {
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {
                    blocks[x, y] = generator.GenerateBlock(((position.x * _chunkToWorldSize) + x) * BlockToWorldSize, ((position.y * _chunkToWorldSize) + y) * BlockToWorldSize);
                }
            }
        }
        IsGenerated = true;
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

    /// <summary>
    /// Returns an index from a coordinate based on chunk * block size
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int CoordToBroadIndex(int x, int y)
    {
        return ((chunkSize * blockSize) * y) + x;
    }

    /// <summary>
    /// Returns an index from a coordinate based on chunksize
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>

    private int CoordToNarrowIndex(int x, int y)
    {
        return (chunkSize * y) + x;
    }

    /// <summary>
    /// Render entire chunk
    /// </summary>
    public void SetupGraphics()
    {
        // Create the sprite
        chunkSpriteRend.sprite = Sprite.Create(texture, new Rect(0, 0, blockSize * chunkSize, blockSize * chunkSize), Vector2.zero, PixelUnitSize);

        //   DrawAllBlocks();

        texture.Apply();

        _isSpriteSetup = true;
    }

    #endregion

    #region Collision

    /// <summary>
    /// Create collider at index x, y
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CreateCollider(int x, int y)
    {
        int index = CoordToNarrowIndex(x, y);
        if (boxColliders[index] == null)
        {
            boxColliders[index] = gameObject.AddComponent<BoxCollider2D>();
            // Hide box components in the inspector
            boxColliders[index].hideFlags = HideFlags.HideInInspector;
            boxColliders[index].center = new Vector2(((x * blockSize) / PixelUnitSize) + (blockSize / PixelUnitSize) / 2, ((y * blockSize) / PixelUnitSize) + (blockSize / PixelUnitSize) / 2);
            boxColliders[index].size = new Vector2(blockSize / PixelUnitSize, blockSize / PixelUnitSize);
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
        {
            return false;
        }
        else
            return true;
    }

    /// <summary>
    /// Updates all the colliders
    /// </summary>
    private void UpdateColliders()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if (GetBlockAtIndex(x, y) == null || IsBlockContained(x, y))
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
    private void DrawBlock(int x, int y, Block block)
    {
        if (!useThreading)
            for (int xx = 0; xx < blockSize; xx++)
            {
                for (int yy = 0; yy < blockSize; yy++)
                {
                    texturePixels[CoordToBroadIndex((x * blockSize) + xx, (y * blockSize) + yy)] = block != null ? block.BlockColor : Color.clear;
                }
            }
        else
        {
            ThreadPool.QueueUserWorkItem(DrawBlockThreaded, new ChunkEventArgs(x, y, block, texture));
        }
    }

    private void DrawBlockThreaded(object context)
    {
        ChunkEventArgs ch = (ChunkEventArgs)context;
        for (int xx = 0; xx < blockSize; xx++)
        {
            for (int yy = 0; yy < blockSize; yy++)
            {
                texturePixels[CoordToBroadIndex((ch.x * blockSize) + xx, (ch.y * blockSize) + yy)] = ch.block != null ? ch.block.BlockColor : Color.clear;
            }
        }
        IsDirty = true;
    }

    /// <summary>
    /// Draw the block at the specified index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="color"></param>
    /// <param name="texture"></param>
    private void DrawBlock(Vector2 index, Block block)
    {
        DrawBlock((int)index.x, (int)index.y, block);
        IsDirty = true;
    }

    /// <summary>
    /// Draw all the blocks in this chunk
    /// </summary>
    /// <param name="context"></param>
    private void DrawAllBlocks(object context)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                DrawBlock(x, y, blocks[x, y]);
            }
        }
        IsDirty = true;
    }

    /// <summary>
    /// Draw all the blocks in this chunk
    /// </summary>
    public void DrawAllBlocks()
    {
        if (useThreading)
            ThreadPool.QueueUserWorkItem(DrawAllBlocks);
        else
            DrawAllBlocks(null);
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
            Chunk otherChunk = TranslateChunk(ref x, ref y);
            if (otherChunk != null)
                return otherChunk.GetBlockAtIndex(x, y);
            else return null;
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
        Vector2 index = WorldPositionToIndex(position);
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
    public Vector2 WorldPositionToIndex(Vector2 position)
    {
        position -= transform.position.ToVector2();
        position *= PixelUnitSize;
        Vector2 points = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        // Vector2 points = position;
        Vector2 mod = new Vector2(points.x % blockSize, points.y % blockSize);
        Vector2 index = points - mod;
        index /= PixelUnitSize;
        index *= _chunkToWorldSize;
        return index;
    }

    /// <summary>
    /// Converts a block index into a world position
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector2 IndexToWorldPosition(Vector2 index)
    {
        return ChunkIndex + (index * _blockToWorldSize);
    }

    /// <summary>
    /// Sets the block at index. Set block to null to destroy it
    /// </summary>
    public void SetBlockAtIndex(int x, int y, Block block)
    {
        // If the index is outside the bounds of this chunk try and translate that index to the proper chunk
        // and change the block accordingly
        if (x < 0 || y < 0 || x >= blocks.GetLength(0) || y >= blocks.GetLength(1))
        {
            Chunk otherChunk = TranslateChunk(ref x, ref y);
            if (otherChunk != null)
                otherChunk.SetBlockAtIndex(x, y, block);
            else return;
        }
        else
        {
            if (blocks[x, y] != null)
            {
                blocks[x, y].OnDestroy(this);
            }

            // Don't redraw if the block is already the same type
            if (block == null || blocks[x, y] == null || (blocks[x, y].GetType() != block.GetType()))
                DrawBlock(x, y, block);
            IsDirty = true;

            blocks[x, y] = block;
            if (block != null)
                block.SetIndex(x, y);

            if (block != null)
            {
                block.OnCreate(this);
            }

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

    /// <summary>
    /// Sets the block at index. Set block to null to destroy it
    /// </summary>
    public void SetBlockAtIndex(Vector2 index, Block block)
    {
        SetBlockAtIndex((int)index.x, (int)index.y, block);
    }

    /// <summary>
    /// Turns a block collider on or off
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="enabled"></param>
    public void SetBlockCollision(int x, int y, bool enabled)
    {
        if (x < 0 || y < 0 || x >= chunkSize || y >= chunkSize)
            return;
        else
            boxColliders[CoordToNarrowIndex(x, y)].enabled = enabled;
    }

    /// <summary>
    /// Set block at the contact point.
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="block"></param>
    public void SetBlockAtContact(ContactPoint2D contact, Vector2 normal, Block block)
    {
        SetBlockAtIndex(GetIndexFromContact(contact), block);
    }

    /// <summary>
    /// Set block at the contact point
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="block"></param>
    public void SetBlockAtContact(ContactPoint2D contact, Block block)
    {
        SetBlockAtContact(contact, -contact.normal, block);
    }

    /// <summary>
    /// Get the block index from the contact point
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public Vector2 GetIndexFromContact(ContactPoint2D contact)
    {
        return WorldPositionToIndex(contact.point + -contact.normal / (_chunkToWorldSize * 2));
    }

    /// <summary>
    /// Returns the block the contact hit in world position
    /// </summary>
    /// <param name="contact"></param>
    /// <returns></returns>
    public Vector2 GetWorldPositionFromContact(ContactPoint2D contact)
    {
        return contact.point + (-contact.normal / (_chunkToWorldSize * 2)).RoundVector();
    }

    /// <summary>
    /// Sets a block at world position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="block"></param>
    public void SetBlockWorldPosition(Vector2 position, Block block)
    {
        SetBlockAtIndex(WorldPositionToIndex(position), block);
    }

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (updateMethod == UpdateMethod.Update)
            UpdateChunk();
    }

    private void FixedUpdate()
    {
        if (updateMethod == UpdateMethod.FixedUpdate)
            UpdateChunk();
    }

    private void UpdateChunk()
    {
        if (tick && Time.time - _lastTickTime >= tickTime)
            Tick();

        if (IsDirty && _isSpriteSetup && IsGenerated)
        {
            texture.SetPixels(texturePixels);
            texture.Apply();
            if (useColliders)
                UpdateColliders();
            IsDirty = false;
        }
    }


    #endregion

    #region Chunk

    /// <summary>
    /// Trigger a chunk tick
    /// </summary>
    public void Tick()
    {
        // Update all updateable blocks
        //for (int i = updateables.Count - 1; i >= 0; i--)
        //{
        //    updateables[i].Update(this);
        //}

        foreach (Block b in blocks)
        {
            if (b != null)
                b.Update(this);
        }

        _lastTickTime = Time.time;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Translate the index positions x and y to the proper chunk in relation to this chunk. i.e x = -5 and the chunk size is 10 it will
    /// translate to the chunk to the left of this chunk.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Chunk TranslateChunk(ref int x, ref int y)
    {
        int xIndex = 0;
        int yIndex = 0;
        if (x >= chunkSize)
        {
            xIndex = Mathf.FloorToInt((x / chunkSize) * _chunkIndexSize);
            x = x % chunkSize;
        }
        if (y >= chunkSize)
        {
            yIndex = Mathf.FloorToInt((y / chunkSize) * _chunkIndexSize);
            y = y % chunkSize;
        }
        if (x < 0)
        {
            xIndex = Mathf.CeilToInt((x / chunkSize) * _chunkIndexSize) - (int)_chunkIndexSize;
            x = (x % chunkSize) + chunkSize;
        }
        if (y < 0)
        {
            yIndex = Mathf.CeilToInt((y / chunkSize) * _chunkIndexSize) - (int)_chunkIndexSize;
            y = (y % chunkSize) + chunkSize;
        }

        return World.GetChunkFromIndex(ChunkIndex + new Vector2(xIndex, yIndex));
    }

    #endregion
}

public struct ChunkEventArgs
{
    public int x, y;
    public Block block;
    public Texture2D texture;

    public ChunkEventArgs(int x, int y, Block block, Texture2D texture)
    {
        this.x = x;
        this.y = y;
        this.block = block;
        this.texture = texture;
    }

}
