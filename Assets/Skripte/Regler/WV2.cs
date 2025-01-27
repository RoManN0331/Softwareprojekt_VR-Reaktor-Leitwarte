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

            if (to_rotate.transform.rotation.eulerAngles.y == EndRotation)
                     /*accounts for the orientation of the console*/
            {
                SetValveStatus("WV2", true);
                Debug.Log("Valve WV2 is open");
            }

            else if (to_rotate.transform.rotation.eulerAngles.y == 270)
                    /*accounts for the orientation of the console*/            
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
	
	public void SetPercentFromExternal(int percent)
	{
		Percent = Mathf.Clamp(percent, 0, 100); 
		UpdateRotation(); 

		if (to_rotate.transform.rotation.eulerAngles.y == EndRotation)
		{
			SetValveStatus("WV2", true);
			Debug.Log("Valve WV2 is open");
		}
		else if (to_rotate.transform.rotation.eulerAngles.y == 270)
		{
			SetValveStatus("WV2", false);
			Debug.Log("Valve WV2 is closed");
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