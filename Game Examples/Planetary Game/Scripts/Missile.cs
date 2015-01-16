using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{

    public Vector2 targetPos;
    public int missileRadius = 10;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 9);

        rigidbody2D.AddForce((targetPos - transform.position.ToVector2()).normalized * 5f, ForceMode2D.Impulse);

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 11)
        {
            Chunk chunk = coll.gameObject.GetComponent<Chunk>();

            foreach (ContactPoint2D contact in coll.contacts)
            {
                Vector2 index = chunk.GetIndexFromContact(contact);

                chunk.SetBlockAtIndex(index, null);

                //for (int y = 0; y < missileRadius; y++)
                //    for (int x = 0; x < missileRadius; x++)
                //        chunk.SetBlockAtIndex(index + new Vector2(Mathf.Sin(x) + y, Mathf.Cos(x) + y), null);


                for (int x = -missileRadius; x < missileRadius; x++)
                    for (int y = -missileRadius; y < missileRadius; y++)
                        chunk.SetBlockAtIndex(index + new Vector2(x, y), null);


            }
        }
        Destroy(gameObject);

    }

    void Update()
    {
        var dir = targetPos.ToVector3() - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
