using UnityEngine;
using System.Collections;

public class PlanetGuard : MonoBehaviour {

    public Vector3 pivotPoint;
    public float speed = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {

        transform.RotateAround(pivotPoint, Vector3.forward, speed * Time.deltaTime);

        var dir = pivotPoint - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
