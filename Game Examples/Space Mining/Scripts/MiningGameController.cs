using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiningGameController : MonoBehaviour
{

    public MiningPlayerController player;
    public Transform enemyPrefab;
    public LayerMask layermask;
    public float spawnDistance = 50f;
    public Text dangerLevelText;

    public Color mediumColor;
    public Color dangerColor;

    public Timer dangerIncrease;
    private int spawnFrequency = 350;
    private int dangerLevel = 0;

    // Use this for initialization
    void Start()
    {
        dangerIncrease = new Timer(30);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
        if (dangerLevel != 2 && dangerIncrease.CanTickAndReset())
            IncreaseDangerLevel();
    }

    void IncreaseDangerLevel()
    {
        dangerLevel = Mathf.Min(dangerLevel + 1, 2);
        switch (dangerLevel)
        {
            case 1:
                spawnFrequency = 100;
                dangerLevelText.text = "caution";
                dangerLevelText.color = mediumColor;
                break;
            case 2:
                spawnFrequency = 10;
                dangerLevelText.text = "danger";
                dangerLevelText.color = dangerColor;
                break;
        }
    }

    void SpawnEnemy()
    {
        if(player.LivingState == MiningEntity.EntityLivingState.Alive)
        if (Random.Range(0, spawnFrequency) == 0)
        {
            Vector2 direction = Random.onUnitSphere;
            direction.Normalize();
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, spawnDistance, layermask);
            if (hit.collider == null)
            {
                Transform t = Instantiate(enemyPrefab, player.transform.position + (direction * spawnDistance).ToVector3(), Quaternion.identity) as Transform;
                t.GetComponent<MiningEnemy>().player = player.transform;
            }

            Debug.DrawRay(player.transform.position, direction * spawnDistance, Color.red, 1f);

        }
    }
}
