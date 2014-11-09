using UnityEngine;
using System.Collections;

public class PlanetPlayerController : MonoBehaviour {

    public Vector3 pivotPoint;
    public float speed = 10f;
	
	// Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivotPoint, Vector3.forward, Input.GetAxis("Horizontal") * speed * Time.deltaTime);

       transform.position += (pivotPoint - transform.position).normalized * Input.GetAxis("Vertical") * 2f * Time.deltaTime;

        var dir = pivotPoint - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
