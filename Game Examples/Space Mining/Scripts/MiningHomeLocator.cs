using UnityEngine;
using System.Collections;

public class MiningHomeLocator : MonoBehaviour
{

    public Transform home;
    public Transform pivot;
    public float radius = 10f;


    Vector3 Offset
    {
        get
        {
            return (home.position - Camera.main.ScreenToWorldPoint(pivot.position)).normalized * radius;
        }
    }

    void LateUpdate()
    {
        transform.position = pivot.transform.position + Offset;
    }
}
