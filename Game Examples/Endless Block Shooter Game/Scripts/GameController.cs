using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public Transform player;
    public World world;
    public Vector3 boundsSize;
    public Chunk chunkPrefab;
    public int chunkUpdateBatch = 5;

    Chunk[] chunks;
    Vector3 lastPosition;
    Chunk bottomChunk;
    int bottomIndex = 0;
    int topIndex;
    Queue<Chunk> chunkUpdateQueue;

    Vector3 BoundsPosition
    {
        get { return new Vector3(10, player.transform.position.y, 0); }
    }

    void Start()
    {
        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk prefab is not assigned. Assign chunk prefab so that chunk size values may be retrieved");
            Destroy(this);
            return;
        }
        if (world == null)
        {
            Debug.LogError("World is not assigned. Please assign the world");
            Destroy(this);
            return;
        }
        chunks = world.Chunks.ToArray();
        topIndex = chunks.Length - world.chunksX;
        bottomChunk = chunks[0];
        chunkUpdateQueue = new Queue<Chunk>();
        StartCoroutine(UpdateChunks());
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(BoundsPosition, boundsSize * 2);

        if (Application.isPlaying)
        {
            Gizmos.DrawCube(chunks[topIndex].transform.position - new Vector3(0.5f, 0.5f, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawCube(bottomChunk.transform.position - new Vector3(0.5f, 0.5f, 0), new Vector3(1f, 1f, 1f));

        }

    }


    void Update()
    {
        if (bottomChunk.transform.position.y < player.transform.position.y - boundsSize.y)
        {
            for (int i = 0; i < world.chunksX; i++)
            {
                chunks[i + bottomIndex].transform.position = world.WorldPositionToChunkIndex(new Vector2(chunks[i + bottomIndex].transform.position.x, chunks[i + bottomIndex].transform.position.y + (world.chunksY * chunkPrefab.ChunkIndexSize)));
              //  chunks[i + bottomIndex].Initialize();
             //   chunks[i + bottomIndex].DrawAllBlocks();
                chunkUpdateQueue.Enqueue(chunks[i + bottomIndex]);
            }

            //for (int i = 0; i < world.chunksX; i++)
            //{
            //    chunks[topIndex + i].IsDirty = true;
            //}

            bottomIndex = AdvanceIndex(bottomIndex);
            topIndex = AdvanceIndex(topIndex);
            bottomChunk = chunks[bottomIndex];
        }
    }

    int AdvanceIndex(int index)
    {
        return index + (world.chunksX) < (world.chunksX * world.chunksY) ? index + (world.chunksX) : 0;
    }

    private IEnumerator UpdateChunks()
    {
        while (true)
        {
            for (int i = 0; i < chunkUpdateBatch; i++)
            {
                if (chunkUpdateQueue.Count > 0)
                {
                    Chunk chunk = chunkUpdateQueue.Dequeue();
                    chunk.Initialize();
                  //  chunk.DrawAllBlocks();
                }
            }
            yield return null;

        }
    }

}
