using UnityEngine;

public class PfeilNfS : MonoBehaviour
{
    private float moveSpeed = 3f; // Speed of the up and down movement
    public float moveAmount = 0.0055f; // Amount of movement on the y-axis
    public float offset = -0.01f; // Offset to adjust the starting position
    
    private float initialY; // Initial y position

    public GazeGuidingTarget target;
    
    void Start()
    {
        initialY = transform.localPosition.y; // Store the initial y position
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.4f); // Set the initial position
    }

    void Update()
    {
        // Move the GameObject up and down on the y-axis
        float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, newY + offset, transform.localPosition.z);

        // Rotate the GameObject to point at the target
        if (target != null)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }
}
