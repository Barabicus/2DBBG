using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public Transform player;
    public World world;
    public Vector3 boundsSize;
    public Chunk chunkPrefab;
    public int chunksToMoveAtOnce = 5;

    Chunk[] chunks;
    Vector3 lastPosition;
    Chunk bottomChunk;
    int bottomIndex = 0;

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
        bottomChunk = chunks[0];
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(BoundsPosition, boundsSize * 2);
    }


    void Update()
    {
            if (bottomChunk.transform.position.y < player.transform.position.y - boundsSize.y)
            {
                for (int i = 0; i < world.chunksX * chunksToMoveAtOnce; i++)
                {
                    chunks[i + bottomIndex].transform.position = world.WorldPositionToChunkIndex(new Vector2(chunks[i + bottomIndex].transform.position.x, chunks[i + bottomIndex].transform.position.y + (world.chunksY * chunkPrefab.ChunkIndexSize)));
                    chunks[i + bottomIndex].Initialize();
                    chunks[i + bottomIndex].DrawAllBlocks();
                }
                bottomIndex = bottomIndex + (world.chunksX * chunksToMoveAtOnce) < (world.chunksX * world.chunksY) ? bottomIndex + (world.chunksX * chunksToMoveAtOnce) : 0;
                bottomChunk = chunks[bottomIndex];
            }
    }

}
