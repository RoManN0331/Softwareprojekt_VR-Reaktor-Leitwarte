using UnityEngine;
using System.Collections;

public class Rotate3DArrowBinaer : MonoBehaviour
{

    /// <summary>
    /// This class implements a 3 dimensional rotating arrow as a visual cue indicating the direction the player is supposed to turn a binary switch. 
    /// </summary>

    /// <param name="rotationSpeed">float specifying the speed by which the arrow is rotating</param>
    /// <param name="flipDirection">boolean specifying the direction the arrow is rotating in</param>
    /// <param name="moveSpeed">float specifying the speed of vertical movements</param>
    /// <param name="moveAmount">float specifying the amount of vertical movement</param>
    /// <param name="initialY">float specifying the initial y position</param>

    public float rotationSpeed = 500f;      // Rotation speed in degrees per second
    public bool flipDirection = false;      // Boolean to flip the rotation direction

    private float moveSpeed = 3f;           // Speed of the up and down movement
    private float moveAmount = 0.015f;      // Amount of movement on the y-axis

    private float initialY;                 // Initial y position
    private bool isRotating = false;        // deprecated?

/// <summary>
/// This method initialises the arrows position and rotation
/// <summary>

    void Start()
    {
        initialY = transform.localPosition.y; // Store the initial y position
        StartCoroutine(RotateAndPause());
    }

/// <summary>
/// This method updates the arrow's position.
/// </summary>

    void Update()
    {
        // Move the GameObject up and down on the y-axis
        float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

/// <summary>
/// This method rotates the arrow in 90 degree increments in the direction specified by flipDirection (false: clockwise, true: counterclockwise).
/// </summary>

    private IEnumerator RotateAndPause()
    {
        while (true)
        {
            float xScale = flipDirection ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            // Determine the rotation direction
            float direction = flipDirection ? -1f : 1f;

            // Rotate 90 degrees smoothly
            float targetAngle = transform.localRotation.eulerAngles.y + (90f * direction);
            while (Mathf.Abs(Mathf.DeltaAngle(transform.localRotation.eulerAngles.y, targetAngle)) > 0.1f)
            {
                float step = rotationSpeed * Time.deltaTime;
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, targetAngle, 0), step);
                yield return null;
            }

            
            // Pause for a moment
            yield return new WaitForSeconds(1f); // Adjust the pause duration as needed
        }
    }
}