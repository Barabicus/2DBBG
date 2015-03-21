using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiningGUIControl : MonoBehaviour
{

    public MiningPlayerController player;
    public Transform orbPrefab;
    public World world;
    public Text min;
    public Text max;
    public Text resource;
    public Transform spritePrefab;
    public int backgroundTiles = 50;
    public Text killsText;

    public Transform blasterUpgradeGUI;
    private int _kills = 0;

    public static MiningGUIControl Instance
    {
        get;
        set;
    }

    public int Kills
    {
        get { return _kills; }
        set { _kills = value; }
    }

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        CreateBackground();
    }

    void CreateBackground()
    {
        for (int x = -backgroundTiles; x < backgroundTiles; x++)
            for (int y = -backgroundTiles; y < backgroundTiles; y++)
                Instantiate(spritePrefab, new Vector2(x * 6, y * 6), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            player.SetGunMode(MiningPlayerController.GunMode.Blaster);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            player.SetGunMode(MiningPlayerController.GunMode.Mining);

        min.text = Mathf.Round(player.currentHP).ToString();
        max.text = Mathf.Round(player.maxHp).ToString();
        resource.text = player.resources.ToString();
        killsText.text = "kills: " + Kills;
    }


    public void UpgradeBlaser()
    {
        if (player.resources >= 50)
        {
            player.UpgradeBlaster();
            SubtractResources(50);
            Destroy(blasterUpgradeGUI.gameObject);
        }
    }

    public void PurchaseMiniTurret()
    {
        if (player.resources >= 50)
        {
            player.EnableMiniTurret();
            SubtractResources(50);
        }
    }

    private void SubtractResources(int amount)
    {
        player.resources = Mathf.Max(player.resources - amount, 0);
    }
}
