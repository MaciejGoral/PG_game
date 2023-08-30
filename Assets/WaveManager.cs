using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Array of spawn points
    public TextMeshProUGUI waveCounterText;
    public GameObject dome;
    public int domeHealth = 100;
    public Slider domeHealthSlider;
    public Camera mainCamera;

    private int currentWave = 1;
    private int enemiesInWave = 2; // Starting enemies per wave
    private int enemiesAlive = 0; // Enemies alive in the current wave

    public float timeBetweenWaves = 10f; // Time between waves
    private float waveTimer;

    void Start()
    {
        waveTimer = timeBetweenWaves;
        domeHealthSlider.maxValue = domeHealth;
    }
    public void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    void Update()
    {
        domeHealthSlider.value = domeHealth;
        if (waveTimer <= 0)
        {
            StartCoroutine(SpawnWave());
            waveTimer = timeBetweenWaves;
        }

        if (enemiesAlive == 0)
        {
            waveTimer -= Time.deltaTime;
        }

        UpdateWaveCounterText();
    }

    IEnumerator SpawnWave()
    {
        dome.GetComponent<AudioSource>().Play();
        enemiesAlive = enemiesInWave;
        for (int i = 0; i < enemiesInWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            enemyMovement.waveManager = this;
            enemyMovement.targetPosition = transform;
            // Wait for 0.5 seconds before spawning the next enemy
            yield return new WaitForSeconds(2f);
        }

        enemiesInWave += 2;
        currentWave++;
    }


    void UpdateWaveCounterText()
    {
        if (enemiesAlive > 0)
        {
            waveCounterText.text = "Enemies Left: " + enemiesAlive.ToString();
        }
        else
        {
            waveCounterText.text = "Next Wave: " + Mathf.Ceil(waveTimer).ToString("0") + "s";
        }
    }

    public void damageDome (int damageAmount)
    {
        GetComponent<AudioSource>().Play();
        domeHealth -= damageAmount;
        mainCamera.GetComponent<CameraShake>().Shake();
        if (domeHealth <= 0)
        {
            SceneManager.LoadScene("DefeatScreen");
        }
    }

}
