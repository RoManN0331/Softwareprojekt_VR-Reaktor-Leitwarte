using System;
using System.Text.RegularExpressions;

/// <summary>
/// This class is responsible for parsing text on a clipboard to extract scenario information and a checklist of tasks for the player, as well as highlighting the current task.
/// </summary>
public class GazeGuidingClipboard
{

    private const float DISTANCE_THRESHOLD = 1.0f;              // deprecated?
    /// <param name="HIGHLIGHT_TEXT_COLOR">string containing the color code for the highlighted text</param>
    public  string HIGHLIGHT_TEXT_COLOR = "<color=#00FF00>";
    /// <param name="informationText">string containing the portion of the text about the scenario</param>
    private string informationText;
    /// <param name="taskList">string array containing the individual tasks</param>
    private string[] taskList;

    /// <summary>
    /// This constructor parses the clipboard text into information about the scenario and a checklist of tasks to be performed by the player.
    /// </summary>
    /// <param name="clipboardText">string containing the whole text on the clipboard</param>
    public GazeGuidingClipboard(String clipboardText, string Color)     // Parse clipboard text into task information and checklist
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
    /// This method highlights a task in the checklist with HIGHLIGHT_TEXT_COLOR.
    /// </summary>
    /// <param name="n"> int specifying the task to be highlighted</param>
    public void HighlightTask(int n)    // Highlight a task in the checklist with HIGHLIGHT_TEXT_COLOR. If n is out of range, nothing will be highlighted.
    {
        if (n < 1 || n > taskList.Length) return;   
        
        taskList[n-1] = HIGHLIGHT_TEXT_COLOR + taskList[n-1] + "</color>"; 
    }

    /// <summary>
    /// This method returns the formatted clipboard text.
    /// </summary>
    public string GetFormattedClipboardText()       // Build formatted clipboard text with information and checklist
    {
        return informationText + "Checkliste\n" + String.Join("", taskList);
    }
}