using UnityEngine;
using System.Collections;

public class MiningBackgroundCamera : MonoBehaviour
{

    public Transform mainCamera;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Scale(mainCamera.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
