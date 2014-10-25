using UnityEngine;
using System.Collections;

public class SpoutConfig : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 9);
        Physics2D.IgnoreLayerCollision(9, 10);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}

public static class Vector3Extention
{

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}

public static class MathfExtention
{
    public static float FloorFloat2Decimal(this Mathf math)
    {
        return 0.0f;
    }
}
