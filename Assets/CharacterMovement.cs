using UnityEngine;
using System.Collections;
using TMPro;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    public int Damage = 1;
    private bool isBeingPushed = false;
    private Vector2 pushbackDirection;
    public float pushbackForce = 200f;
    public float pushbackDuration = 0.5f;
    public float stateChangeCooldown = 1f; 
    private float currentStateChangeCooldown = 0f;
    private bool canChangeState = true;
    public Canvas interactionCanvas;
    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI shopInteractionText;
    public Transform entranceTrigger;
    public float interactionRange = 0.5f;
    public Camera mainCamera;
    private float cameraTransitionDuration = 1f;
    public Canvas shop;
    public Transform shopTrigger;
    private float drillTime = 0.5f;
    public GameObject drillSprite;
    public enum PlayerState
    {
        OutsideCannon,
        InsideCannon
    }

    public PlayerState currentPlayerState = PlayerState.OutsideCannon;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (currentPlayerState == PlayerState.OutsideCannon && isBeingPushed==false)
        {

            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");
            if (horizontalMovement > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (horizontalMovement < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            Vector2 movement = new Vector2(horizontalMovement, verticalMovement).normalized;
            Vector2 velocity = movement * speed;
            rb.velocity = new Vector2(velocity.x, velocity.y);
            if(horizontalMovement==0 && verticalMovement == 0)
            {
               rb.velocity = Vector2.zero;
            }
            if (Vector2.Distance(transform.position, shopTrigger.position) < interactionRange)
            {
                shopInteractionText.enabled = true;
                if (Input.GetKey(KeyCode.R) && canChangeState)
                {
                    shop.enabled = !shop.enabled;
                    canChangeState = false;
                    currentStateChangeCooldown = stateChangeCooldown;
                }
            }
            else
            {
                shop.enabled = false;
                shopInteractionText.enabled = false;
            }
        }

        if (Vector2.Distance(transform.position, entranceTrigger.position) < interactionRange)
        {
            if(currentPlayerState == PlayerState.OutsideCannon) 
            {
                interactionCanvas.enabled = true;
            }
            else
            {
                interactionCanvas.enabled = false;
            }

            if (Input.GetKey(KeyCode.R))
            {
                ToggleCannonState();
            }
        }
        else
        {
            interactionCanvas.enabled = false;
        }
        if (!canChangeState)
        {
            currentStateChangeCooldown -= Time.deltaTime;
            if (currentStateChangeCooldown <= 0)
            {
                canChangeState = true;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tile") || collision.gameObject.CompareTag("OreTile"))
        {
            TileHealth tileHealth = collision.gameObject.GetComponent<TileHealth>();
            if (tileHealth != null)
            {
                int damage = Damage;

                // Bounce the player back
                if (!isBeingPushed)
                {
                    // Get the input values
                    float horizontal = Input.GetAxisRaw("Horizontal")==0 ? 0: Mathf.Sign(Input.GetAxisRaw("Horizontal"));
                    float vertical = Input.GetAxisRaw("Vertical") == 0 ? 0 : Mathf.Sign(Input.GetAxisRaw("Vertical"));
                    pushbackDirection = collision.contacts[0].normal;

                     // Check if the input is in the same direction as the collision normal
                    if ((horizontal!=0 && (int)(pushbackDirection.x)!=0 && horizontal != Mathf.Sign((int)pushbackDirection.x)) || (vertical!=0 && (int)(pushbackDirection.y)!=0 && vertical != Mathf.Sign((int)pushbackDirection.y)))
                    {
                        // Damage the tile or ore tile
                        tileHealth.TakeDamage(damage);
                        GetComponent<AudioSource>().Play();
                        StartCoroutine(ShowDrillSprite(collision));
                        StartCoroutine(ApplySmoothPushback());
                    }

                }
            }
        }
    }

    private IEnumerator ApplySmoothPushback()
    {
        isBeingPushed = true;
        float elapsedTime = 0f;

        while (elapsedTime < pushbackDuration)
        {
            // Calculate the current force based on the elapsed time
            float currentForce = Mathf.Lerp(pushbackForce, 0f, elapsedTime / pushbackDuration);

            // Apply the force gradually over time
            rb.AddForce(pushbackDirection * currentForce);

            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isBeingPushed = false;
    }
    public void ToggleCannonState()
    {
        if (canChangeState)
        {
            float cameraTargetYOffset = (currentPlayerState == PlayerState.OutsideCannon) ? 3f : -3f;
            float cameraTargetOrthoSize = (currentPlayerState == PlayerState.OutsideCannon) ? 9f : 6f;

            Vector3 cameraTargetPosition = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + cameraTargetYOffset, mainCamera.transform.position.z);

            StartCoroutine(MoveCameraSmoothly(cameraTargetPosition, cameraTargetOrthoSize));

            if (currentPlayerState == PlayerState.OutsideCannon)
            {
                currentPlayerState = PlayerState.InsideCannon;
                GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
                GetComponent<SpriteRenderer>().enabled = false;
                // Handle entering the cannon
                // Lock player movement, set player position inside the cannon
            }
            else if (currentPlayerState == PlayerState.InsideCannon)
            {
                currentPlayerState = PlayerState.OutsideCannon;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<SpriteRenderer>().enabled = true;
                // Handle exiting the cannon
                // Unlock player movement
            }

            canChangeState = false;
            currentStateChangeCooldown = stateChangeCooldown;
        }
    }

    private IEnumerator MoveCameraSmoothly(Vector3 targetPosition, float targetOrthoSize)
    {
        Vector3 startingPosition = mainCamera.transform.position;
        float startingOrthoSize = mainCamera.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < cameraTransitionDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / cameraTransitionDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startingOrthoSize, targetOrthoSize, elapsedTime / cameraTransitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetOrthoSize;
    }
    IEnumerator ShowDrillSprite(Collision2D collision)
    {
        drillSprite.GetComponent<ParticleSystem>().Play();
        // Get the angle between the collision normal and the up vector
        float angle = Vector2.SignedAngle(Vector2.up, collision.contacts[0].normal);

        // Rotate the drill sprite by that angle
        drillSprite.transform.rotation = Quaternion.Euler(0, 0, angle+90);

        // Move the drill sprite to the collision point
        drillSprite.transform.position = collision.contacts[0].point;

        // Enable the drill sprite renderer
        drillSprite.GetComponent<SpriteRenderer>().enabled = true;

        // Wait for the drill time
        yield return new WaitForSeconds(drillTime);

        // Disable the drill sprite renderer
        drillSprite.GetComponent<SpriteRenderer>().enabled = false;
    }

}
