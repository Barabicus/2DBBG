using UnityEngine;
using System.Collections;

public class MiningSpaceStation : MonoBehaviour {

    public Transform purchaseGUI;
    public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), 5f * Time.deltaTime);

        if (Vector3.Distance(player.position, transform.position) > 15f)
        {
            purchaseGUI.gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        purchaseGUI.gameObject.SetActive(true);
    }
}
