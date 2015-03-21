using UnityEngine;
using System.Collections;

public class MiningZoomOutOnDead : MonoBehaviour {

    public MiningPlayerController cont;
    Animator anim;
    private bool zooming = false;

	// Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
    void Update()
    {
        if (!zooming && cont.LivingState == MiningEntity.EntityLivingState.Dead)
        {
            Invoke("ZoomOut", 2f);
            zooming = true;
        }
    }

    void ZoomOut()
    {
        anim.enabled = true;
    }
}
