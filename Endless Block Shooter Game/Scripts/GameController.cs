using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public Transform player;
    public World world;
    public Vector3 boundsSize;
    public Chunk chunkPrefab;
    public float chunkUpdatetime = 1f;
    Vector3 lastPosition;

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
        UpdateChunks();
        StartCoroutine(RemoveCheck());
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(BoundsPosition, boundsSize * 2);
    }

    void FixedUpdate()
    {
        if (lastPosition != player.transform.position)
        {
            lastPosition = player.transform.position;
            UpdateChunks();
        }
    }

    void UpdateChunks()
    {
            Debug.DrawRay(new Vector3(0, player.transform.position.y + boundsSize.y, 0), new Vector3(10f, 0, 0), Color.red, 0.1f);

            if (!world.ContainsChunk(new Vector2(0, BoundsPosition.y + boundsSize.y)))
            {
                for (float x = 0; x < boundsSize.x * 2; x += chunkPrefab.ChunkIndexSize)
                {
                    world.AddChunkAtindex(world.WorldPositionToChunkIndex(new Vector2(x, BoundsPosition.y + boundsSize.y)));
                }
            }
    }

    IEnumerator RemoveCheck()
    {
        while (true)
        {
            Bounds bounds = new Bounds(BoundsPosition, boundsSize * 2);
            foreach (Chunk c in world.Chunks)
            {
                if (!bounds.Contains(c.transform.position))
                {
                    world.RemoveChunkAtIndex(c.ChunkIndex);
                }
            }
            yield return new WaitForSeconds(chunkUpdatetime);
        }
    }

}
