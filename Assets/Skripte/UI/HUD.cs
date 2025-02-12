using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class displays the instructions for each task the player is supposed to perform in a chosen scenario on the HUD. 
/// </summary>
public class HUD : MonoBehaviour
{
    /// <param name="text">Text that is displayed</param>
    private Text text;
    /// <param name="pattern">string containing a regular expression</param>
    private string pattern;
    /// <param name="sibling">Transform</param>
    private Transform sibling;

    /// <summary>
    /// This method initialises the pattern used to separate tasks, as well as the canvas on which the tasks will be displayed.
    /// </summary>
    private void Start()
    {
        pattern = @">(.*?)<";
        text = GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("Text component not found!");
        }
        // Find the main camera in the XR Origin (XR Rig) > Camera Offset > Main Camera
        Camera mainCamera = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").GetComponent<Camera>();

        // Set the render camera of the Canvas component
        Canvas canvas = transform.parent.GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        // Set the plane distance to bring the UI closer to the camera
        canvas.planeDistance = 0.05f;

        // Set the sorting order to ensure the UI is rendered on top of other objects but behind the HUD
        canvas.sortingOrder = 99;
        
        // Set the parent transform as a child of the current camera
        transform.parent.SetParent(mainCamera.transform);
        
        // Search for the sibling transform named "Hintergrund" and set it to inactive
        sibling = transform.parent.Find("Hintergrund");
        if (sibling != null)
        {
            sibling.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This method extracts the text from the input string and sets it to the text component of the canvas.
    /// </summary>
    /// <param name="inputText">string containing the text to be displayed</param>
    public void setText(string inputText)
    {
        // Remove new lines
        inputText = Regex.Replace(inputText, @"\r\n|\r|\n", string.Empty);
        //Debug.Log("Input Text: " + inputText);
        Match match = Regex.Match(inputText, pattern);
        if (match.Success)
        {
            text.text = match.Groups[1].Value;
            //Debug.Log("Extracted Text: " + match.Groups[1].Value);
            
            if (sibling != null && !match.Groups[1].Value.Equals(""))
            {
                sibling.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("No match found!");
        }
    }

    /// <summary>
    /// This method clears the text displayed on the canvas.
    /// </summary>
    public void clearText()
    {
        text.text = "";
    }
}