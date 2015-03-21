using UnityEngine;
using System.Collections;

public class MiningEnemy : MiningEntity
{

    public Transform player;
    public float shootDistance = 5;
    public float followDistance = 5;

    private MiningPlayerController playercont;
    private int destroyFreq;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        destroyFreq = Random.Range(100, 750);
        playercont = player.GetComponent<MiningPlayerController>();
    }

    public override void LivingUpdate()
    {
        if (!CheckPlayerLiving())
            return;

        RotateTowardsPlayer();
        MoveTowardsPlayer();
        TryShootPlayer();
    }

    private void TryShootPlayer()
    {
        if (Vector2.Distance(player.position, transform.position) < shootDistance && shootFreqTimer.CanTickAndReset() && playercont.LivingState == EntityLivingState.Alive)
        {
            GetComponent<AudioSource>().PlayOneShot(lazerClip);
            for (int i = 0; i < Random.Range(minShootAmount, maxShootAmount); i++)
            {
                ShootBullet(shootPointLeft, transform.up, bulletPrefab);
                ShootBullet(shootPointRight, transform.up, bulletPrefab);
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (Vector2.Distance(player.position, transform.position) > followDistance)
            GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * speed);
    }

    private void RotateTowardsPlayer()
    {
        var dir = player.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private bool CheckPlayerLiving()
    {
        if ((playercont.LivingState == MiningEntity.EntityLivingState.Dead && Random.Range(0, destroyFreq) == 0))
        {
            Die();
            return false;
        }
        return true;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 9)
        {
            CurrentHP -= 1;
            Destroy(coll.gameObject);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        if (playercont.LivingState == EntityLivingState.Alive)
            MiningGUIControl.Instance.Kills++;
    }

}
