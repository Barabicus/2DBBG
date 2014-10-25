using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	// Use this for initialization
    public float destroyTime = 5f;

    void Start()
    {
        StartCoroutine(Destroy());
    }
	
	// Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == 8)
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
