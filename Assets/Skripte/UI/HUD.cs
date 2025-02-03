using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Text text;
    private string pattern;

    private void Start()
    {
        pattern = @">(.*?)<";
        text = GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError("Text component not found!");
        }
    }

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
        }
        else
        {
            Debug.LogWarning("No match found!");
        }
    }

    public void clearText()
    {
        text.text = "";
    }
}