using UnityEngine;
using UnityEngine.UIElements;

public class MountainTurn : MonoBehaviour
{
    public float rotationSpeed = 2f; // Speed of mountain rotation

    private bool isRotatingRight = false; // Flag to indicate if the mountain is currently rotating to the right
    private bool isRotatingLeft = false; // Flag to indicate if the mountain is currently rotating to the left
    private void Start()
    {
        transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f, Space.Self);
        isRotatingRight = true;
    }
    private void Update()
    {
       
        if (isRotatingRight)
        {
            // Rotate the mountain to the left
            transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f, Space.Self);
        }
        else if (isRotatingLeft)
        {
            // Rotate the mountain to the right
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
        }
        else if(GameManager.Instance.currentGameState== GameState.Active)
        {
            // Rotate the mountain continuously
            transform.Rotate(0f, 5 * Time.deltaTime, 0f, Space.Self);
        }
    }

    public void RightSwipe()
    {
        isRotatingRight = true;
        isRotatingLeft = false;
    }

    public void LeftSwipe()
    {
        isRotatingLeft = true;
        isRotatingRight = false;
    }

    public void StopRotation()
    {
        isRotatingRight = false;
        isRotatingLeft = false;
    }
}
