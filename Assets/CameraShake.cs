using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // The duration of the shake effect in seconds
    public float shakeDuration = 0.5f;

    // The magnitude of the shake effect in units
    public float shakeMagnitude = 0.1f;

    // The rate at which the shake effect decays over time
    public float dampingSpeed = 1.0f;

    // A reference to the camera's transform component
    private Transform camTransform;

    // The original position of the camera before shaking
    private Vector3 originalPos;

    // A flag to indicate whether the camera is shaking or not
    private bool isShaking = false;

    // A timer to keep track of the shake duration
    private float shakeTimer = 0f;

    public GameObject player;

    void Awake()
    {
        // Get the transform component of the camera
        
    }

    void OnEnable()
    {
        // Store the original position of the camera
       
    }

    void Update()
    {
        // Check if the camera is shaking
        if (isShaking)
        {
            // Update the timer
            shakeTimer += Time.deltaTime;

            // Check if the timer has reached the shake duration
            if (shakeTimer >= shakeDuration)
            {
                // Stop shaking and reset the timer
                isShaking = false;
                shakeTimer = 0f;
            }
            else
            {
                // Calculate a random offset based on the shake magnitude
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;

                // Apply the offset to the camera's position
                camTransform.localPosition = new Vector3(camTransform.localPosition.x+x, camTransform.localPosition.y+y, originalPos.z);
            }
        }
        else
        {
            camTransform = GetComponent<Transform>();
            originalPos = camTransform.localPosition;
            if (player.GetComponent<CharacterMovement>().currentPlayerState == CharacterMovement.PlayerState.InsideCannon)
            {
                camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, new Vector3(0, 15, -10), dampingSpeed * Time.deltaTime);
            }
            else
            {
                camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, new Vector3(0, 0, -10), dampingSpeed * Time.deltaTime);
            }
        }
    }

    // A public method that can be called from other scripts to start shaking the camera
    public void Shake()
    {
        // Set the flag to true and reset the timer
        isShaking = true;
        shakeTimer = 0f;
    }
}
