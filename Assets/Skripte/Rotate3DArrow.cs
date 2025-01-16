using UnityEngine;

public class Rotate3DArrow : MonoBehaviour
{

    // Update is called once per frame
    public float rotationSpeed = 235f; // Rotation speed in degrees per second
    public bool flipDirection = false; // Boolean to flip the rotation direction

    // Update is called once per frame
    void Update()
    {
        // Determine the rotation direction
        float direction = flipDirection ? -1f : 1f;

        // Rotate the GameObject around the y-axis
        transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0);

        // Adjust the x scale
        float xScale = flipDirection ? -0.075f : 0.075f;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }
}
