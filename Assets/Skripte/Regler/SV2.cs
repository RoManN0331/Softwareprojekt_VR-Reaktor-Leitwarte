using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class SV2: MonoBehaviour
{

    /// <summary>
    /// This class is used to control steam valve 2 in the NPP simulation.
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
    ///<param name="lightRegler">LoghtRegler to switch on lights</param>
    ///<param name="nppClient">Reference to the NPPClient instance in the scene</param>

    public enum ReglerTypeEnum
    {
        Genau = 0,
        Binaer = 1
    }
    public ReglerTypeEnum ReglerType = ReglerTypeEnum.Binaer;

    GameObject to_rotate;

    public int Percent = 0;

    private GameObject clientObject;

    private int StartRotation = -90;
    private int EndRotation = 0;
    private float lastPressTime = 0f;
    private float pressCooldown = 1f; // 1 second cooldown

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    //private bool isInteracting = false;
    private Vector3 initialInteractorPosition;
    private int initialPercent;
    private int previousPercent;
    private LightRegler lightRegler;
	private NPPClient nppClient;

/// <summary>
/// This method initializes the SV2 instance, sets the initial rotation of the switch and initializes the switches lamp.
/// </summary>

    void Start()
    {

        to_rotate = GameObject.Find("KNOB.SV2");
        clientObject = GameObject.Find("NPPclientObject");
		
		nppClient = FindObjectOfType<NPPClient>();

        if (nppClient == null)
        {
            Debug.LogError("NPPClient instance not found in the scene.");
            return;
        }

        initialPercent = Percent;
        previousPercent = Percent;

        UpdateRotation();

        //Signal Lampe um zu signalisieren ob Ventil offen oder geschlossen ist
        initLamp();
    }

/// <summary>
/// This method updates the rotation of the switch based on the current percentage value. Additionally a call to the REST Server is initiated via SetValveStatus() to update the simulation.
/// </summary>

    void Update()
    {

        if (Percent != previousPercent)
        {

            UpdateRotation();

            if (Percent == 100)
            {
                
                SetValveStatus("SV2", true);
                // Debug.Log("Valve SV2 is open");
                
                lightRegler.SetLight(true);
            }
            else if (Percent == 0)
            {
                SetValveStatus("SV2", false);
                // Debug.Log("Valve SV2 is closed");
                
                lightRegler.SetLight(false);
            }
        }

        previousPercent = Percent;
                
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
/// This method initiates a call to the REST Server to update the simulation with the current status of steam valve 2.
/// </summary>
/// <param name="valveId">string containing the Id of the valve as specified on the REST Server </param>
/// <param name="value"> boolean the valve will be set to</param>


	public void SetValveStatus(string valveId, bool value)
    {
        if (nppClient != null)
        {
            StartCoroutine(nppClient.UpdateValveStatus(valveId, value));
        }
        else
        {
            Debug.LogError("NPPClient is not initialized.");
        }
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
        
        if (Time.time - lastPressTime >= pressCooldown)
        {
            // Toggle the Percent value between 0 and 100
            Percent = Percent == 0 ? 100 : 0;
            lastPressTime = Time.time; // Update the last press time
        }
    }

/// <summary>
/// This method is called when an interactor exits the object and resets the isInteracting and interactor values.
/// </summary>
/// <param name="args">SelectExitEventArgs to pass event specific arguments upon exiting the interaction</param>

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // pass
    }

/// <summary>
/// This method initialises the switches lamp.
/// </summary>

    private void initLamp()
    {
        // Find the child GameObject named "Lampe"
        Transform lampeTransform = transform.Find("Lampe");
        if (lampeTransform != null)
        {
            // Get the LightRegler component from the child GameObject
            lightRegler = lampeTransform.GetComponent<LightRegler>();
        }
        else
        {
            Debug.LogError("Child GameObject 'Lampe' not found.");
        }
    }
}