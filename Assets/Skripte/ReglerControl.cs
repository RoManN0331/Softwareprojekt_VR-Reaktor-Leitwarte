using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReglerControl : MonoBehaviour
{
    public enum ReglerTypeEnum
    {
        Genau = 0,
        Binaer = 1
    }

    public ReglerTypeEnum ReglerType = ReglerTypeEnum.Genau;

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

    void Start()
    {
        //to_rotate = GameObject.Find("KNOB.005");

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

    private void OnEnable()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    private Quaternion initialInteractorRotation;
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

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (ReglerType == ReglerTypeEnum.Genau)
        {
            isInteracting = false;
            interactor = null;
        }
    }
    
    private LightRegler lightRegler;
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