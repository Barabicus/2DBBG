using UnityEngine;
using System.Collections;

public class MiningExtractionBullet : MonoBehaviour
{

    public float destroyTime = 5f;

    void Start()
    {
        StartCoroutine(Destroy());
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 11)
        {
            foreach (ContactPoint2D cont in coll.contacts)
            {

                Chunk ch = cont.collider.gameObject.GetComponent<Chunk>();
                if (ch != null)
                {
                    ch.SetBlockAtContact(cont, new MiningExtractionBlock(100));
                }
            }
            Destroy(gameObject);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
