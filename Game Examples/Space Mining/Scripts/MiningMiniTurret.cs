using UnityEngine;
using System.Collections;

public class MiningMiniTurret : MonoBehaviour
{

    public float rotateSpeed;
    public LayerMask layerMask;
    public MiningPlayerController player;
    public AudioClip lazerShot;

    public Transform bulletPrefab;

    private Timer switchTargetTimer;
    private Timer shootTimerFreq;

    private Transform target;
    public Transform shootPoint;

    // Use this for initialization
    void Start()
    {
        switchTargetTimer = new Timer(1);
        shootTimerFreq = new Timer(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.LivingState != MiningEntity.EntityLivingState.Alive)
            return;

        if (switchTargetTimer.CanTickAndReset()) ;
        LookForEnemies();

        if (target != null)
        {
            RotateTowards(target.position);
            if (shootTimerFreq.CanTickAndReset())
                ShootBullet(shootPoint, transform.up);
        }

    }

    void LookForEnemies()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position.ToVector2(), 20f, layerMask);

        if (hit != null)
        {
            target = hit.transform;
        }
        else
            target = null;

    }

    void RotateTowards(Vector3 targetPos)
    {
        var dir = targetPos - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotateSpeed);
    }

    private void ShootBullet(Transform position, Vector3 direction)
    {
        Transform bullet = Instantiate(bulletPrefab, position.position, Quaternion.identity) as Transform;
        bullet.GetComponent<Rigidbody2D>().AddForce((Quaternion.Euler(0f, 0f, Random.Range(-1, 1)) * direction) * Random.Range(1000, 3000));
        GetComponent<AudioSource>().PlayOneShot(lazerShot);
    }
}
