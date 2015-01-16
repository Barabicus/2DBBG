using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FallingSandController : MonoBehaviour
{


    World world;
    public float placeAmount;
    public int placeRadius = 3;
    public int stoneRadius = 3;
    public float tickTime = 10;
    public Slider radiusSlider;

    private List<Chunk> chunks = new List<Chunk>();

    System.Type leftClickSpawn = typeof(FallingWater);

    public string CurrentLeftClickType
    {
        set
        {
            switch (value)
            {
                case "fallingblock":
                    leftClickSpawn = typeof(FallingSand);
                    break;
                case "fallingvirus":
                    leftClickSpawn = typeof(FallingVirus);
                    break;
                case "fallingwater":
                    leftClickSpawn = typeof(FallingWater);
                    break;
                case "seed":
                    leftClickSpawn = typeof(VineBlock);
                    break;
                case "clone":
                    leftClickSpawn = typeof(FallingClone);
                    break;
                case "eraser":
                    leftClickSpawn = null;
                    break;
            }
        }
    }



    // Use this for initialization
    void Start()
    {
        radiusSlider.value = placeRadius;
        world = GetComponent<World>();
        chunks = world.Chunks;
        StartCoroutine(Tick());
    }

    /// <summary>
    /// Use a custom tick to make sure all the chunks are updated from the ground up
    /// </summary>
    /// <returns></returns>
    IEnumerator Tick()
    {
        while (true)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                chunks[i].Tick();
            }
            yield return new WaitForSeconds(tickTime);
        }
    }

    public void ChangePlaceRadius(float radius)
    {
        placeRadius = (int)radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int count = 0;
            foreach (Chunk ch in chunks)
            {
                foreach (Block b in ch.blocks)
                {
                    if (b != null && !(b is WallBlock))
                        count++;
                }
            }
            Debug.Log("count: " + count);
        }


        if (Input.GetMouseButton(1))
        {
            Chunk ch = world.GetChunkFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
            if (ch != null)
            {
                Vector2 index = ch.WorldPositionToIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                ch.SetBlockAtIndex(index, (Block)System.Activator.CreateInstance(leftClickSpawn));
            }
        }


        if (Input.GetMouseButton(0))
        {
            if (leftClickSpawn != null)
            {

                Chunk ch = world.GetChunkFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                if (ch != null)
                {
                    Vector2 index = ch.WorldPositionToIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                    //  ch.SetBlockAtIndex(index, new WallBlock());
                    //for (int y = -placeRadius; y < placeRadius; y++)
                    //    for (int x = -placeRadius; x < placeRadius; x++)
                    //        ch.SetBlockAtIndex(index + new Vector2(x, y), (Block)System.Activator.CreateInstance(leftClickSpawn));

                    for (int i = 1; i < placeRadius; i++)
                        CirclePlace((int)index.x, (int)index.y, i, ch);

                }

            }
            else
            {
                // Eraser
                Chunk ch = world.GetChunkFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                if (ch != null)
                {
                    Vector2 index = ch.WorldPositionToIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                    ch.SetBlockAtIndex(index, new WallBlock());

                    for (int y = -placeRadius; y < placeRadius; y++)
                        for (int x = -placeRadius; x < placeRadius; x++)
                            ch.SetBlockAtIndex(index + new Vector2(x, y), null);
                }
            }
        }
        if (Input.GetMouseButton(2))
        {
            Chunk ch = world.GetChunkFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
            if (ch != null)
            {
                Vector2 index = ch.WorldPositionToIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                ch.SetBlockAtIndex(index, new WallBlock());

                for (int y = -placeRadius; y < placeRadius; y++)
                    for (int x = -placeRadius; x < placeRadius; x++)
                        ch.SetBlockAtIndex(index + new Vector2(x, y), new WallBlock());
            }
        }
    }

    public void CirclePlace(int x0, int y0, int radius, Chunk ch)
    {
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            ch.SetBlockAtIndex(x + x0, y + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(y + x0, x + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(-x + x0, y + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(-y + x0, x + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(-x + x0, -y + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(-y + x0, -x + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(x + x0, -y + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            ch.SetBlockAtIndex(y + x0, -x + y0, (Block)System.Activator.CreateInstance(leftClickSpawn));
            y++;
            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
    }
}
