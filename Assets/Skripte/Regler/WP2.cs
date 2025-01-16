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
    int WP2Rpm;


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

        if (ReglerType == ReglerTypeEnum.Genau && isInteracting && interactor != null)
        {
            // Calculate the horizontal movement of the controller
            Vector3 currentInteractorPosition = interactor.transform.position;
            float horizontalMovement = (currentInteractorPosition.x - initialInteractorPosition.x) * -2;

            // Update the Percent value based on horizontal movement
            Percent = Mathf.Clamp(initialPercent + (int)(horizontalMovement * 100), 0, 100);

            // Calculate the rotation angle based on Percent
            float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);

            // Apply the rotation to the to_rotate objectS
            to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);

            if (Time.time - lastPressTime > pressCooldown)
            {

                /* converting angles to Rpm.
                there must be a better way for sure...*/

                lastPressTime = Time.time;
                if (angle < 0)
                {
                    WP2Rpm = (int) ((90 - Mathf.Abs(angle)) * 11.1111f);
                }
                else
                {
                    WP2Rpm = (int) (angle * 11.1111f + 999f);
                }
                StartCoroutine(UpdatePump("WP2", WP2Rpm));
            }

        } else if (ReglerType == ReglerTypeEnum.Genau && Percent != previousPercent)
        {
            // Calculate the rotation angle based on Percent
            float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
            // Apply the rotation to the to_rotate object
            to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);

            if (Time.time - lastPressTime > pressCooldown)
            {

                /* converting angles to Rpm.
                there must be a better way for sure...*/

                lastPressTime = Time.time;
                if (angle < 0)
                {
                    WP2Rpm = (int) ((90 - Mathf.Abs(angle)) * 11.1111f);
                
                }
                else
                {
                    WP2Rpm = (int) (angle * 11.1111f + 999);
                }
                StartCoroutine(UpdatePump("WP2", WP2Rpm));
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

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
            isInteracting = true;
            interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
            initialInteractorPosition = interactor.transform.position;
            initialPercent = Percent;
        
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (ReglerType == ReglerTypeEnum.Genau)
        {
            isInteracting = false;
            interactor = null;
        }
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