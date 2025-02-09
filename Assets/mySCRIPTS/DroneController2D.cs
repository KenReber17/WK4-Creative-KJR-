using UnityEngine;

public class DroneController2D : MonoBehaviour
{
    public float maxSpeed = 4f; // Maximum speed the drone can reach
    public float accelerationRate = 5f; // How quickly the drone accelerates
    public float decelerationRate = 2f; // How quickly the drone decelerates when no input is given

    private PlayerMovement playerMovement; // Reference to PlayerMovement script to check if controlling drone
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 currentVelocity = Vector2.zero; // To track current velocity for acceleration
    private bool isFacingRight = true; // Track the direction the drone is facing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
    }

    void Update()
    {
        if (playerMovement != null && playerMovement.controllingDrone)
        {
            // Movement based on player input
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            if (moveInput != Vector2.zero)
            {
                // Accelerate in the direction of input
                currentVelocity = Vector2.MoveTowards(currentVelocity, moveInput.normalized * maxSpeed, accelerationRate * Time.deltaTime);
                Flip(moveInput.x); // Flip based on horizontal input
            }
            else
            {
                // Decelerate if there's no input
                currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, decelerationRate * Time.deltaTime);
            }

            // Apply the calculated velocity to the Rigidbody
            rb.linearVelocity = currentVelocity;
        }
        else // If not controlling the drone
        {
            // Stop the drone immediately
            rb.linearVelocity = Vector2.zero;
            // Reset currentVelocity to ensure no residual speed
            currentVelocity = Vector2.zero;
           
        }

        // Keep drone upright by resetting rotation
        transform.rotation = Quaternion.identity;
    }

    private void Flip(float horizontalInput)
    {
        if (horizontalInput != 0 && ((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0)))
        {
            isFacingRight = !isFacingRight;
            // Flip the sprite
            spriteRenderer.flipX = !isFacingRight;
        }
    }
}