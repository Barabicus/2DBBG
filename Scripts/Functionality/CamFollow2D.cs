using UnityEngine;
using System.Collections;

public class CamFollow2D : MonoBehaviour {

    public Transform follow;

    public bool followX = true;
    public bool followY = true;

	// Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(followX ? follow.position.x : 0f, followY ? follow.position.y : 0f, 0) - new Vector3(0, 0, 10); 
    }
}
