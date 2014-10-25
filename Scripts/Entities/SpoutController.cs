using UnityEngine;
using System.Collections;

public class SpoutController : MonoBehaviour
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

    public void Update()
    {
        rigidbody2D.MoveRotation(rigidbody2D.rotation + -Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            rigidbody2D.AddForce(transform.up * speed * Time.deltaTime);
        }
    }


    void FixedUpdate()
    {
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

        if (Input.GetKey(KeyCode.Space))
        {
            ShootBullet(shootPointLeft, transform.up);

            ShootBullet(shootPointRight, transform.up);

        }
    }


    private void ShootBullet(Transform position, Vector3 direction)
    {
        Transform bullet = Instantiate(bulletPrefab, position.position, Quaternion.identity) as Transform;
        bullet.rigidbody2D.AddForce((Quaternion.Euler(0f, 0f, Random.Range(-shootRange, shootRange)) * direction) * Random.Range(bulletSpeedMin, bulletSpeedMax));
    }

}
