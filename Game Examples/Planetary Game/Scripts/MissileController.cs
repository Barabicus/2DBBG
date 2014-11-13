using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

    public Transform missilePrefab;
    public int currentMissiles;
    public int maxMissiles;
    public Vector2 centerPoint;
    public float spawnRadius = 50f;

    void Start()
    {
        currentMissiles = maxMissiles;
    }

	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float rotation = Random.Range(0, 360);
            Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (clickPoint - centerPoint).normalized * spawnRadius;
            dir += centerPoint;

            Missile m = ((Transform)Instantiate(missilePrefab, dir, Quaternion.identity)).GetComponent<Missile>();
            m.targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
