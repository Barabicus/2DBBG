using UnityEngine;
using System.Collections;

public class MissileSpawner : MonoBehaviour {

    public Transform missile;
    public float spawnTime = 1f;
    public float spawnRadius = 10f;
    public int spawnMinAmount = 1;
    public int spawnMaxAmount = 5;
    public Vector2 centerPoint;

    void Start()
    {
        StartCoroutine(SpawnMissle());
    }


    IEnumerator SpawnMissle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            for (int i = 0; i < Random.Range(spawnMinAmount, spawnMaxAmount); i++)
            {
                float rotation = Random.Range(0, 360);
                Transform t = Instantiate(missile, new Vector3(Mathf.Sin(rotation) * spawnRadius, Mathf.Cos(rotation) * spawnRadius, 0) + centerPoint.ToVector3(), Quaternion.identity) as Transform;


            }
        }
    }

}
