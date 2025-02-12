using UnityEngine;

/// <summary>
/// This class implements a 3 dimensional rotating arrow as a visual cue indicating the direction the player is supposed to turn an exact switch. 
/// </summary>
public class Rotate3DArrow : MonoBehaviour
{
    /// <param name="rotationSpeed">float specifying the speed by which the arrow is rotating</param>
    public float rotationSpeed = 235f;
    /// <param name="flipDirection">boolean specifying the direction the arrow is rotating in</param>
    public bool flipDirection = false;
    /// <param name="moveSpeed">float specifying the speed of vertical movements</param>
    private float moveSpeed = 3f; 
    /// <param name="moveAmount">float specifying the amount of vertical movement</param>
    private float moveAmount = 0.015f; 
    /// <param name="initialY">float specifying the initial y position of the arrow</param>
    private float initialY; 

    /// <summary>
    /// This method initialises the arrow's position.
    /// <summary>
    void Start()
    {
        initialY = transform.localPosition.y; // Store the initial y position
    }

    /// <summary>
    /// This method updates the arrow's position for each frame, rotating the arrow continuously in the direction specified in flipDirection (false: clockwise, true: counterclockwise).
    /// </summary>
    void Update()
    {
        // Determine the rotation direction
        float direction = flipDirection ? -1f : 1f;

        // Rotate the GameObject around the y-axis
        transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0);

        // Adjust the x scale
        float xScale = flipDirection ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);

        // Move the GameObject up and down on the y-axis
        float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}
