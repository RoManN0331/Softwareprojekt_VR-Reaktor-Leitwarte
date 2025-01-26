using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class CP : MonoBehaviour
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
    private float pressCooldown = 0.1f; // 1 second cooldown

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    private bool isInteracting = false;
    private Vector3 initialInteractorPosition;
    private int initialPercent;
    private int previousPercent;

    private const string BASE_URL = "http://localhost:8443/api/";

    void Start()
    {

        initialPercent = Percent;
        previousPercent = Percent;

        // Calculate the rotation angle based on Percent
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
        // Apply the rotation to the to_rotate object
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
        
    }

    void Update()
    {

        if (isInteracting && interactor != null)
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

            if (Time.time - lastPressTime > pressCooldown)
            {

                lastPressTime = Time.time;
                StartCoroutine(UpdatePump("CP", (int)(Percent*20)));
            }

        } else if (Percent != previousPercent)
        {
            // Calculate the rotation angle based on Percent
            float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
            // Apply the rotation to the to_rotate object
            to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);

            if (Time.time - lastPressTime > pressCooldown)
            {

                lastPressTime = Time.time;
                StartCoroutine(UpdatePump("CP", (int)(Percent*20)));
            }
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

    public IEnumerator UpdatePump(string id, int value)
    {

        using (UnityWebRequest req = UnityWebRequest.Put($"{BASE_URL}control/pump/{id}?setRpm={value}", ""))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Request Error: {req.error}");
            }
            else
            {
                Debug.Log($"Request Successful: {req.downloadHandler.text}");
            }
        }
    }


}