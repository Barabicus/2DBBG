using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine.UI;

public class World : MonoBehaviour
{

    private Dictionary<Vector2, Chunk> _chunks;

    public Chunk chunkPrefab;
    public int chunksX, chunksY;
    public bool preRenderChunks = false;
    public int renderBatch = 8;
    public Text text;

    private float _chunkSize;

    #region Initilization

    void Start()
    {
        //if (1 % ((chunkPrefab.chunkSize * chunkPrefab.blockSize) / chunkPrefab.pixelUnitSize) != 0)
        //{
        //    Debug.Log(1 % (chunkPrefab.chunkSize * chunkPrefab.blockSize) + " : " + (chunkPrefab.chunkSize * chunkPrefab.blockSize));
        //    Debug.LogError("Chunk Index must increase in multiples of 1");
        //    return;
        //}

        _chunkSize = (chunkPrefab.blockSize * chunkPrefab.chunkSize) / chunkPrefab.pixelUnitSize;

        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab not specified!");
            Destroy(this);
            return;
        }

        _chunks = new Dictionary<Vector2, Chunk>();

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
                _chunks.Add(chunk.ChunkIndex, chunk);
            }
        }

        StartCoroutine(CreateChunkColliders());

    }

    private IEnumerator CreateChunkColliders()
    {
        int i = 0;
        // Setup chunk colliders
        foreach (Chunk chunk in _chunks.Values)
        {
            text.text = "Creating Collider: " + i + " / " + _chunks.Values.Count;
            chunk.SetupBoxColliders();
            i++;
            if (i % 50 == 0)
                yield return null;
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
            text.text = "Rendering: " + i + " / " + _chunks.Values.Count;
            chunk.RenderChunk();
            yield++;
            if (yield == renderBatch)
            {
                yield = 0;
                yield return null;
            }
            i++;
        }

        // Destroy GUI when finished loading
        Destroy(text.transform.parent.gameObject);
        
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
        _chunks.TryGetValue(index, out chunk);
        return chunk;
    }

    public Chunk GetChunkFromIndex(Vector2 index)
    {
        Chunk chunk;
        _chunks.TryGetValue(index, out chunk);
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
        if (Input.GetMouseButton(2))
        {
            SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new StandardBlock());
        }
    }

    #endregion

    #region Helper Methods

  

    #endregion
}
