using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndlessGUI : MonoBehaviour
{

    public Text scoreText;
    public Text gameOverScore;
    public PlayerController player;
    public Transform gameOver;

    private Vector3 lastPlayerPosition;
    private int score = 0;

    // Use this for initialization
    void Start()
    {
        lastPlayerPosition = player.transform.position;
        player.OnKilled += PlayerKilled;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y - lastPlayerPosition.y > 1f)
        {
            score += Mathf.FloorToInt(player.transform.position.y - lastPlayerPosition.y);
            lastPlayerPosition = player.transform.position;
            scoreText.text = "Score: " + score;
        }
    }

    private void PlayerKilled()
    {
        gameOverScore.text = score.ToString();
        gameOver.gameObject.SetActive(true);
    }

    public void Restart()
    {
        Application.LoadLevel("EndlessScene");
    }
}
