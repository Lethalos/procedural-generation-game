using UnityEngine;

public class PlaneFlying : MonoBehaviour
{
    public Vector3 flightDirection = Vector3.right;  // Set the desired flight direction

    private Vector3 initialPosition;  // Stores the initial position of the airplane
    private bool isFlying = false;  // Indicates if the airplane is currently flying
    public bool triggerEntered = false;
    public float speed=0.5f;


    int random;


    private void Start()
    {
        // Store the initial position of the airplane
        initialPosition = transform.position;
       
    }

    private void Update()
    { 
        
       AirplaneFly();
        
    }

    public void AirplaneFly()
    {
         // Default speed
        Vector3 movement = flightDirection.normalized * speed * Time.deltaTime;
        transform.position += movement;
    }

    
    public void ResetPosition()
    {
        // Reset the airplane's position and any other necessary properties
        transform.position = initialPosition;
    }

    public void StartFlight()
    {
        // Start the flight of the airplane
        isFlying = true;
    }

    public void StopFlight()
    {
        // Stop the flight of the airplane
        isFlying = false;
    }

    public void OnTriggerEnter(Collider other)
    {   
        // Check if the airplane has collided with the target collider
        if (other.gameObject.CompareTag("Target"))
        {
            triggerEntered = true;
            ResetPosition();

        }
    }
}
