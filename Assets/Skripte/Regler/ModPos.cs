using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class ModPos : MonoBehaviour
{

    /// <summary>
    /// This class is used to control the control rods in the NPP simulation.
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
    private float pressCooldown = 1f; // 1 second cooldown

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    private bool isInteracting = false;
    private Vector3 initialInteractorPosition;
    private int initialPercent;
    private int previousPercent;
	private Quaternion initialInteractorRotation;
	private NPPClient nppClient;

    /// <summary>
    /// This method initializes the ModPos instance and sets the initial rotation of the switch.
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
/// This method updates the rotation of the switch based on the current percentage value. Additionally a call to the REST Server is initiated via UpdateRodPosition() to update the simulation.
/// </summary>

    void Update()
    {

        if (isInteracting && interactor != null)
        {
            HandleInteractorRotation();
        }
        else if (Percent != previousPercent)
        {
            UpdateRotation();
            UpdateRodPosition();
        }

        previousPercent = Percent;
        
        if(Time.frameCount % 30 == 0 && Mathf.RoundToInt(nppClient.simulation.Reactor.rodPosition) != 100 - Percent)
        {
            UpdateRodPosition();
        }

    }

/// <summary>
/// This method updates the rotation of the switch.
/// </summary>

    private void UpdateRotation()
    {
        // Calculate the rotation angle based on Percent
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);

        // Apply the rotation to the to_rotate object
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

/// <summary>
/// This method computes the rotation of the handle based on the rotation of the interactor and calls UpdateRotation() to update the rotation of the switch as well as UpdateRodPosition() to intiate a call to the REST Server to update the simulation.
/// </summary>

	private void HandleInteractorRotation()
    {
        // Calculate the rotation of the controller around the z-axis
        float currentZRotation = interactor.transform.eulerAngles.z;
        float initialZRotation = initialInteractorRotation.eulerAngles.z;
        float rotationDifference = Mathf.DeltaAngle(initialZRotation, currentZRotation);

        // Update the Percent value based on rotation difference
        Percent = Mathf.Clamp(initialPercent + (int)(rotationDifference * -0.35f), 0, 100);

        UpdateRotation();
        UpdateRodPosition();
    }

/// <summary>
/// This method initiates a call to the REST Server to update the simulation with the current position of the control rods.
/// </summary>

	private void UpdateRodPosition()
    {
		
        if (Time.time - lastPressTime > pressCooldown)
        {
            lastPressTime = Time.time;

            int rodValue = 100 - Percent; 

            StartCoroutine(nppClient.SetRodPosition(rodValue));
        }
    }

/// <summary>
/// This method sets the percentage value of the switch based on an external input.
/// </summary>
/// <param name="newPercent">int specifying the percentage value to set the switch to</param>

	public void SetPercentFromExternal(int newPercent)
    {
        Percent = Mathf.Clamp(newPercent, 0, 100);
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