using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class WP2 : MonoBehaviour
{

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


    private const string BASE_URL = "http://localhost:8443/api/";

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

    void Update()
    {

        if (ReglerType == ReglerTypeEnum.Genau && isInteracting && interactor != null)
        {
            HandleRotationInteraction();
        }
        else if (Percent != previousPercent)
        {
            UpdateRotation();
            SendPercentToSimulation();
        }

        previousPercent = Percent;

    }
	
	private void UpdateRotation()
    {
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    private void HandleRotationInteraction()
    {
        float currentZRotation = interactor.transform.eulerAngles.z;
        float initialZRotation = initialInteractorRotation.eulerAngles.z;
        float rotationDifference = Mathf.DeltaAngle(initialZRotation, currentZRotation);

        Percent = Mathf.Clamp(initialPercent + (int)(rotationDifference * -0.5f), 0, 100);
        UpdateRotation();

        if (Time.time - lastPressTime > pressCooldown)
        {
            lastPressTime = Time.time;
            SendPercentToSimulation();
        }
    }

    private void SendPercentToSimulation()
    {
        if (ReglerType == ReglerTypeEnum.Binaer)
        {
            Percent = Percent == 0 ? 100 : 0;
        }

        int rpmValue = Percent * 20; // Convert percent to RPM
        StartCoroutine(nppClient.UpdatePump("WP2", rpmValue));
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


}