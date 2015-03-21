using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float destroyTime = 5f;

    void Start()
    {
        StartCoroutine(Destroy());
    }
	

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == 11)
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                Chunk ch = contact.collider.gameObject.GetComponent<Chunk>();
                if(ch != null)
                ch.SetBlockAtContact(contact, null);
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
