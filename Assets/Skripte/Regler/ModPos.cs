using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class ModPos : MonoBehaviour
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
    int CPRpm;
	
	private NPPClient nppClient;

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

    }
	
	private void HandleInteractorRotation()
    {
        // Calculate the rotation of the controller around the z-axis
        float currentZRotation = interactor.transform.eulerAngles.z;
        float initialZRotation = initialInteractorRotation.eulerAngles.z;
        float rotationDifference = Mathf.DeltaAngle(initialZRotation, currentZRotation);

        // Update the Percent value based on rotation difference
        Percent = Mathf.Clamp(initialPercent + (int)(rotationDifference * -0.5f), 0, 100);

        UpdateRotation();
        UpdateRodPosition();
    }

    private void UpdateRotation()
    {
        // Calculate the rotation angle based on Percent
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);

        // Apply the rotation to the to_rotate object
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }
	
	private void UpdateRodPosition()
    {
		
        if (Time.time - lastPressTime > pressCooldown)
        {
            lastPressTime = Time.time;

            int rodValue = 100 - Percent; 
            Debug.Log($"Setting rod position to {rodValue}");

            StartCoroutine(nppClient.SetRodPosition(rodValue));
        }
    }
	
	public void SetPercentFromExternal(int newPercent)
    {
        Percent = Mathf.Clamp(newPercent, 0, 100);
        UpdateRotation();
        UpdateRodPosition();
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
        isInteracting = true;
        interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        initialInteractorRotation = interactor.transform.rotation;
        initialPercent = Percent;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isInteracting = false;
        interactor = null;
    }
}