using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WelcomeScreen : MonoBehaviour
{
    public GameObject forwardButton;
    public GameObject backButton;
    public GameObject confirmButton;

    public GameObject scrollPanel;

    public bool DISABLE_WELCOME_SCREEN = false;

    public InputActionAsset clipbaordInputAction;
    private string[] infoTexts = new string[] {
        "Sie befinden sich in der Steuerzentrale eines Kernreaktors. Ihre Aufgaben umfassen die Überwachung der Systeme, die Steuerung der Energieproduktion und die Gewährleistung der Sicherheit. Ihnen stehen folgende Elemente zur Verfügung:",
        "📟 Hauptkonsole:\nZentrale Steuereinheit zur Regulierung des Reaktors und Überwachung der Systemparameter.",
        "💡 Statuslampen:\nInformieren Sie über den aktuellen Zustand der Reaktorkomponenten.",
        "📋 Clipboards:\nStandartverfahren verschiedener Szenarien. Starten Sie ein Szenario durch Betätigen der <color=#00aaaa>{INPUT_BTN}</color> Taste",
        "🔍 Gazeguiding Panel:\nEs stehen eine Reihe von unterstüzenden Gaze-Guiding Elementen zur Verfügung. Diese Können nach belieben ausgewählt werden",
        "🚪 Tür:\nVollständiges Zurücksetzen der Simulation, um verschiedene Szenarien erneut zu durchlaufen oder Fehler zu korrigieren.",
        "Machen Sie sich bereit, die Kontrolle zu übernehmen - die Sicherheit des Reaktors liegt in Ihren Händen! 🔥⚡"
    };

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

    // Display the next text of the infoTexts array
    public void Next()
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

    // Display the previous text of the infoTexts array
    public void Previous()
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

    // Hide welcome screen
    public void Confirm()
    {
        this.gameObject.SetActive(false);
    }
}
