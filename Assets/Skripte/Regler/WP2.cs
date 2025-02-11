using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class WP2 : MonoBehaviour
{

    /// <summary>
    /// This class is used to control water pump 2 in the NPP simulation.
    /// </summary>

    ///<param name="ReglerType"> Specifies the type of rotary switch</param>
    ///<param name="to_rotate">specifies the handle the player must interact with to rotate the switch</param>
    ///<param name="Percent">int specifying the percentage the switch has been rotated based on its leftmost position</param>
    ///<param name="StartRotation">int specifying the angle of the switches leftmost position</param>
    ///<param name="EndRotation">int specifying the angle of the switches rightmost position</param>
    ///<param name="lastPressTime">float specifying when the switch was last interacted with</param>
    ///<param name="pressCooldown">float specifying a cooldown between interactions with the switch</param>
    ///<param name="interactor">Interactor</param>
    ///<param name="isInteracting">boolean tracking if the player is interacting with the switch</param>
    ///<param name="initialInteractorPosition">Vector3 specifying the initial Position of the Interactor</param>
    ///<param name="initialPercent">int specifying the initinal percentage the switch has already been rotated</param>
    ///<param name="previousPercent">int specifying the percentage the switch has been rotated in the last frame</param>
    ///<param name="initialInteractorRotation">Quaternion specifying the initial rotation of the interactor upon interaction</param>
    ///<param name="nppClient">Reference to the NPPClient instance in the scene</param>


    private enum ReglerTypeEnum
    {
        Genau = 0,
        Binaer = 1
    }

    private ReglerTypeEnum ReglerType = ReglerTypeEnum.Genau;

    public GameObject to_rotate;

    [Range(0, 100)]
    public int Percent = 0;

    private int StartRotation = -90;
    private int EndRotation = 90;

    private float lastPressTime = 0f;
    private float pressCooldown = 0.1f; // 1 second cooldown

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    private bool isInteracting = false;
    private Vector3 initialInteractorPosition;
    private int initialPercent;
    private int previousPercent;
	private Quaternion initialInteractorRotation;
	
	private NPPClient nppClient;

    /// <summary>
    /// This method initializes the WP2 instance and sets the initial rotation of the switch.
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
/// This method updates the rotation of the switch based on the current percentage value. Additionally a call to the REST Server is initiated via SendPercentToSimulation() to update the simulation.
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

        if(Time.frameCount % 30 == 0 &&  Mathf.RoundToInt(nppClient.simulation.WP2.rpm) !=  Percent * 20)
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
/// This method initiates a call to the REST Server to update the simulation with the current RPM value of water pump 2.
/// </summary>

    private void SendPercentToSimulation()
    {
        int rpmValue = Percent * 20; // Convert percent to RPM
        StartCoroutine(nppClient.UpdatePump("WP2", rpmValue));
    }

/// <summary>
/// This method sets the percentage value of the switch based on an external input.
/// </summary>
/// <param name="percent">int specifying the percentage value to set the switch to</param>

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
/// <param name="args">SelectEnterEventArgs to pass event specific arguments upon entering the interaction</param>

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
/// <param name="args">SelectExitEventArgs to pass event specific arguments upon exiting the interaction</param>


    private void OnSelectExited(SelectExitEventArgs args)
    {
       
        isInteracting = false;
        interactor = null;
    }


}