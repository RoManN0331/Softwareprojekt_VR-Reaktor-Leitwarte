using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class implements the default behaviour for rotary switches.
/// </summary>
public class ReglerControl : MonoBehaviour
{
    ///<summary>Defines two types of rotary switches binary and exact</summary>
    public enum ReglerTypeEnum
    {
        Genau = 0,
        Binaer = 1
    }

    ///<param name="ReglerType"> Specifies the type of rotary switch</param>
    public ReglerTypeEnum ReglerType = ReglerTypeEnum.Genau;
    ///<param name="to_rotate">specifies the handle the player must interact with to rotate the switch</param>
    public GameObject to_rotate;
    [Range(0, 100)]
    ///<param name="Percent">int specifying the percentage the switch has been rotated based on its leftmost position</param>
    public int Percent = 0;
    ///<param name="StartRotation">int specifying the angle of the switches leftmost position</param>
    private int StartRotation = -90;
    ///<param name="EndRotation">int specifying the angle of the switches rightmost position</param>
    private int EndRotation = 90;
    ///<param name="lastPressTime">float specifying when the switch was last interacted with</param>
    private float lastPressTime = 0f;
    ///<param name="pressCooldown">float specifying a cooldown between interactions with the switch</param>
    private float pressCooldown = 1f;
    ///<param name="interactor">Interactor</param>
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    ///<param name="isInteracting">boolean tracking if the player is interacting with the switch</param>
    private bool isInteracting = false;
    ///<param name="initialInteractorPosition">Vector3 specifying the initial Position of the Interactor</param>
    private Vector3 initialInteractorPosition;
    ///<param name="initialPercent">int specifying the initinal percentage the switch has already been rotated</param>
    private int initialPercent;
    ///<param name="previousPercent">int specifying the percentage the switch has been rotated in the last frame</param>
    private int previousPercent;
    ///<param name="lightRegler">LoghtRegler to switch on lights</param>
    private LightRegler lightRegler;
    ///<param name="initialInteractorRotation">Quaternion specifying the initial rotation of the interactor upon interaction</param>
    private Quaternion initialInteractorRotation;


    /// <summary>
    /// This method initializes the switch instance and sets the initial rotation of the switch.
    /// </summary>
    void Start()
    {

        if (ReglerType == ReglerTypeEnum.Binaer)
        {
            StartRotation = -90;
            EndRotation = 0;
        }

        initialPercent = Percent;
        previousPercent = Percent;


        // Calculate the rotation angle based on Percent
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
        // Apply the rotation to the to_rotate object
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
        
        //Signal Lampe um zu signalisieren ob Ventil offen oder geschlossen ist
        initLamp();
    }

    /// <summary>
    /// This method updates the rotation of the switch based on the current percentage value and the type of the switch.
    /// </summary>
    void Update()
    {
        if (Percent != previousPercent)
        {
            if (ReglerType == ReglerTypeEnum.Binaer)
            {
                // Calculate the rotation angle based on Percent
                float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
                // Apply the rotation to the to_rotate object
                to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
                
                if (Percent == 100)
                {
                    lightRegler.SetLight(true);
                }

                else if (Percent == 0)
            
                {
                    lightRegler.SetLight(false);
                }
            }
        }
        
        if (ReglerType == ReglerTypeEnum.Genau && isInteracting && interactor != null)
        {
            // Calculate the rotation of the controller around the z-axis
            float currentZRotation = interactor.transform.eulerAngles.z;
            float initialZRotation = initialInteractorRotation.eulerAngles.z;
            float rotationDifference = Mathf.DeltaAngle(initialZRotation, currentZRotation); 
            
            // Update the Percent value based on rotation difference
            Percent = Mathf.Clamp(initialPercent + (int)(rotationDifference * -0.5f), 0, 100);

            // Calculate the rotation angle based on Percent
            float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);

            // Apply the rotation to the to_rotate object
            to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
        } else if (ReglerType == ReglerTypeEnum.Genau && Percent != previousPercent)
        {
            // Calculate the rotation angle based on Percent
            float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
            // Apply the rotation to the to_rotate object
            to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
        }

        previousPercent = Percent;
        
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
        if (ReglerType == ReglerTypeEnum.Genau)
        {
            isInteracting = true;
            interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
            initialInteractorRotation = interactor.transform.rotation;
            initialPercent = Percent;
        }
        else if (ReglerType == ReglerTypeEnum.Binaer && Time.time - lastPressTime >= pressCooldown)
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
        if (ReglerType == ReglerTypeEnum.Genau)
        {
            isInteracting = false;
            interactor = null;
        }
    }
    
    /// <summary>
    /// This method initialises the switches lamp.
    /// </summary>
    private void initLamp()
    {
        if (ReglerType == ReglerTypeEnum.Binaer)
        {
            // Find the child GameObject named "Lampe"
            Transform lampeTransform = transform.Find("Lampe");
            if (lampeTransform != null)
            {
                lightRegler = lampeTransform.GetComponent<LightRegler>();
            }
            else
            {
                Debug.LogError("Child GameObject 'Lampe' not found.");
            }
        }
    }
}