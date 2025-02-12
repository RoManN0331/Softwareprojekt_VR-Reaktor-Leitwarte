using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// This class is used to trigger the normal shutdown scenario on the NPPClient when the clipboard this script is attached to is interacted with.
/// </summary>
public class ClipboardNormalShutdown : MonoBehaviour
{
    /// <param name="actionTrigger">InputAction to trigger the execution of the shutdown scenario</param>
    public InputAction actionTrigger;
    /// <param name="isInteracting">Boolean to check if the clipboard is currently being interacted with</param>
	private bool isInteracting = false;
    /// <param name="interactor">Reference to the XRBaseInteractor that is currently interacting with the clipboard</param>
	private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    /// <param name="clipboardActions">InputActionAsset containing the clipboard actions</param>	
	public InputActionAsset clipboardActions;
    /// <param name="nppClient">Reference to the NPPClient instance in the scene</param>
    private NPPClient nppClient;


    /// <summary>
    /// This method initializes the clipboardActions and adds ActionListeners to the clipboard's Grab Interactable.
    /// </summary>
    private void Start()
    {
        // Suche nach der NPPClient-Instanz in der Szene

        nppClient = FindObjectOfType<NPPClient>();
        if (nppClient == null)
        {
            Debug.LogError("NPPClient instance not found in the scene.");
            return;
        }
		
        // Add ActionListeners to the XRGrabInteractable of the clipboard
		
		var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
		
        // Add and enable ActionTrigger

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
		} else {
			actionTrigger.Enable();
            actionTrigger.performed += OnActionTriggered;
		}	
    }

    /// <summary>
    /// This method destroys the object and removes the ActionListener and disables the ActionTrigger.
    /// </summary>
    private void OnDestroy()
    {
        if (actionTrigger != null)
        {
            actionTrigger.performed -= OnActionTriggered;
            actionTrigger.Disable();
        }
    }

    /// <summary>
    /// This method is called when the clipboard is being interacted with.
    /// </summary>
    /// <param name="args">SelectEnterEventArgs to pass event specific arguments upon entering the interaction</param>
	private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isInteracting = true;
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    /// <summary>
    /// This method is called when the clipboard is no longer being interacted with.
    /// </summary>
    /// <param name="args">SelectExitEventArgs to pass event specific arguments upon exiting the interaction</param>
    private void OnSelectExited(SelectExitEventArgs args)
    {
        isInteracting = false;
        interactor = null;
    }

    /// <summary>
    /// This method is called when the actionTrigger is performed starting the normal shutdown scenario on the NPPClient.
    /// </summary>
    /// <param name="context">CallbackContext to pass event specific arguments</param>
    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (nppClient != null && isInteracting)
        {
            Debug.Log("Setting Normal Shutdown Scenario...");
            nppClient.StartCoroutine(nppClient.SetNormalShutdownScenario());
        }
    }
}