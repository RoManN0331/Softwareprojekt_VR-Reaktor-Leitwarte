using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class implements a welcome screen that is displayed at the start of the VR application explaining to the player what they are supposed to do.
/// </summary>
public class WelcomeScreen : MonoBehaviour
{
    /// <param name="forwardButton"> is a button to go to the next page of the welcome message</param>
    public GameObject forwardButton;
    /// <param name="backButton">is a button to go to the previous page of the welcome message</param>
    public GameObject backButton;
    /// <param name="confirmButton"> is a button to close the welcome message</param>
    public GameObject confirmButton;
    /// <param name="scrollPanel"> is a Panel displaying the welcome message</param>
    public GameObject scrollPanel;
    /// <param name="DISABLE_WELCOME_SCREEN"> tracks whether the welcome screen is displayed or not</param>
    public bool DISABLE_WELCOME_SCREEN = false;
    /// <param name="clipbaordInputAction"> is a reference to an InputActionAsset for retrieving the controller button that is used to trigger a chosen scenario</param>
    public InputActionAsset clipbaordInputAction;
    /// <param name="infoTexts"> is an array containing the welcome message</param>
    private string[] infoTexts = new string[] {
        "Sie befinden sich in der Steuerzentrale eines Kernreaktors. Ihre Aufgaben umfassen die Überwachung der Systeme, die Steuerung der Energieproduktion und die Gewährleistung der Sicherheit. Ihnen stehen folgende Elemente zur Verfügung:",
        "📟 Hauptkonsole:\nZentrale Steuereinheit zur Regulierung des Reaktors und Überwachung der Systemparameter.",
        "💡 Statuslampen:\nInformieren Sie über den aktuellen Zustand der Reaktorkomponenten.",
        "📋 Clipboards:\nStandartverfahren verschiedener Szenarien. Starten Sie ein Szenario durch Betätigen der <color=#00aaaa>{INPUT_BTN}</color> Taste",
        "🔍 Gazeguiding Panel:\nEs stehen eine Reihe von unterstüzenden Gaze-Guiding Elementen zur Verfügung. Diese Können nach belieben ausgewählt werden",
        "🚪 Tür:\nVollständiges Zurücksetzen der Simulation, um verschiedene Szenarien erneut zu durchlaufen oder Fehler zu korrigieren.",
        "Machen Sie sich bereit, die Kontrolle zu übernehmen - die Sicherheit des Reaktors liegt in Ihren Händen! 🔥⚡"
    };

    /// <summary>
    /// This method initialises the welcome screen and its components.
    /// </summary>
    void Start()
    {
        //Replace {INPUT_BTN} with actual button
        InputAction action = clipbaordInputAction.FindAction("Clipboard/TriggerClipboardScenario");
        infoTexts[3] = infoTexts[3].Replace("{INPUT_BTN}", action.GetBindingDisplayString(4));
        
        if (DISABLE_WELCOME_SCREEN){
            this.gameObject.SetActive(false);
            return;          
        }
        backButton.SetActive(false);
        confirmButton.SetActive(true);
        scrollPanel.GetComponent<TextMeshProUGUI>().text = infoTexts[0];
    }

    /// <summary>
    /// This method implements the logic for the forward button.
    /// </summary>
    public void Next()    // Display the next text of the infoTexts array
    {
        int currentTextIndex = System.Array.IndexOf(infoTexts, scrollPanel.GetComponent<TextMeshProUGUI>().text);
        Debug.Log(currentTextIndex);
        if (currentTextIndex < infoTexts.Length - 1)
        {
            scrollPanel.GetComponent<TextMeshProUGUI>().text = infoTexts[currentTextIndex + 1];
        }
        if (currentTextIndex == infoTexts.Length - 2)
        {
            forwardButton.SetActive(false);
            //confirmButton.SetActive(true);
        }
        if (currentTextIndex >= 0)
        {
            backButton.SetActive(true);
        }
    }

    /// <summary>
    /// This method implements the logic for the back button.
    /// </summary>
    public void Previous()    // Display the previous text of the infoTexts array
    {
        int currentTextIndex = System.Array.IndexOf(infoTexts, scrollPanel.GetComponent<TextMeshProUGUI>().text);
        if (currentTextIndex > 0)
        {
            scrollPanel.GetComponent<TextMeshProUGUI>().text = infoTexts[currentTextIndex - 1];
        }
        if (currentTextIndex == 1)
        {
            backButton.SetActive(false);
        }
        if (currentTextIndex < infoTexts.Length - 1)
        {
            forwardButton.SetActive(true);
            //confirmButton.SetActive(false);
        }
    }

    /// <summary>
    /// This method implements the logic for the confirm button.
    /// </summary>
    public void Confirm()    // Hide welcome screen
    {
        this.gameObject.SetActive(false);
    }
}
