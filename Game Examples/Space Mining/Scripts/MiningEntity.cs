using UnityEngine;
using System.Collections;

public class MiningEntity : MonoBehaviour
{

    public Transform bulletPrefab;
    public Transform shootPointBack;
    public Transform shootPointLeft;
    public Transform shootPointRight;
    public float speed = 100f;
    public float rotSpeed = 200f;
    public float shootRange = 10f;
    public int minShootAmount = 1;
    public int maxShootAmount = 2;
    public float bulletSpeedMin = 1000f;
    public float bulletSpeedMax = 3000f;
    public bool canShootBullets = false;
    public float currentHP;
    public float maxHp;
    public AudioClip lazerClip;
    public AudioClip explodeClip;
    public float shootFrequency = 0.1f;
    public ParticleSystem deathParticle;

    protected Timer shotSoundFreq;
    protected Timer shootFreqTimer;
    protected EntityLivingState livingState = EntityLivingState.Alive;


    public enum EntityLivingState
    {
        Alive,
        Dead
    }

    public EntityLivingState LivingState
    {
        get
        {
            return livingState;
        }
    }

    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            if (livingState != EntityLivingState.Alive)
                return;

            currentHP = value;
            currentHP = Mathf.Max(currentHP, 0);
            if (currentHP == 0)
                Die();
        }
    }

    public bool IsDead
    {
        get
        {
            return currentHP == 0;
        }
    }

    public virtual void Start()
    {
        shotSoundFreq = new Timer(0.1f);
        shootFreqTimer = new Timer(shootFrequency);
    }

    public void Update()
    {
        switch (livingState)
        {
            case EntityLivingState.Alive:
                LivingUpdate();
                break;
            case EntityLivingState.Dead:
                DeadUpdate();
                break;
        }
    }

    public virtual void OnDead()
    {

    }

    public virtual void LivingUpdate()
    {

    }

    public virtual void DeadUpdate()
    {

    }

    public void Die()
    {
        deathParticle.Emit(100);
        for (int i = transform.childCount - 1; i >= 0; i--)
        {

            Rigidbody2D rigid = transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.isKinematic = false;
            }
            transform.GetChild(i).gameObject.layer = 0;
        }
        GetComponent<AudioSource>().PlayOneShot(explodeClip);
        gameObject.layer = 0;
        livingState = EntityLivingState.Dead;
        OnDead();
    }

    public void ShootBullet(Transform position, Vector3 direction, Transform bulletPrefab)
    {
        Transform bullet = Instantiate(bulletPrefab, position.position, Quaternion.identity) as Transform;
        bullet.GetComponent<Rigidbody2D>().AddForce((Quaternion.Euler(0f, 0f, Random.Range(-shootRange, shootRange)) * direction) * Random.Range(bulletSpeedMin, bulletSpeedMax));
    }


}
