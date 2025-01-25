using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class WV2: MonoBehaviour
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
	
	private NPPClient nppClient;

    private const string BASE_URL = "http://localhost:8080/api/";

    void Start()
    {

        to_rotate = GameObject.Find("KNOB.WV2");
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


    }

    void Update()
    {

        if (Percent != previousPercent)
        {

            UpdateRotation();

            if (Percent == 100)
            {
                SetValveStatus("WV2", true);
                Debug.Log("Valve WV2 is open");
            }

            else if (Percent == 0)
                    /*
                      percent kommt mit jeder rotation klar 
                      alternativer wäre noch to_rotate.transform.localrotation.eulerAngles.y == StartRoation
                      also die LOKALE rotation ;)   * /
                     */            
            {
                SetValveStatus("WV2", false);
                Debug.Log("Valve WV2 is closed");
            }
        }

        previousPercent = Percent;
                
    }
	
	private void UpdateRotation()
    {
        // Calculate the rotation angle based on Percent
        float angle = Mathf.Lerp(StartRotation, EndRotation, Percent / 100f);

        // Apply the rotation to the to_rotate object
        to_rotate.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }
	
	IEnumerator SetValveStatus(string valveId, bool value)
    {
        if (nppClient != null)
        {
            yield return StartCoroutine(nppClient.UpdateValveStatus(valveId, value));
        }
        else
        {
            Debug.LogError("NPPClient is not initialized.");
        }
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

}