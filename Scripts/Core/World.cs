using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine.UI;
using System.Linq;

public class World : MonoBehaviour
{

    private Dictionary<Vector2, Chunk> _chunks;

    public Chunk chunkPrefab;
    public int chunksX, chunksY;
    public bool preRenderChunks = false;
    public int renderBatch = 8;
    public bool loadChunksOnStart = true;

    private float _chunkSize;

    public List<Chunk> Chunks
    {
        get { return _chunks.Values.ToList(); }
    }

    #region Initilization

    void Awake()
    {
        _chunkSize = (chunkPrefab.blockSize * chunkPrefab.chunkSize) / chunkPrefab.pixelUnitSize;

        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab not specified!");
            Destroy(this);
            return;
        }

        _chunks = new Dictionary<Vector2, Chunk>();

        if (loadChunksOnStart)
            SetupChunks();

    }

    private void SetupChunks()
    {
        float blockSize = chunkPrefab.blockSize;
        float chunkSize = chunkPrefab.chunkSize;

        for (int y = 0; y < chunksY; y++)
        {
            for (int x = 0; x < chunksX; x++)
            {
                CreateChunk(x, y);
            }
        }

        StartCoroutine(CreateChunkColliders());
    }

    private Chunk CreateChunk(int x, int y)
    {
        Chunk chunk = Instantiate(chunkPrefab, transform.position + new Vector3(x * chunkPrefab.ChunkIndexSize, y * chunkPrefab.ChunkIndexSize, 0), Quaternion.identity) as Chunk;
        chunk.Init();
        chunk.World = this;
        chunk.GenerateChunk();
        _chunks.Add(chunk.ChunkIndex, chunk);
        return chunk;
    }

    private Chunk CreateChunk(Vector2 index)
    {
        return CreateChunk((int)index.x, (int)index.y);
    }

    private IEnumerator CreateChunkColliders()
    {
        int i = 0;
        // Setup chunk colliders
        foreach (Chunk chunk in _chunks.Values)
        {
            UIController.Instance.SetText("Creating Blocks: " + i + " / " + _chunks.Values.Count, 1f);
            chunk.SetupBoxColliders();
            if (i % 50 == 0)
                yield return null;
            i++;
        }


        if (preRenderChunks)
        {
            foreach (Chunk chunk in _chunks.Values)
            {
                chunk.RenderChunk();
            }
        }
        else
            StartCoroutine(RenderChunks());

    }

    private IEnumerator RenderChunks()
    {
        int yield = 0;
        int i = 0;
        foreach (Chunk chunk in _chunks.Values)
        {
            UIController.Instance.SetText("Rendering: " + i + " / " + _chunks.Values.Count, 1f);
            chunk.RenderChunk();
            yield++;
            if (yield == renderBatch)
            {
                yield = 0;
                yield return null;
            }
            i++;
        }

    }

    #endregion

    #region Chunk & Blocks

    public bool ContainsChunk(Vector2 index)
    {
        return _chunks.ContainsKey(index);
    }

    public void RemoveChunkAtIndex(Vector2 index)
    {
        Chunk ch;
        _chunks.TryGetValue(index, out ch);
        if (ch != null)
        {
            Destroy(ch.gameObject);
        }
        _chunks.Remove(index);
    }

    public Chunk AddChunkAtindex(Vector2 index)
    {
        if (!_chunks.ContainsKey(index))
        {
            Chunk chunk = CreateChunk(index);
            chunk.SetupBoxColliders();
            chunk.RenderChunk();
            return chunk;
        }
        return null;
    }

    /// <summary>
    /// Returns the block at the world position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Block GetBlockFromWorldPosition(Vector2 position)
    {
        Chunk chunk = GetChunkFromWorldPosition(position);
        if (chunk != null)
            return chunk.GetBlockFromWorldPosition(position);
        else
            return null;
    }

    /// <summary>
    /// Sets the block at the position in the world
    /// </summary>
    /// <param name="position"></param>
    /// <param name="block"></param>
    public void SetBlockWorldPosition(Vector2 position, Block block)
    {
        Chunk chunk = GetChunkFromWorldPosition(position);
        if (chunk != null)
            chunk.SetBlockAtWorldPosition(position, block);
    }


    // TOOD LOOKING INTO THIS
    public void SetBlockWorldPosition(Vector2 position, Vector2 blockIndex, Block block)
    {
        Vector2 setPos = position + blockIndex * (chunkPrefab.blockSize / 100f);
        Chunk chunk = GetChunkFromWorldPosition(setPos);
        if (chunk != null)
            chunk.SetBlockAtWorldPosition(setPos, block);
    }

    /// <summary>
    /// Returns the chunk at the world position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Chunk GetChunkFromWorldPosition(Vector2 position)
    {
        Vector2 index = WorldPositionToChunkIndex(position);
        Chunk chunk;
        _chunks.TryGetValue(index, out chunk);
        return chunk;
    }

    /// <summary>
    /// Gets a chunk from an index value
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Chunk GetChunkFromIndex(Vector2 index)
    {
        Chunk chunk;
        _chunks.TryGetValue(index, out chunk);
        return chunk;
    }

    /// <summary>
    /// Gets a chunk from an index value
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Chunk GetChunkFromIndex(float x, float y)
    {
        return GetChunkFromIndex(new Vector2(x, y));
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Converts a world position into a chunk index
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 WorldPositionToChunkIndex(Vector2 position)
    {
        Vector2 mod = new Vector2(position.x % _chunkSize, position.y % _chunkSize);
        position -= mod;
        return position;
    }

    #endregion

    #region Unity Methods

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new VineBlock());
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new VineBlock());
        }
        if (Input.GetMouseButton(2))
        {
            SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new StandardBlock());
        }
    }

    #endregion
}
