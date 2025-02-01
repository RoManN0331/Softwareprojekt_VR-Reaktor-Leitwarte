using UnityEngine;
using System.Collections;

public class Rotate3DArrowBinaer : MonoBehaviour
{
    public float rotationSpeed = 500f; // Rotation speed in degrees per second
    public bool flipDirection = false; // Boolean to flip the rotation direction

    private float moveSpeed = 3f; // Speed of the up and down movement
    private float moveAmount = 0.015f; // Amount of movement on the y-axis

    private float initialY; // Initial y position
    private bool isRotating = false;

    void Start()
    {
        initialY = transform.localPosition.y; // Store the initial y position
        StartCoroutine(RotateAndPause());
    }

    void Update()
    {
        // Move the GameObject up and down on the y-axis
        float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }

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