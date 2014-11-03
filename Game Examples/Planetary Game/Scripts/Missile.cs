using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{

    public Vector2 targetPos;

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
                 chunk.SetBlockAtContact(contact, null);
                //Vector2 worldPos = chunk.IndexToWorldPosition(chunk.GetIndexFromContact(contact));
                //chunk.SetBlockAtWorldPosition(worldPos, null);
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        var dir = targetPos.ToVector3() - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
