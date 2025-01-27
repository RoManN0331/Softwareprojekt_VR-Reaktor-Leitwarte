using UnityEngine;
using TMPro;

public class AnzeigenMarker : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed of the up and down movement
    public float moveAmount = 0.015f; // Amount of movement on the y-axis
    public float fontSizeSpeed = 2f; // Speed of the font size change
    public float fontSizeAmount = 0.1f; // Amount of font size change

    public float targetNumber = 1000;

    private Transform pfeilTransform;
    private TextMeshPro textMeshPro;
    private float initialY; // Initial y position
    private float initialFontSize; // Initial font size

    private float xleft = 0.0611f;
    private float xright = -0.0734f;

    private AnzeigeSteuerung anzeigeSteuerung; // Reference to the AnzeigeSteuerung component
    void Start()
    {
        // Find the child GameObject named "Pfeil"
        pfeilTransform = transform.Find("Pfeil");
        if (pfeilTransform != null)
        {
            initialY = pfeilTransform.localPosition.y; // Store the initial y position
        }
        else
        {
            Debug.LogError("Child GameObject 'Pfeil' not found.");
        }

        // Find the TextMeshPro component in the child GameObject named "AusrufeZeichen"
        Transform ausrufeZeichenTransform = transform.Find("AusrufeZeichen");
        if (ausrufeZeichenTransform != null)
        {
            textMeshPro = ausrufeZeichenTransform.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                initialFontSize = textMeshPro.fontSize; // Store the initial font size
            }
            else
            {
                Debug.LogError("TextMeshPro component not found on 'AusrufeZeichen'.");
            }
        }
        else
        {
            Debug.LogError("Child GameObject 'AusrufeZeichen' not found.");
        }
        
        // Find the AnzeigeSteuerung component in the parent GameObject
        anzeigeSteuerung = GetComponentInParent<AnzeigeSteuerung>();
        if (anzeigeSteuerung == null)
        {
            Debug.LogError("AnzeigeSteuerung component not found in parent GameObject.");
        }
    }

    void Update()
    {
        if (pfeilTransform != null)
        {
            // Move the GameObject up and down on the y-axis smoothly
            float newY = initialY + Mathf.Sin(Time.time * moveSpeed) * moveAmount;
            pfeilTransform.localPosition = new Vector3(pfeilTransform.localPosition.x, newY - 0.0025f, pfeilTransform.localPosition.z);
        }

        if (textMeshPro != null)
        {
            // Change the font size smoothly
            float newFontSize = initialFontSize + Mathf.Sin(Time.time * fontSizeSpeed) * fontSizeAmount;
            textMeshPro.fontSize = newFontSize;
        }
        if (anzeigeSteuerung != null)
        {
            // Calculate the x-position based on the targetNumber
            float t = targetNumber / anzeigeSteuerung.end_Number;
            float newX = Mathf.Lerp(xleft, xright, t);
            // Update the x-position of the Pfeil
            pfeilTransform.localPosition = new Vector3(newX, pfeilTransform.localPosition.y, pfeilTransform.localPosition.z);
        }
    }
}