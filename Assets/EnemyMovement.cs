using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform targetPosition;
    public int maxHealth = 100;
    private int currentHealth;
    public WaveManager waveManager;
    public int damage = 1;
    private float timer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        MoveTowardsTarget();

        if (transform.position == targetPosition.position)
        {
            Destroy(gameObject);
        }
    }

    void MoveTowardsTarget()
    {
        // Get the rigidbody component of your object
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Calculate the direction vector towards the target position
        Vector3 direction = (targetPosition.position - transform.position).normalized;
        // Calculate the distance to move in this frame
        float distance = moveSpeed * Time.deltaTime;
        // Move the rigidbody towards the target position
        rb.MovePosition(transform.position + direction * distance);
    }


    public void TakeDamage(int damageAmount)
    {
        GetComponent<AudioSource>().Play();
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (waveManager != null)
        {
            waveManager.EnemyDestroyed();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // If the other collider belongs to the dome
        if (collision.gameObject.tag == "Dome" && timer >= 1f)
        {
            waveManager.damageDome(damage);
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
