using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

/// <summary>
/// This class is used to control water pump one in an NPP simulation.
/// </summary>
public class WP1 : MonoBehaviour
{

    ///<summary>This Enum defines two types of rotary switches binary and exact</summary>
    private enum ReglerTypeEnum
    {
        Genau = 0,
        Binaer = 1
    }

    ///<param name="ReglerType"> specifies the type of rotary switch</param>
    private ReglerTypeEnum ReglerType = ReglerTypeEnum.Genau;
    ///<param name="to_rotate"> specifies the handle the player must interact with to rotate the switch</param>
    public GameObject to_rotate;
    [Range(0, 100)]
    ///<param name="Percent"> specifies the percentage the switch has been rotated based on its leftmost position</param>
    public int Percent = 0;
    ///<param name="StartRotation"> specifies  the angle of the switches leftmost position</param>
    private int StartRotation = -90;
    ///<param name="EndRotation"> specifies the angle of the switches rightmost position</param>
    private int EndRotation = 90;
    ///<param name="lastPressTime"> specifies the last time the player didinteract with the switch</param>
    private float lastPressTime = 0f;
    ///<param name="pressCooldown"> specifies a cooldown between interactions with the switch</param>
    private float pressCooldown = 0.1f; // 1 second cooldown
    ///<param name="interactor"> is a reference to an interactor</param>
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    ///<param name="isInteracting"> tracks whether the player is currently interacting with the switch</param>
    private bool isInteracting = false;
    ///<param name="initialInteractorPosition"> is a Vector3 specifying the initial position of the interactor</param>
    private Vector3 initialInteractorPosition;
    ///<param name="initialPercent"> specifies the initinal percentage the switch has already been rotated</param>
    private int initialPercent;
    ///<param name="previousPercent"> specifies the percentage by which the switch was rotated in the last frame</param>
    private int previousPercent;
    ///<param name="initialInteractorRotation"> is a Quaternion specifying the initial rotation of the interactor upon interaction</param>
    private Quaternion initialInteractorRotation;
    ///<param name="nppClient"> is a reference to a NPPClient instance in the scene</param>
	private NPPClient nppClient;

    /// <summary>
    /// This method initializes the WP1 instance and sets the initial rotation of the switch.
    /// </summary>
    void Start()
    {
		nppClient = FindObjectOfType<NPPClient>();

        if (nppClient == null)
        {
            Debug.LogError("NPPClient instance not found in the scene.");
            return;
        }

        initialPercent = Percent;
        previousPercent = Percent;

        // Calculate the rotation angle based on Percent
        // Apply the rotation to the to_rotate object
        UpdateRotation();
    }

    /// <summary>
    /// This method updates the rotation of the switch based on the current value of Percent. Additionally a call to the REST Server is initiated via SendPercentToSimulation() to update the simulation.
    /// </summary>
    void Update()
    {
        if (isInteracting && interactor != null)
        {
            HandleRotationInteraction();
        }
        else if (Percent != previousPercent)
        {
            UpdateRotation();
            SendPercentToSimulation();
        }
        previousPercent = Percent;
        
        if(Time.frameCount % 30 == 0 && Mathf.RoundToInt(nppClient.simulation.WP1.rpm) !=  Percent * 20)
        {
            SendPercentToSimulation();
        }
    }
	
    /// <summary>
    /// This method updates the rotation of the switch.
    /// </summary>
	private void UpdateRotation()
    {
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    /// <summary>
    /// This method computes the rotation of the handle based on the rotation of the interactor and calls UpdateRotation() to update the rotation of the switch as well as SendPercentToSimulation() to intiate a call to the REST Server to update the simulation.
    /// </summary>
    private void HandleRotationInteraction()
    {

        Quaternion currentRotation = interactor.transform.rotation;
        Quaternion initialRotation = initialInteractorRotation;
        Quaternion rotationDifference = Quaternion.Inverse(initialRotation) * currentRotation;

        float angle;
        Vector3 axis;
        rotationDifference.ToAngleAxis(out angle, out axis);

        if (axis.z < 0)
        {
            angle = -angle;
        }

        Percent = Mathf.Clamp(initialPercent + (int)(angle * -0.5f), 0, 100);
        UpdateRotation();
    
        if (Time.time - lastPressTime > pressCooldown)
        {
            lastPressTime = Time.time;
            SendPercentToSimulation();
        }
    }

    /// <summary>
    /// This method initiates a call to the REST Server to update the simulation with the current RPM value of water pump 1.
    /// </summary>
    private void SendPercentToSimulation()
    {
        int rpmValue = Percent * 20; // Convert percent to RPM
        StartCoroutine(nppClient.UpdatePump("WP1", rpmValue));
    }

    /// <summary>
    /// This method sets the percentage value of the switch based on an external input.
    /// </summary>
    /// <param name="percent"> specifies the percentage value to set the switch to</param>
	public void SetPercentFromExternal(int percent)
	{
		Percent = Mathf.Clamp(percent, 0, 100);
	}

    /// <summary>
    /// This method is called when the object is enabled and adds event listeners for the selectEntered and selectExited events.
    /// </summary>
    private void OnEnable()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    /// <summary>
    /// This method is called when the object is disabled and removes event listeners for the selectEntered and selectExited events.
    /// </summary>
    private void OnDisable()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    /// <summary>
    /// This method is called when an interactor enters the object and sets the interactor and initialInteractorRotation values.
    /// </summary>
    /// <param name="args"> passes event specific arguments upon entering the interaction</param>
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isInteracting = true;
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        initialInteractorRotation = interactor.transform.rotation;
        initialPercent = Percent;
    }

    /// <summary>
    /// This method is called when an interactor exits the object and resets the isInteracting and interactor values.
    /// </summary>
    /// <param name="args">passes event specific arguments upon exiting the interaction</param>
    private void OnSelectExited(SelectExitEventArgs args)
    {
        isInteracting = false;
        interactor = null;
    }
}