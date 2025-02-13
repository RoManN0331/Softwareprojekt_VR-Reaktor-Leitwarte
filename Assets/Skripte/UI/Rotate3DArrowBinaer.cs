using UnityEngine;
using System.Collections;

/// <summary>
/// This class implements a 3D arrow rotating in a choppy manner as a visual cue indicating the direction in which the player is supposed to turn a binary switch. 
/// </summary>
public class Rotate3DArrowBinaer : MonoBehaviour
{
    /// <param name="rotationSpeed"> specifies the speed by which the arrow is rotating</param>
    public float rotationSpeed = 500f;      
    /// <param name="flipDirection"> specifies the direction in which the arrow is rotating</param>
    public bool flipDirection = false;      
    /// <param name="moveSpeed"> specifies the speed of vertical movements</param>
    private float moveSpeed = 3f;           
    /// <param name="moveAmount"> specifies the amount of vertical movement</param>
    private float moveAmount = 0.015f;      
    /// <param name="initialY"> specifies the initial y position</param>
    private float initialY;                 

    private bool isRotating = false;        // deprecated

    /// <summary>
    /// This method initialises the arrow's position and rotation.
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