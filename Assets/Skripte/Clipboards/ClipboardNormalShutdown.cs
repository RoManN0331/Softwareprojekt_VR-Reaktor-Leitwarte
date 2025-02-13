using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// This class implements logic to trigger the normal shutdown scenario of the NPPClient class.
/// </summary>
public class ClipboardNormalShutdown : MonoBehaviour
{
    /// <param name="nppClient"> is a reference to an instance of the NPPClient class</param>
    private NPPClient nppClient;
    /// <param name="actionTrigger">  is a reference to an InputAction</param>
    public InputAction actionTrigger;
    /// <param name="isInteracting"> tracks whether the player interacts with the object this script is attached to</param>
	private bool isInteracting = false;
    /// <param name="interactor"> os a reference to ta XRBaseInteractor</param>
	private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    /// <param name="clipboardActions"> is a reference to an InputActionAsset</param>
	public InputActionAsset clipboardActions;

    /// <summary>
    /// This method initializes clipboardActions and adds ActionListeners to the clipboard's grab interactable.
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
    /// This method removes the ActionListener, disables the ActionTrigger and destroys the object.
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
    /// This method is called when the player interacts with the clipboard.
    /// </summary>
    /// <param name="args"> passes event specific arguments at the beginning of the interaction</param>
	private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isInteracting = true;
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
    }

    /// <summary>
    /// This method is called when the player no longer interacts with the clipboard.
    /// </summary>
    /// <param name="args"> passes event specific arguments at the end of the interaction</param>
    private void OnSelectExited(SelectExitEventArgs args)
    {
        isInteracting = false;
        interactor = null;
    }

    /// <summary>
    /// This method is called when the actionTrigger is released, initiating the normal shutdown scenario on nppClient.
    /// </summary>
    /// <param name="context"> passes event specific arguments</param>
    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (nppClient != null && isInteracting)
        {
            Debug.Log("Setting Normal Shutdown Scenario...");
            nppClient.StartCoroutine(nppClient.SetNormalShutdownScenario());
        }
    }
}