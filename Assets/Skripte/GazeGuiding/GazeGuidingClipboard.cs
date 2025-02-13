using System;
using System.Text.RegularExpressions;


/// <summary>
/// This class implements logic to highlight tasks on a clipboard a player has to perform to complete a scenario.
/// </summary>
public class GazeGuidingClipboard
{

    private const float DISTANCE_THRESHOLD = 1.0f;      // deprecated
    /// <param name="HIGHLIGHT_TEXT_COLOR"> contains the colour code of the highlight colour</param>
    public  string HIGHLIGHT_TEXT_COLOR = "<color=#00FF00>";
    /// <param name="informationText"> contains information about a scenario</param>
    private string informationText;
    /// <param name="taskList"> is a string array containing a list of tasks the player must perform to complete a scenario</param>
    private string[] taskList;


    /// <summary>
    /// This constructor parses the clipboard text and creates a list of tasks a player must perform to complete a scenario.
    /// </summary>
    /// <param name="clipboardText"> contains the whole text of a clipboard</param>
    public GazeGuidingClipboard(String clipboardText, string Color) // Parse clipboard text into task information and checklist
    {
        HIGHLIGHT_TEXT_COLOR = Color;
        clipboardText = Regex.Replace(clipboardText, @"<color=.*?>|</color>", string.Empty, RegexOptions.Multiline);
        // Split clipboard into information and checklist
        string[] clipboard = Regex.Split(clipboardText, @"Checkliste");
        informationText = clipboard[0];
        string checklist = clipboard[1];

        // Split cheklist items on each number
        this.taskList = Regex.Split(checklist, @"(?=\d+\.\s)", RegexOptions.Multiline);
        taskList = Array.FindAll(taskList, s => !string.IsNullOrWhiteSpace(s));
    }

    /// <summary>
    /// This method highlights a task in tasklist.
    /// </summary>
    /// <param name="n"> specifies a task to be highlighted</param>
    public void HighlightTask(int n)    // Highlight a task in the checklist with HIGHLIGHT_TEXT_COLOR. If n is out of range, nothing will be highlighted.
    {
        if (n < 1 || n > taskList.Length) return;   
        
        taskList[n-1] = HIGHLIGHT_TEXT_COLOR + taskList[n-1] + "</color>"; 
    }

    /// <summary>
    /// This method returns a formatted clipboard text.
    /// </summary>    
    public string GetFormattedClipboardText()   // Build formatted clipboard text with information and checklist
    {
        return informationText + "Checkliste\n" + String.Join("", taskList);
    }
}