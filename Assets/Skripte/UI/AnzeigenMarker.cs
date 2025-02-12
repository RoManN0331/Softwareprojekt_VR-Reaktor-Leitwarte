using UnityEngine;
using TMPro;

/// <summary>
/// This class implements graphical cues to highlight a specific display that show the value the player is supposed to manipulate and to mark the target value that the player is supposed to set. 
/// </summary>
public class AnzeigenMarker : MonoBehaviour
{
    /// <param name="moveSpeed">float specifying the speed of vertical movements</param>
    public float moveSpeed = 3f;            
    /// <param name="moveAmount">float specifying the amount of vertical movement</param>
    public float moveAmount = 0.015f;
    /// <param name="fontSizeSpeed">float specifying the speed of the font size change</param>
    public float fontSizeSpeed = 2f;        
    /// <param name="fontSizeAmount">float specifying the amount of font size change</param>
    public float fontSizeAmount = 0.1f;     
    /// <param name="targetNumber">float used to calculate the position of the arrow pointing to the target value</param>
    public float targetNumber = 1000;
    /// <param name="pfeilTransform"> Transfom for animating the arrow pointing to the target value</param>
    private Transform pfeilTransform;
    /// <param name="textMeshPro">TextMeshPro used to mark the display by an !</param>
    private TextMeshPro textMeshPro;
    /// <param name="initialY">float specifying the initial y position of the arrow pointing to the target value</param>
    private float initialY;
    /// <param name="initialFontSize">float specifying the initial font size set for the TextMeshPro</param>
    private float initialFontSize;
    /// <param name="xleft">float specifying the leftmost position of the arrow indicating the target value</param>
    private float xleft = 0.0611f;
    /// <param name="xright">float specifying the rightmost position of the arrow indicating the target value</param>
    private float xright = -0.0734f;
    /// <param name="anzeigeSteuerung">AnzeigeSteuerung used to get upper limit of the display to calculate the target number</param>
    private AnzeigeSteuerung anzeigeSteuerung;
    /// <param name="anzeigeSteuerung2">AnzeigeSteuerung5 used to get upper limit of the display to calculate the target number</param>
    private AnzeigeSteuerung5 anzeigeSteuerung2;

    /// <summary>
    /// This method initialises the arrow pointing to the target value, the ! marking the display and the control component (AnzeigeSteuerung) of the display.
    /// <summary>
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
            anzeigeSteuerung2 = GetComponentInParent<AnzeigeSteuerung5>();
            if (anzeigeSteuerung2 == null)
            {
                Debug.LogError("AnzeigeSteuerung component not found in parent GameObject.");
            }
        }
        
    }

    /// <summary>
    /// This method updates the arrow's position marking the target value and the font size of the ! marking the display.
    /// </summary>
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
        else if (anzeigeSteuerung2 != null)
        {
            // Calculate the x-position based on the targetNumber
            float t = targetNumber / anzeigeSteuerung2.end_Number;
            float newX = Mathf.Lerp(xleft, xright, t);
            // Update the x-position of the Pfeil
            pfeilTransform.localPosition = new Vector3(newX, pfeilTransform.localPosition.y, pfeilTransform.localPosition.z);
        }
    }
}