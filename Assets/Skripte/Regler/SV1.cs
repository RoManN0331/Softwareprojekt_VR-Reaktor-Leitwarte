using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class SV1: MonoBehaviour
{

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

    void Start()
    {

        to_rotate = GameObject.Find("KNOB.SV1");
        clientObject = GameObject.Find("NPPclientObject");

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

                // Calculate the rotation angle based on Percent
                float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);
                // Apply the rotation to the to_rotate object
                to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);

            if (to_rotate.transform.rotation.eulerAngles.y == 32.7f)
                    /*accounts for the orientation of the console*/
                {

                    StartCoroutine(SetValves("SV1", true));
                    Debug.Log("Valve SV1 is open");
                }
            
            else if (to_rotate.transform.rotation.eulerAngles.y == 302.7f)
                    /*accounts for the orientation of the console*/
                {
                    StartCoroutine(SetValves("SV1", false));
                    Debug.Log("Valve SV1 is closed");
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
        
        if (Time.time - lastPressTime >= pressCooldown)
        {
            // Toggle the Percent value between 0 and 100
            Percent = Percent == 0 ? 100 : 0;
            lastPressTime = Time.time; // Update the last press time
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
    
    }


    IEnumerator SetValves(string ValveID, bool value){


    UnityWebRequest req = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}control/valve/{ValveID}?activate={value}", "");

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