using UnityEngine;


public class Rotate3DArrow : MonoBehaviour
{
    public float rotationSpeed = 235f; // Rotation speed in degrees per second
    public bool flipDirection = false; // Boolean to flip the rotation direction
    
    private float moveSpeed = 3f; // Speed of the up and down movement
    private float moveAmount = 0.015f; // Amount of movement on the y-axis

    private float initialY; // Initial y position

    void Start()
    {
        initialY = transform.localPosition.y; // Store the initial y position
    }

    void Update()
    {
        // Determine the rotation direction
        float direction = flipDirection ? -1f : 1f;

        // Rotate the GameObject around the y-axis
        transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0);

        // Adjust the x scale
        float xScale = flipDirection ? -0.075f : 0.075f;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);

        // Move the GameObject up and down on the y-axis
        float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}
