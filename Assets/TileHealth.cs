
using UnityEngine;

public class TileHealth : MonoBehaviour
{
    private int hitPoints;
    public float pushbackForce = 20;
    public float pushbackDuration = 0.1f;

    public void SetHitPoints(int value)
    {
        hitPoints = value;
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        // Check if the tile has been destroyed
        if (hitPoints <= 0)
        {
            DestroySurroundingInvisibleBlocks(transform.position);
            Destroy(gameObject);
        }
    }
    private void DestroySurroundingInvisibleBlocks(Vector3 position)
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

        foreach (Vector3 direction in directions)
        {
            Vector3 blockPosition = position + direction;
            Collider2D[] colliders = Physics2D.OverlapPointAll(blockPosition);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("InvisibleTile"))
                {
                    Destroy(collider.gameObject);
                }
            }
        }
    }


}
