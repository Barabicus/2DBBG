using UnityEngine;
using System.Collections;

public class SpoutPlayerInput : MonoBehaviour {

    public Transform bulletPrefab;
    public int spawnAmount = 360;
    public float spawnRadius = 50f;
    public float speed = 300f;


	// Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Vector2 inpPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2();
                Transform t = Instantiate(bulletPrefab, inpPosition.ToVector3() + Random.insideUnitCircle.ToVector3() * spawnRadius, Quaternion.identity) as Transform;
                t.rigidbody2D.AddForce((t.transform.position - inpPosition.ToVector3()).ToVector2().normalized * speed);
            }
        }

    }
}
