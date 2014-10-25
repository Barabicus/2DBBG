using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

public class World : MonoBehaviour
{

    private Dictionary<Vector2IndexWrapper, Chunk> _chunks;

    public Chunk chunkPrefab;
    public int chunksX, chunksY;
    public bool preRenderChunks = false;
    public int renderBatch = 8;

    private float _chunkSize;

    #region Initilization

    void Start()
    {
        _chunkSize = (chunkPrefab.blockSize * chunkPrefab.chunkSize) / chunkPrefab.pixelUnitSize;

        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab not specified!");
            Destroy(this);
            return;
        }

        _chunks = new Dictionary<Vector2IndexWrapper, Chunk>();

        SetupChunks();

    }

    private void SetupChunks()
    {
        float blockSize = chunkPrefab.blockSize;
        float chunkSize = chunkPrefab.chunkSize;

        float cachedAmount = (blockSize * chunkSize) / 100f;
        for (int y = 0; y < chunksY; y++)
        {
            for (int x = 0; x < chunksX; x++)
            {
                Chunk chunk = Instantiate(chunkPrefab, transform.position + new Vector3(x * cachedAmount, y * cachedAmount, 0), Quaternion.identity) as Chunk;
                chunk.Init();
                chunk.World = this;
                chunk.GenerateChunk();
                _chunks.Add(chunk.ChunkPosition, chunk);
                if (chunk.ChunkPosition.y == 3.2f)
                    Debug.Log("T: " + chunk.transform.position.ToVector2() + " W: " + chunk.ChunkPosition + " H: "+  chunk.ChunkPosition.GetHashCode());
            }
        }

        // Setup chunk colliders
        foreach (Chunk chunk in _chunks.Values)
        {
            chunk.SetupBoxColliders();
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
        foreach (Chunk chunk in _chunks.Values)
        {
            chunk.RenderChunk();
            yield++;
            if (yield == renderBatch)
            {
                yield = 0;
                yield return null;
            }
        }
    }

    #endregion

    #region Chunk & Blocks

    public Block GetBlockFromPosition(Vector2 position)
    {
        Chunk chunk = GetChunkFromWorldPosition(position);
        if (chunk != null)
            return chunk.GetBlockFromWorldPosition(position);
        else
            return null;
    }

    public void SetBlockWorldPosition(Vector2 position, Block block)
    {
        Chunk chunk = GetChunkFromWorldPosition(position);
        if (chunk != null)
            chunk.SetBlockAtWorldPosition(position, block);
    }

    public void SetBlockWorldPosition(Vector2 position, Vector2 blockIndex, Block block)
    {
        Vector2 setPos = position + blockIndex * (chunkPrefab.blockSize / 100f);
        Chunk chunk = GetChunkFromWorldPosition(setPos);
        if (chunk != null)
            chunk.SetBlockAtWorldPosition(setPos, block);
    }

    public Chunk GetChunkFromWorldPosition(Vector2 position)
    {
        Vector2 index = WorldPositionToChunkIndex(position);
        Chunk chunk;
        _chunks.TryGetValue(new Vector2IndexWrapper(index), out chunk);
        return chunk;
    }

    public Chunk GetChunkFromIndex(Vector2 index)
    {
        Chunk chunk;
        Vector2IndexWrapper indexWrap = index;
        _chunks.TryGetValue(indexWrap, out chunk);
        if (chunk == null)
            Debug.Log("Return NULL: " + index + " : " + indexWrap.GetHashCode());
        return chunk;
    }

    public Chunk GetChunkFromIndex(float x, float y)
    {
        return GetChunkFromIndex(new Vector2(x, y));
    }

    #endregion

    #region Helper Methods

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
            SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new VineBlock());
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), null);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Chunk chunk = GetChunkFromWorldPosition(new Vector2(1.456133f, 0.4429992f));
            chunk.SetBlockAtIndex(0, 0, new DebugBlock());
            Block block = chunk.GetBlockAtIndex(0, 0);
            Vector2 index;
            chunk.GetIndexFromBlock(block, out index);
            Vector2 worldpos = chunk.IndexToWorldPosition(index);
            SetBlockWorldPosition(worldpos, new Vector2(0, 1), null);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Chunk chunk in _chunks.Values)
            {
                for (int x = 0; x < chunk.chunkSize; x++)
                {
                    for (int y = 0; y < chunk.chunkSize; y++)
                    {
                        chunk.SetBlockAtIndex(x, y, new DebugBlock());
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (Chunk chunk in _chunks.Values)
            {
                for (int x = 0; x < chunk.chunkSize; x++)
                {
                    for (int y = 0; y < chunk.chunkSize; y++)
                    {
                        chunk.SetBlockAtIndex(x, y, new StandardBlock());
                    }
                }
            }
        }
    }

    #endregion

    #region Helper Methods

  

    #endregion
}
