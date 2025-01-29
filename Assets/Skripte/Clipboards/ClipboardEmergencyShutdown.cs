using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ClipboardEmergencyShutdown : MonoBehaviour
{
    private NPPClient nppClient; // Referenz zu NPPClient
    public InputAction actionTrigger;
	private bool isInteracting = false;
	private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
	
	public InputActionAsset clipboardActions;

    private void Start()
    {
        // Suche nach der NPPClient-Instanz in der Szene
        nppClient = FindObjectOfType<NPPClient>();
        if (nppClient == null)
        {
            Debug.LogError("NPPClient instance not found in the scene.");
            return;
        }
		
		Debug.LogError("NPPClient instance found.");
		
		var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
		
		var actionMap = clipboardActions.FindActionMap("Clipboard");
		
		if (actionMap == null)
		{
			Debug.LogError("Action Map 'Clipboard' not found in InputActionAsset.");
			return;
		}
		
		actionTrigger = actionMap.FindAction("TriggerClipboardScenario");
		// Debug.Log($"Binding: {actionTrigger.bindings[0].path}");
		
		if (actionTrigger == null)
		{
			Debug.LogError("Action 'TriggerClipboardScenario' not found in Action Map 'Clipboard'.");
			return;
		} else {
			Debug.LogError("ActionTrigger NOT null.");
			actionTrigger.Enable();
			Debug.Log("ActionTrigger enabled.");
            actionTrigger.performed += OnActionTriggered;
		}
		
		Debug.Log($"Binding: {actionTrigger.bindings[0].path}");
		
       
		
    }

    private void OnDestroy()
    {
        if (actionTrigger != null)
        {
            actionTrigger.performed -= OnActionTriggered;
            actionTrigger.Disable();
        }
    }
	
	private void OnSelectEntered(SelectEnterEventArgs args)
    {
		Debug.Log("Entered Clipboard for Emergency Shutdown");
        isInteracting = true;
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
		Debug.Log("Exited Clipboard for Emergency Shutdown");
        isInteracting = false;
        interactor = null;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
		Debug.Log("Action Triggered: " + context.action.name);
		Debug.Log(isInteracting);
		Debug.Log(interactor);
        if (nppClient != null && isInteracting)
        {
            Debug.Log("Setting Emergency Shutdown Scenario...");
            nppClient.StartCoroutine(nppClient.SetEmergencyShutdownScenario());
        }
    }
}