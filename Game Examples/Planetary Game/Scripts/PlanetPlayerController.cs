using UnityEngine;
using System.Collections;

public class PlanetPlayerController : MonoBehaviour {

    public Vector3 pivotPoint;
    public float speed = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivotPoint, Input.GetAxis("Horizontal") * speed * Time.deltaTime);
    }
}
