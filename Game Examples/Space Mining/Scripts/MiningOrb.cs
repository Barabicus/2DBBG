using UnityEngine;
using System.Collections;

public class MiningOrb : MonoBehaviour
{

    MiningPlayerController player;

    public float followRange = 20f;

    public void Start()
    {
        player = MiningGUIControl.Instance.player;
    }

    public void Update()
    {
        if (player != null)
            if (Vector3.Distance(player.transform.position, transform.position) <= followRange)
                transform.position += ((player.transform.position - transform.position).normalized * 15f * Time.deltaTime);
    }


}
