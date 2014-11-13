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

                //for (int a = 0; a < missileRadius; a++)
                //    for (int i = 0; i < 360; i++)
                //        chunk.SetBlockWorldPosition(new Vector2(contact.point.x + Mathf.Sin(i) * (chunk.BlockToWorldSize * a), contact.point.y + Mathf.Cos(i) * (chunk.BlockToWorldSize * a)), null);  

                chunk.SetBlockAtIndex(index, null);

                for(int x = -missileRadius; x < missileRadius; x++)
                    for(int y = -missileRadius; y < missileRadius; y++)
                        chunk.SetBlockAtIndex(index + new Vector2(x,y), null);

                //for (int a = 0; a < missileRadius; a++)
                //    for (int i = 0; i < 360; i++)
                //        chunk.SetBlockAtIndex(index + new Vector2(a + Mathf.Round(Mathf.Sin(i)), a + Mathf.Round(Mathf.Cos(i))), null);


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
