using UnityEngine;

public class PfeilNfS : MonoBehaviour
{

    /// <summary>
    /// This class implements a directional cue indicating to the player where to turn to find the object the player is supposed to interact with.
    /// </summary>


    /// <param name="moveSpeed">float specifying the speed of vertical movements</param>
    /// <param name="moveAmount">float specifying the amount of vertical movement</param>
    /// <param name="offset">float to offset the starting position </param>
    /// <param name="initialY">float specifying the initial y position</param>
    /// <param name="target">GazeGuidingTarget</param>

    private float moveSpeed = 3f; // Speed of the up and down movement
    public float moveAmount = 0.0055f; // Amount of movement on the y-axis
    public float offset = -0.01f; // Offset to adjust the starting position
    
    private float initialY; // Initial y position

    public GazeGuidingTarget target;
    
/// <summary>
/// This method initialises the position of the arrow.
/// </summary>

    void Start()
    {
        initialY = transform.localPosition.y; // Store the initial y position
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.4f); // Set the initial position
    }

/// <summary>
/// This method updates the arrow's vertical position as well as its orientation, continuously rotating the arrow to point towards the object that the player is supposed to interact with (i.e. whose GazeGuidingTarget component is set as target).  
/// </summary>

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
