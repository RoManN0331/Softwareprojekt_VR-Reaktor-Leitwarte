using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ClipboardNormalShutdown : MonoBehaviour
{
    private NPPClient nppClient; // Referenz zu NPPClient
    public InputActionProperty actionTrigger; 

    private void Start()
    {
        // Suche nach der NPPClient-Instanz in der Szene
        nppClient = FindObjectOfType<NPPClient>();
        if (nppClient == null)
        {
            Debug.LogError("NPPClient instance not found in the scene.");
            return;
        }

        // Input-Aktion für die G-Taste registrieren
        if (actionTrigger.action != null)
        {
            actionTrigger.action.performed += ctx => SetScenario();
        }
    }

    private void OnDestroy()
    {
        // Deregistriere die Input-Aktion, um Speicherlecks zu vermeiden
        if (actionTrigger.action != null)
        {
            actionTrigger.action.performed -= ctx => SetScenario();
        }
    }

    private void SetScenario()
    {
        if (nppClient != null)
        {
            Debug.Log("Setting Normal Shutdown Scenario...");
            nppClient.StartCoroutine(nppClient.SetNormalShutdownScenario());
        }
    }
}