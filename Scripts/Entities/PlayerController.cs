using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public Transform bulletPrefab;
    public Transform shootPointBack;
    public Transform shootPointLeft;
    public Transform shootPointRight;
    public float speed = 5f;
    public float rotSpeed = 5f;
    public float shootRange = 50f;
    public int minShootAmount = 5;
    public int maxShootAmount = 10;
    public float bulletSpeedMin = 100f;
    public float bulletSpeedMax = 200f;
    public bool sprayBullet = true;
    public bool clickCausesExplode = false;
    public bool canShootBullets = false;

    public event System.Action OnKilled;


    public void Update()
    {

        rigidbody2D.MoveRotation(rigidbody2D.rotation + -Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            rigidbody2D.AddForce(transform.up * speed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0) && clickCausesExplode)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = mousePos.ToVector2().ToVector3();
            for (int i = 0; i < 250; i++)
            {
                Transform bullet = Instantiate(bulletPrefab, mousePos + Random.insideUnitCircle.ToVector3() * 0.25f, Quaternion.identity) as Transform;
                bullet.rigidbody2D.AddForce((bullet.transform.position - mousePos).normalized * Random.Range(bulletSpeedMin, bulletSpeedMax));
            }
        }

        if (canShootBullets && Input.GetKey(KeyCode.Space))
        {
            for (int i = 0; i < Random.Range(minShootAmount, maxShootAmount); i++)
            {
                ShootBullet(shootPointLeft, transform.up);

                ShootBullet(shootPointRight, transform.up);
            }

        }

        if (Input.GetKey(KeyCode.W))
        {
            if (sprayBullet)
            {
                for (int i = 0; i < Random.Range(minShootAmount, maxShootAmount); i++)
                {
                    ShootBullet(shootPointBack, -transform.up);
                }
            }
        }

    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 11)
        {
            if (OnKilled != null)
                OnKilled();

            Die();
        }
    }


    private void Die()
    {
        gameObject.SetActive(false);
        for (int x = 0; x < 500; x++)
        {
            Transform bullet = Instantiate(bulletPrefab, transform.position.ToVector2() + Random.insideUnitCircle, Quaternion.identity) as Transform;
            bullet.rigidbody2D.AddForce((bullet.transform.position - transform.position).normalized * 5, ForceMode2D.Impulse);
        }
    }

    private void ShootBullet(Transform position, Vector3 direction)
    {
        Transform bullet = Instantiate(bulletPrefab, position.position, Quaternion.identity) as Transform;
        bullet.rigidbody2D.AddForce((Quaternion.Euler(0f, 0f, Random.Range(-shootRange, shootRange)) * direction) * Random.Range(bulletSpeedMin, bulletSpeedMax));
    }

}
