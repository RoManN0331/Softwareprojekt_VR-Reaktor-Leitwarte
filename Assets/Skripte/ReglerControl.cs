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
            }
        }
        
        if (ReglerType == ReglerTypeEnum.Genau && isInteracting && interactor != null)
        {
            // Calculate the horizontal movement of the controller
            Vector3 currentInteractorPosition = interactor.transform.position;
            float horizontalMovement = (currentInteractorPosition.x - initialInteractorPosition.x) * -2;

            // Update the Percent value based on horizontal movement
            Percent = Mathf.Clamp(initialPercent + (int)(horizontalMovement * 100), 0, 100);

            // Calculate the rotation angle based on Percent
            float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);

            // Apply the rotation to the to_rotate object
            to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
            
        }else if (ReglerType == ReglerTypeEnum.Genau && Percent != previousPercent)
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

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (ReglerType == ReglerTypeEnum.Genau)
        {
            isInteracting = true;
            interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
            initialInteractorPosition = interactor.transform.position;
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
}