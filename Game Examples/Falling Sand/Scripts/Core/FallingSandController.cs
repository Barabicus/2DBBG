using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FallingSandController : MonoBehaviour
{


    World world;
    public float placeAmount;
    public int placeRadius = 3;
    public int stoneRadius = 3;
    public int gameSpeed = 1;
    public float tickTime = 10;
    public Slider radiusSlider;

    private List<Chunk> chunks = new List<Chunk>();

    System.Type leftClickSpawn = typeof(WallBlock);

    public string SelectedString
    {
        get;
        set;
    }

    public string CurrentLeftClickType
    {
        set
        {
            switch (value)
            {
                case "Sand":
                    leftClickSpawn = typeof(FallingSand);
                    break;
                case "Virus":
                    leftClickSpawn = typeof(FallingVirus);
                    break;
                case "Water":
                    leftClickSpawn = typeof(FallingWater);
                    break;
                case "Vine":
                    leftClickSpawn = typeof(VineBlock);
                    break;
                case "Clone":
                    leftClickSpawn = typeof(FallingClone);
                    break;
                case "Eraser":
                    leftClickSpawn = null;
                    break;
                case "Fire":
                    leftClickSpawn = typeof(FallingFire);
                    break;
                case "Wall":
                    leftClickSpawn = typeof(WallBlock);
                    break;
                case "Ice":
                    leftClickSpawn = typeof(FallingIce);
                    break;
                case "Grass":
                    leftClickSpawn = typeof(FallingGrass);
                    break;
                case "Maze":
                    leftClickSpawn = typeof(FallingMaze);
                    break;
                case "Steel":
                    leftClickSpawn = typeof(FallingSteel);
                    break;
                case "VirusSolid":
                    leftClickSpawn = typeof(FallingVirusSolid);
                    break;
                case "Tree":
                    leftClickSpawn = typeof(FallingTree);
                    break;
                case "Hydrogen":
                    leftClickSpawn = typeof(FallingHydrogen);
                    break;
            }
            SelectedString = value;
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
            for (int s = 0; s < gameSpeed; s++)
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

    public void ChangeGameSpeed(float speed)
    {
        gameSpeed = (int)speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
            radiusSlider.value = Mathf.Max(radiusSlider.value - 1, radiusSlider.minValue);

        if (Input.GetKeyDown(KeyCode.RightBracket))
            radiusSlider.value = Mathf.Min(radiusSlider.value + 1, radiusSlider.maxValue);

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
        }


        if (Input.GetMouseButton(1))
        {
            Chunk ch = world.GetChunkFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
            if (ch != null)
            {
                Vector2 index = ch.WorldPositionToIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
                ch.SetBlockAtIndex(index, CreateBlock(leftClickSpawn));
            }
        }


        if (Input.GetMouseButton(0))
        {
            Chunk ch = world.GetChunkFromWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());
            if (ch != null)
            {
                Vector2 index = ch.WorldPositionToIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2());

                for (int i = 1; i < placeRadius; i++)
                    CirclePlace((int)index.x, (int)index.y, i, ch);
            }
        }
    }

    /// <summary>
    /// Place blocks as a circle at points x, y with the radius size
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="radius"></param>
    /// <param name="ch"></param>
    public void CirclePlace(int x0, int y0, int radius, Chunk ch)
    {
        int x = radius;
        int y = 0;
        int radiusError = 1 - x;

        while (x >= y)
        {
            ch.SetBlockAtIndex(x + x0, y + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(y + x0, x + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(-x + x0, y + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(-y + x0, x + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(-x + x0, -y + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(-y + x0, -x + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(x + x0, -y + y0, CreateBlock(leftClickSpawn));
            ch.SetBlockAtIndex(y + x0, -x + y0, CreateBlock(leftClickSpawn));
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

    /// <summary>
    /// Creates a block that can instantly update
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    public FrequentBlock CreateBlock(System.Type blockType)
    {
        if (leftClickSpawn == null)
            return null;

        FrequentBlock block = (FrequentBlock)System.Activator.CreateInstance(leftClickSpawn);
        block.CanUpdate = true;
        return block;
    }
}
