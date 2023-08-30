using UnityEngine;

public class OreBlock : MonoBehaviour
{
    public GameObject oreFragmentPrefab;
    public int fragmentCount = 3;
    public float fragmentForce = 5f;

    private bool isQuitting = false;

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            SpawnOreFragments();
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void SpawnOreFragments()
    {
        if (gameObject.scene.isLoaded)
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                GameObject fragment = Instantiate(oreFragmentPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = fragment.GetComponent<Rigidbody2D>();
                Vector2 forceDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                rb.AddForce(forceDirection * fragmentForce, ForceMode2D.Impulse);
            }
        }
    }
}
