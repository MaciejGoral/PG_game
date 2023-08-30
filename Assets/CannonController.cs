using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Transform domeCenter;
    public EdgeCollider2D domeCollider;
    public float cannonWidth = 0.5f;
    public float minAngle = -90;
    public float maxAngle = 90;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootForce = 10f;
    public float attackCooldown = 0.5f;
    private float currentCooldown = 0f;
    private bool canShoot = true;
    public GameObject player;
    public int BulletDamage = 20;


    void Update()
    {
        if (player.GetComponent<CharacterMovement>().currentPlayerState == CharacterMovement.PlayerState.InsideCannon)
        {
            {

            }
            if (Input.GetKey(KeyCode.A) && transform.eulerAngles.z < maxAngle || transform.eulerAngles.z > 270)
            {
                transform.RotateAround(domeCenter.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) && transform.eulerAngles.z > minAngle)
            {
                transform.RotateAround(domeCenter.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
            }
            if (canShoot && Input.GetKey(KeyCode.Space)) // Or any other key you prefer
            {
                Shoot();
                canShoot = false;
                currentCooldown = attackCooldown;
            }

            // Cooldown timer logic
            if (!canShoot)
            {
                currentCooldown -= Time.deltaTime;
                if (currentCooldown <= 0)
                {
                    canShoot = true;
                }
            }
        }
        // Cast a ray from the center of the dome towards the cannon
        RaycastHit2D hit = Physics2D.Raycast(domeCenter.position, transform.position - domeCenter.position, Mathf.Infinity, LayerMask.GetMask("Dome"));

        // Position the cannon at the point where the ray intersects with the dome collider
        if (hit.collider != null)
        {
            // Calculate the angle between the cannon and the center of the dome
            float angle = Vector2.SignedAngle(Vector2.right, hit.point - (Vector2)domeCenter.position);

            // Calculate the offset based on the size of the cannon and the angle
            Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * cannonWidth / 2f, Mathf.Sin(angle * Mathf.Deg2Rad) * cannonWidth / 2f);

            // Position the cannon at the point where the ray intersects with the dome collider plus the offset
            transform.position = hit.point + offset;
        }

    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.AddForce(bulletSpawnPoint.right * shootForce, ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().damageAmount = BulletDamage;
        }
    }

}
