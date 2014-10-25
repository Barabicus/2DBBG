using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

    public Transform follow;
	
	// Update is called once per frame
    void LateUpdate()
    {
        transform.position = follow.position - new Vector3(0, 0, 10); 
    }
}
