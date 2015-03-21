using UnityEngine;
using System.Collections;

public class CamFollow2D : MonoBehaviour {

    [HideInInspector]
    public Transform follow;

        [HideInInspector]
    public bool followX = true;
    [HideInInspector]
    public bool followY = true;
    [HideInInspector]
    public float xDefaultPos = 0.0f;
    [HideInInspector]
    public float yDefaultPos = 0.0f;

    public Vector2 offset = Vector2.zero;
    public float force = 0;
    public float time = 50;

    void Update()
    {
        if (time > 0)
        {
            time = Mathf.Max(time - Time.deltaTime, 0);
            offset = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
        }
        else
            offset = Vector2.zero;
    }

	// Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(followX ? follow.position.x : xDefaultPos, followY ? follow.position.y : yDefaultPos, 0) - new Vector3(0, 0, 10) + offset.ToVector3(); 
    }

    public void ShakeCamera(float force, float time)
    {
        this.force = force;
        this.time = time;
    }
}
