using UnityEngine;
using System.Collections;

public class EndlessCollider : MonoBehaviour {

    public Transform player;

    public float xPos;

	
    void FixedUpdate()
    {
        transform.position = new Vector3(xPos, player.transform.position.y, 0);
    }
}
