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

        //Signal Lampe um zu signalisieren ob Ventil offen oder geschlossen ist
        initLamp();
    }

    void Update()
    {

        if (Percent != previousPercent)
        {

            UpdateRotation();

            if (Percent == 100)
            {
                StartCoroutine(SetValves("WV2", true));
                Debug.Log("Valve WV2 is open");
                
                lightRegler.SetLight(true);
            }

            else if (Percent == 0)
                    /*
                      percent kommt mit jeder rotation klar 
                      alternativer wäre noch to_rotate.transform.localrotation.eulerAngles.y == StartRoation
                      also die LOKALE rotation ;)   * /
                     */            
            {
                StartCoroutine(SetValves("WV2", false));
                Debug.Log("Valve WV2 is closed");
                
                lightRegler.SetLight(false);
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