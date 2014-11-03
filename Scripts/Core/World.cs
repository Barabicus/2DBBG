using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(LoadingStatus))]
public class World : MonoBehaviour
{

    private Dictionary<Vector2, Chunk> _chunks;

    public Chunk chunkPrefab;
    public int chunksX, chunksY;
    /// <summary>
    /// Set to true if the chunks should load before anything else happens. If set to false the chunks will load during gameplay.
    /// </summary>
    public bool preloadChunks = false;
    /// <summary>
    /// The amount of chunks to render at each load tick
    /// </summary>
    public int renderBatch = 8;
    /// <summary>
    /// Populate the world on start
    /// </summary>
    public bool loadChunksOnStart = true;

    private float _chunkSize;
    public LoadingStatus worldLoadingStatus;

    public List<Chunk> Chunks
    {
        get { return _chunks.Values.ToList(); }
    }


    #region Initilization

    void Awake()
    {
        _chunkSize = (chunkPrefab.blockSize * chunkPrefab.chunkSize) / chunkPrefab.pixelUnitSize;
        worldLoadingStatus = worldLoadingStatus == null ? gameObject.AddComponent<LoadingStatus>() : worldLoadingStatus;

        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab not specified!");
            Destroy(this);
            return;
        }

        _chunks = new Dictionary<Vector2, Chunk>();

        if (loadChunksOnStart)
        {
            if (!preloadChunks)
                StartCoroutine(SetupChunks());
            else
            {
                // Setup Chunks
                IEnumerator e = SetupChunks();
                while (e.MoveNext() != false) ;
                IEnumerator r = RenderChunks();
                while (r.MoveNext() != false) ;
            }
        }

    }

    private IEnumerator SetupChunks()
    {
        float blockSize = chunkPrefab.blockSize;
        float chunkSize = chunkPrefab.chunkSize;

        int i = 0;
        worldLoadingStatus.isLoading = true;
        worldLoadingStatus.loadingText = "Loading Chunk";
        worldLoadingStatus.finishAmount = chunksY * chunksX;
        worldLoadingStatus.currentAmount = 0;
        for (int y = 0; y < chunksY; y++)
        {
            for (int x = 0; x < chunksX; x++)
            {
                worldLoadingStatus.Increment();
                AddChunkAtindex(new Vector2(x, y));
                i++;
                if (i % 250 == 0)
                    yield return null;
            }
        }

            StartCoroutine(RenderChunks());
    }

    private IEnumerator RenderChunks()
    {
        int yield = 0;
        int i = 0;
        worldLoadingStatus.loadingText = "Rendering";
        worldLoadingStatus.currentAmount = 0;
        worldLoadingStatus.finishAmount = _chunks.Values.Count;
        foreach (Chunk chunk in _chunks.Values)
        {
            worldLoadingStatus.Increment();
            chunk.RenderChunk();
            yield++;
            if (yield == renderBatch)
            {
                yield = 0;
                yield return null;
            }
            i++;
        }

        worldLoadingStatus.isLoading = false;

    }

    private Chunk CreateChunk(int x, int y)
    {
        Chunk chunk = Instantiate(chunkPrefab, transform.position + new Vector3(x * chunkPrefab.ChunkIndexSize, y * chunkPrefab.ChunkIndexSize, 0), Quaternion.identity) as Chunk;
        chunk.InitialSetup();
        chunk.Initialize();
        chunk.World = this;
        _chunks.Add(chunk.ChunkIndex, chunk);
        return chunk;
    }

    private Chunk CreateChunk(Vector2 index)
    {
        return CreateChunk((int)index.x, (int)index.y);
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

    public Vector2 WorldPositionToChunkIndex(Vector3 position)
    {
        return WorldPositionToChunkIndex(position.ToVector2());
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
