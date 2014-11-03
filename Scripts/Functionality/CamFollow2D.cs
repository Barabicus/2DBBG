using UnityEngine;
using System.Collections;

public class CamFollow2D : MonoBehaviour {

    public Transform follow;

    public bool followX = true;
    public bool followY = true;
    public float xDefaultPos = 0.0f;
    public float yDefaultPos = 0.0f;

	// Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(followX ? follow.position.x : xDefaultPos, followY ? follow.position.y : yDefaultPos, 0) - new Vector3(0, 0, 10); 
    }
}
