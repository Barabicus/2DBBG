using UnityEngine;
using System.Collections;

public class BuilderPlayerController : MonoBehaviour {

    public World world;
    public float speed = 10f;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            world.SetBlockWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), null);
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0));
    }
}
