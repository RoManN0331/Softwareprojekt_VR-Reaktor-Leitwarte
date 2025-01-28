using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ClipboardNormalShutdown : MonoBehaviour
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
		
		var actionMap = clipboardActions.FindActionMap("Clipboard");
		
		if (actionMap == null)
		{
			Debug.LogError("Action Map 'Clipboard' not found in InputActionAsset.");
			return;
		}
		
		actionTrigger = actionMap.FindAction("TriggerClipboardScenario");
		
		if (actionTrigger == null)
		{
			Debug.LogError("Action 'TriggerClipboardScenario' not found in Action Map 'Clipboard'.");
			return;
		}
		
		Debug.Log($"Binding: {actionTrigger.bindings[0].path}");
		
        if (actionTrigger != null)
        {
			Debug.LogError("ActionTrigger NOT null.");
            Enable();
			actionTrigger.Enable();
			Debug.Log("ActionTrigger enabled.");
            actionTrigger.performed += OnActionTriggered;
        }
		
    }
	
	private void Enable()
    {
		Debug.Log("Enabled Action");
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
		Debug.Log(interactable);
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
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
		Debug.Log("Entered Clipboard");
        isInteracting = true;
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
		Debug.Log("Exited Clipboard");
        isInteracting = false;
        interactor = null;
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
		Debug.Log("Action Triggered: " + context.action.name);
        if (nppClient != null && isInteracting)
        {
            Debug.Log("Setting Normal Shutdown Scenario...");
            nppClient.StartCoroutine(nppClient.SetNormalShutdownScenario());
        }
    }
}