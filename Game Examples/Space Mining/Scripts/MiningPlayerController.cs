using UnityEngine;
using System.Collections;
public class MiningPlayerController : MiningEntity
{
    public ParticleSystem damageParticles;
    public CamFollow2D camera;
    public Transform miningBulletPrefab;
    public Transform miniTurret;
    public int resources = 0;

    private GunMode gunMode = GunMode.Blaster;

    public enum GunMode
    {
        Blaster,
        Mining
    }

    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = value;
            currentHP = Mathf.Max(currentHP, 0);
            if (currentHP == 0)
                Die();
        }
    }

    public void SetGunMode(string mode)
    {
        gunMode = (GunMode)System.Enum.Parse(typeof(GunMode), mode, true);
    }

    public void SetGunMode(GunMode mode)
    {
        gunMode = mode;
    }

    public void UpgradeBlaster()
    {
        maxShootAmount++;
        shootFreqTimer = new Timer(0.01f);
    }

    public override void LivingUpdate()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        GetComponent<Rigidbody2D>().MoveRotation(GetComponent<Rigidbody2D>().rotation + -Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime);


        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce((transform.up * speed) * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (shootFreqTimer.CanTickAndReset() && canShootBullets && Input.GetKey(KeyCode.Space))
        {
            if (shotSoundFreq.CanTickAndReset())
                GetComponent<AudioSource>().PlayOneShot(lazerClip);

            switch (gunMode)
            {
                case GunMode.Blaster:
                    for (int i = 0; i < Random.Range(minShootAmount, maxShootAmount); i++)
                    {
                        ShootBullet(shootPointLeft, transform.up, bulletPrefab);
                        ShootBullet(shootPointRight, transform.up, bulletPrefab);
                    }
                    break;
                case GunMode.Mining:
                    ShootBullet(shootPointLeft, transform.up, miningBulletPrefab);
                    ShootBullet(shootPointRight, transform.up, miningBulletPrefab);
                    break;
            }
        }

    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            CurrentHP -= 1;
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.layer != LayerMask.NameToLayer("PlayerBullet") && coll.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            CurrentHP -= 1;
            camera.ShakeCamera(0.1f, 0.5f);
            damageParticles.Emit(50);
            GetComponent<AudioSource>().PlayOneShot(explodeClip);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("ResourceOrb"))
        {
            resources++;
            Destroy(other.gameObject);
        }
    }

    public void EnableMiniTurret()
    {
        miniTurret.gameObject.SetActive(true);
    }

}
