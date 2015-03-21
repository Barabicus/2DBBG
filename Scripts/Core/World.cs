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
    public Chunk[] preloadedChunks;

    public Chunk chunkPrefab;
    public int chunksX, chunksY;

    // [HideInInspector]
    //  public bool preLoaded = false;
    /// <summary>
    /// Set to true if the chunks should load before anything else happens. If set to false the chunks will load during gameplay.
    /// </summary>
    public bool preloadChunks = false;
    /// <summary>
    /// The amount of chunks to render at each load tick
    /// </summary>
    public int renderBatch = 8;
    /// <summary>
    /// Populate the world on awake
    /// </summary>
    public bool loadChunksOnAwake = true;

    private float _chunkSize;
    public LoadingStatus worldLoadingStatus;

    private Vector3 originalPosition;

    public List<Chunk> Chunks
    {
        get { return _chunks.Values.ToList(); }
    }


    #region Initilization

    void Awake()
    {
        //  gameObject.SetActive(false);
        originalPosition = transform.position;
        transform.position = Vector3.zero;
        if (chunkPrefab.generator != null)
            chunkPrefab.generator.PreLoadGenerator(chunkPrefab);
        InitVariables();

        if (loadChunksOnAwake)
        {
            if (!preloadChunks)
            {
                worldLoadingStatus.loadingFinished += () => { StartCoroutine(RenderChunks()); };
                StartCoroutine(SetupChunks());
            }
            else
            {
                ForceLoad();
            }
        }
        transform.position = originalPosition;
        gameObject.SetActive(true);
    }

    [ContextMenu("PreInstantiate Chunks")]
    public void PreInstantiateChunks()
    {
        preloadedChunks = new Chunk[chunksX * chunksY];

        for (int x = 0; x < chunksX; x++)
        {
            for (int y = 0; y < chunksY; y++)
            {
                Chunk ch = InstantiateChunk(x, y);
                ch.transform.parent = transform;
                InitChunk(ch);
                preloadedChunks[x + (y * chunksY)] = ch;
            }
        }

    }

    [ContextMenu("Unload Chunks")]
    public void UnloadChunks()
    {
        if (preloadedChunks == null)
        {
            Debug.Log("Can't unload, no chunks found");
            return;
        }
        List<Chunk> _preloaded = preloadedChunks.Cast<Chunk>().ToList();
        for (int i = _preloaded.Count - 1; i >= 0; i--)
        {
            if (_preloaded[i] != null)
                DestroyImmediate(_preloaded[i].gameObject);
        }

        preloadedChunks = null;
    }

    private void InitVariables()
    {
        _chunkSize = (chunkPrefab.blockSize * chunkPrefab.chunkSize) / chunkPrefab.PixelUnitSize;
        worldLoadingStatus = worldLoadingStatus == null ? GetComponent<LoadingStatus>() : worldLoadingStatus;

        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab not specified!");
            Destroy(this);
            return;
        }

        _chunks = new Dictionary<Vector2, Chunk>();
    }

    private void ForceLoad()
    {
        // Setup Chunks
        IEnumerator e = SetupChunks();
        while (e.MoveNext() != false) ;
        IEnumerator r = RenderChunks();
        while (r.MoveNext() != false) ;
    }

    private IEnumerator SetupChunks()
    {
        int i = 0;
        worldLoadingStatus.isLoading = true;
        worldLoadingStatus.loadingText = "Loading Chunk";
        worldLoadingStatus.finishAmount = chunksY * chunksX;
        worldLoadingStatus.currentAmount = 0;
        if (preloadedChunks != null && preloadedChunks.Length > 0)
            for (int p = 0; p < preloadedChunks.Length; p++)
                InitChunk(preloadedChunks[p]);
        else
            for (int y = 0; y < chunksY; y++)
            {
                for (int x = 0; x < chunksX; x++)
                {
                    InitChunk(InstantiateChunk(x, y));
                    i++;
                    if (i % 250 == 0)
                        yield return null;

                    worldLoadingStatus.Increment();
                }
            }
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
            chunk.SetupGraphics();
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

    //private Chunk CreateChunk(int x, int y)
    //{
    //    chunk.World = this;
    //    chunk.InitialSetup();
    //    chunk.Initialize();
    //    chunk.transform.parent = transform;
    //    // Remove chunk from list if it has been reinitialized
    //    //  chunk.ChunkReInitialized += (c) => { RemoveChunkAtIndex(c.ChunkIndex); };
    //    return chunk;
    //}

    private Chunk InstantiateChunk(int x, int y)
    {
        Chunk chunk = Instantiate(chunkPrefab, transform.position + new Vector3(x * chunkPrefab.ChunkIndexSize, y * chunkPrefab.ChunkIndexSize, 0), Quaternion.identity) as Chunk;
        return chunk;
    }

    private void InitChunk(Chunk chunk)
    {
        chunk.World = this;
        chunk.InitialSetup();
        chunk.Initialize();
        chunk.transform.parent = transform;
    }


    #endregion

    #region Chunk & Blocks

    public bool ContainsChunk(Vector2 index)
    {
        return _chunks.ContainsKey(index);
    }

    public void RemoveChunkAtIndex(Vector2 index)
    {
        if (!_chunks.Remove(index))
            Debug.LogWarning("Trying to remove chunk that does not exist " + index);
    }


    /// <summary>
    /// Add a chunk to the world. The chunk must be initialized before it can be added.
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns></returns>
    public bool AddChunk(Chunk chunk)
    {
        if (Application.isPlaying)
        {
            if (!_chunks.ContainsKey(chunk.ChunkIndex) && chunk.IsInitialized)
            {
                _chunks.Add(chunk.ChunkIndex, chunk);
                return true;
            }
        }
        if (!chunk.IsInitialized)
            Debug.LogWarning("Trying to add chunk which has not be initialized");
        return false;
    }

    public void SetNeighbourChunksToDirty(Chunk chunk)
    {
        Chunk topChunk;
        Chunk bottomChunk;
        Chunk leftChunk;
        Chunk rightChunk;
        if (_chunks.TryGetValue(chunk.ChunkIndex + new Vector2(0, chunk.ChunkIndexSize), out topChunk))
            topChunk.IsDirty = true;
        if (_chunks.TryGetValue(chunk.ChunkIndex + new Vector2(0, -chunk.ChunkIndexSize), out bottomChunk))
            bottomChunk.IsDirty = true;
        if (_chunks.TryGetValue(chunk.ChunkIndex + new Vector2(-chunk.ChunkIndexSize, 0), out leftChunk))
            leftChunk.IsDirty = true;
        if (_chunks.TryGetValue(chunk.ChunkIndex + new Vector2(chunk.ChunkIndexSize, 0), out rightChunk))
            rightChunk.IsDirty = true;
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
            chunk.SetBlockWorldPosition(position, block);
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

    #region Indexing

    /// <summary>
    /// Converts a world position into a chunk index
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 WorldPositionToChunkIndex(Vector2 position)
    {
        position -= transform.position.ToVector2();
        Vector2 mod = new Vector2(position.x % _chunkSize, position.y % _chunkSize);
        position -= mod;
        return position;
    }

    /// <summary>
    /// Converts a world position into a chunk index
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2 WorldPositionToChunkIndex(Vector3 position)
    {
        return WorldPositionToChunkIndex(position.ToVector2());
    }

    #endregion

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(chunksX, chunksY, 0), new Vector3(_chunkSize * chunksX, _chunkSize * chunksY, 0));
    }
}