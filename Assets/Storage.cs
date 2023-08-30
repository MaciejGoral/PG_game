using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Storage : MonoBehaviour
{
    // Start is called before the first frame update
    public int iron = 0;
    public int copper = 0;
    public GameObject player;
    public TextMeshProUGUI IronCounter;
    public TextMeshProUGUI CopperCounter;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        IronCounter.text = "Iron: " + iron;
        CopperCounter.text = "Copper: " + copper;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OreBlock")
        {
            // Freeze the position of the collision object
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            Debug.Log(collision.gameObject.GetComponent<oreName>().OreName);
            if (collision.gameObject.GetComponent<oreName>().OreName == "Iron")
            {
                iron++;
                Debug.Log("Iron: " + iron);
            }
            else if (collision.gameObject.GetComponent<oreName>().OreName == "Copper")
            {
                copper++;
                Debug.Log("Copper " + copper);
            }
            else if (collision.gameObject.GetComponent<oreName>().OreName == "Victorium")
            {
                // Play particle system
                ParticleSystem particleSystem = collision.gameObject.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }

                // Delay before loading the victory screen
                StartCoroutine(PullToCenter(collision.gameObject));
                StartCoroutine(LoadVictoryScreenWithDelay("VictoryScreen", 2.0f)); // Adjust the delay time as needed
                return; // Exit the method to prevent further processing of the collision object
            }

            // Apply gravitational effect to pull the object to the center
            player.GetComponent<OreAttachment>().DetachOre(collision.gameObject);
            collision.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(PullToCenter(collision.gameObject));
        }
    }

    private IEnumerator PullToCenter(GameObject obj)
    {
        float duration = 1.0f; // Adjust the duration as needed
        float elapsedTime = 0f;
        Vector3 initialPosition = obj.transform.position;
        Vector3 centerPosition = transform.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            obj.transform.position = Vector3.Lerp(initialPosition, centerPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is at the center
        obj.transform.position = centerPosition;

        // Shrink the object (you can use a Coroutine for gradual shrinking)
        StartCoroutine(ShrinkAndDestroy(obj));
    }

    private IEnumerator ShrinkAndDestroy(GameObject obj)
    {
        float duration = 1.0f; // Adjust the duration as needed
        float elapsedTime = 0f;
        Vector3 initialScale = obj.transform.localScale;
        GetComponent<AudioSource>().Play();
        ParticleSystem particleSystem = obj.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is completely scaled down
        obj.transform.localScale = Vector3.zero;

        // Destroy the object
        Destroy(obj);
    }

    private IEnumerator LoadVictoryScreenWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

}
