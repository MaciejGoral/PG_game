using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20;
    private void Start()
    {
        // Call the DestroyBullet function after 3 seconds
        Destroy(gameObject, 3.0f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
        Debug.Log("Collision with " + collision.gameObject.name);
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
        }
        Destroy(gameObject);
    }
}
