using UnityEngine;
using System.Collections;

public class FallingSandController : MonoBehaviour
{


    World world;

    // Use this for initialization
    void Start()
    {
        world = GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            world.SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new FallingBlock());
        }
        if (Input.GetMouseButton(1))
        {
            world.SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), new WallBlock());
        }
    }
}
