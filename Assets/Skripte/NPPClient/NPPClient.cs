using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class NPPClient : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    public NPPReactorState simulation = new NPPReactorState();
    private AnimatorController animatorController;
	// state machine
	private int scenario = 0;
	private Boolean loadscenario = false;

    private AusfallAnzeigenManager ausfallAnzeigenManager;
    void Awake()
    {
        animatorController = GetComponent<AnimatorController>();
    }

    void Start()
    {
        StartCoroutine(UpdateSimulationState());
        
        ausfallAnzeigenManager = FindObjectOfType<AusfallAnzeigenManager>();
    }

    private async Task FetchReactorState()
    {
        string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/reactor");
        simulation.Reactor = JsonConvert.DeserializeObject<ReactorState>(response);
        // Debug.Log("Reactor State: " + response);
		
    }

    private async Task FetchCondenserState()
    {
        string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/condenser");
        simulation.Condenser = JsonConvert.DeserializeObject<CondenserState>(response);
        //Debug.Log("Condenser State: " + response);
    }

    private async Task FetchGeneratorState()
    {
        string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/generator");
        simulation.Generator = JsonConvert.DeserializeObject<GeneratorState>(response);
        //Debug.Log("Generator State: " + response);
    }

    private async Task FetchComponentHealth()
    {
        string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/health");
        ComponentState[] components = JsonConvert.DeserializeObject<ComponentState[]>(response);
        simulation.ComponentHealth = new ComponentHealth { components = components };
        //Debug.Log("Component Health: " + response);
    }


    private async Task FetchPumps()
    {
        string[] pumpIds = { "WP1", "WP2", "CP" };
        foreach (string pumpId in pumpIds)
        {
            string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/pump/{pumpId}");
            PumpState pumpState = JsonConvert.DeserializeObject<PumpState>(response);

            switch (pumpId)
            {
                case "WP1":
                    simulation.WP1 = pumpState;
                    break;
                case "WP2":
                    simulation.WP2 = pumpState;
                    break;
                case "CP":
                    simulation.CP = pumpState;
                    break;
            }

            //Debug.Log($"Pump {pumpId} State: {response}");
        }
    }

    private async Task FetchValves()
    {
        string[] valveIds = { "SV1", "SV2", "WV1", "WV2" };
        foreach (string valveId in valveIds)
        {
            string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/valve/{valveId}");
            ValveState valveState = JsonConvert.DeserializeObject<ValveState>(response);

            switch (valveId)
            {
                case "SV1":
                    simulation.SV1 = valveState;
                    break;
                case "SV2":
                    simulation.SV2 = valveState;
                    break;
                case "WV1":
                    simulation.WV1 = valveState;
                    break;
                case "WV2":
                    simulation.WV2 = valveState;
                    break;
            }

            //Debug.Log($"Valve {valveId} State: {response}");
        }
    }

    private async Task FetchSystemStatus()
    {
        string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}system/status");
        simulation.SystemStatus = JsonConvert.DeserializeObject<SystemStatus>(response);
        //Debug.Log("System Status: " + response);
    }

    private async Task<string> GetJsonAsync(string url)
    {
        try {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        } catch (Exception e) {
            //Debug.LogError("Error fetching data: " + e.Message);
        }
        return null;        
    }
	
	public IEnumerator UpdatePump(string id, int value)
    {
        using (UnityWebRequest req = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}control/pump/{id}?setRpm={value}", ""))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error updating pump: {req.error}");
            }
            else
            {
                Debug.Log($"Pump updated successfully: {req.downloadHandler.text}");
            }
        }
    }
	
	public IEnumerator UpdateValveStatus(string valveId, bool value)
    {
        using (UnityWebRequest request = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}control/valve/{valveId}?activate={value}", ""))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error updating valve status: {request.error}");
            }
            else
            {
                Debug.Log($"Valve status updated successfully: {request.downloadHandler.text}");
            }
        }
    }
	
	public IEnumerator SetRodPosition(int value)
	{
		using (UnityWebRequest req = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}control/rods?setRod={value}", ""))
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
	
	public IEnumerator SetNormalShutdownScenario()
	{
		Debug.Log("Setting Normal Shutdown Scenario via API...");
		
		ModPos modPos = FindObjectOfType<ModPos>();
		
		if(modPos != null){
			modPos.SetPercentFromExternal(20);
		}
		
		WP1 wp1 = FindObjectOfType<WP1>();
		if (wp1 != null)
		{
			wp1.SetPercentFromExternal(75);
		}
		
		WP2 wp2 = FindObjectOfType<WP2>();
		if (wp2 != null)
		{
			wp2.SetPercentFromExternal(0);
		}
		
		CP cp = FindObjectOfType<CP>();
		if (cp != null)
		{
			cp.SetPercentFromExternal(90);
		}
		
		SV1 sv1 = FindObjectOfType<SV1>();
		if (sv1 != null)
		{
			sv1.SetPercentFromExternal(0); 
		}
		
		SV2 sv2 = FindObjectOfType<SV2>();
		if (sv2 != null)
		{
			sv2.SetPercentFromExternal(0); 
		}

		WV1 wv1 = FindObjectOfType<WV1>();
		if (wv1 != null)
		{
			wv1.SetPercentFromExternal(100); 
		}
		
		WV2 wv2 = FindObjectOfType<WV2>();
		if (wv2 != null)
		{
			wv2.SetPercentFromExternal(0); 
		}
		
		using (UnityWebRequest req = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}system/normalShutdown",""))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting Normal Shutdown Scenario: {req.error}");
            }
            else
            {
                Debug.Log($"Normal Shutdown Scenario set successfully: {req.downloadHandler.text}");
            }
        }

		
		// API-Befehle für das Szenario ausführen
		/* yield return StartCoroutine(SetRodPosition(80)); // Control Rods auf 80%
		yield return StartCoroutine(UpdatePump("WP1", 1500)); // WP1 auf 1500 RPM
		yield return StartCoroutine(UpdatePump("WP2", 0)); // WP2 auf 0 RPM
		yield return StartCoroutine(UpdatePump("CP", 1800)); // CP auf 1800 RPM

		yield return StartCoroutine(UpdateValveStatus("SV1", false)); // SV1 schließen
		yield return StartCoroutine(UpdateValveStatus("SV2", false)); // SV2 schließen
		yield return StartCoroutine(UpdateValveStatus("WV1", true)); // WV1 öffnen
		yield return StartCoroutine(UpdateValveStatus("WV2", false)); // WV2 schließen */

		// state machine
		if (!loadscenario){
			scenario = 2;
			loadscenario = true;
		}

		Debug.Log("Normal Shutdown Scenario applied successfully.");
	}
	
	public IEnumerator SetEmergencyShutdownScenario()
	{
		Debug.Log("Setting Emergency Shutdown Scenario via API...");
		
		ModPos modPos = FindObjectOfType<ModPos>();
		
		if(modPos != null){
			modPos.SetPercentFromExternal(20);
		}
		
		WP1 wp1 = FindObjectOfType<WP1>();
		if (wp1 != null)
		{
			wp1.SetPercentFromExternal(0);
		}
		
		WP2 wp2 = FindObjectOfType<WP2>();
		if (wp2 != null)
		{
			wp2.SetPercentFromExternal(0);
		}
		
		CP cp = FindObjectOfType<CP>();
		if (cp != null)
		{
			cp.SetPercentFromExternal(90);
		}
		
		SV1 sv1 = FindObjectOfType<SV1>();
		if (sv1 != null)
		{
			sv1.SetPercentFromExternal(0); 
		}
		
		SV2 sv2 = FindObjectOfType<SV2>();
		if (sv2 != null)
		{
			sv2.SetPercentFromExternal(0); 
		}

		WV1 wv1 = FindObjectOfType<WV1>();
		if (wv1 != null)
		{
			wv1.SetPercentFromExternal(100); 
		}
		
		WV2 wv2 = FindObjectOfType<WV2>();
		if (wv2 != null)
		{
			wv2.SetPercentFromExternal(0); 
		}
		
		using (UnityWebRequest req = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}system/emergencyShutdown",""))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting Emergency Shutdown Scenario: {req.error}");
            }
            else
            {
                Debug.Log($"Emergency Shutdown Scenario set successfully: {req.downloadHandler.text}");
            }
        }

		
		// API-Befehle für das Szenario ausführen
		/* yield return StartCoroutine(SetRodPosition(80)); // Control Rods auf 80%
		yield return StartCoroutine(UpdatePump("WP1", 1500)); // WP1 auf 1500 RPM
		yield return StartCoroutine(UpdatePump("WP2", 0)); // WP2 auf 0 RPM
		yield return StartCoroutine(UpdatePump("CP", 1800)); // CP auf 1800 RPM

		yield return StartCoroutine(UpdateValveStatus("SV1", false)); // SV1 schließen
		yield return StartCoroutine(UpdateValveStatus("SV2", false)); // SV2 schließen
		yield return StartCoroutine(UpdateValveStatus("WV1", true)); // WV1 öffnen
		yield return StartCoroutine(UpdateValveStatus("WV2", false)); // WV2 schließen */

		// state machine
		if (!loadscenario){
			scenario = 2;
			loadscenario = true;
		}

		//Debug.Log("Emergency Shutdown Scenario applied successfully.");
	}
	
	public IEnumerator SetInitialStateScenario()
	{
		//Debug.Log("Setting Initial State Scenario via API...");
		
		ModPos modPos = FindObjectOfType<ModPos>();
		
		if(modPos != null){
			modPos.SetPercentFromExternal(0);
		}
		
		WP1 wp1 = FindObjectOfType<WP1>();
		if (wp1 != null)
		{
			wp1.SetPercentFromExternal(0);
		}
		
		WP2 wp2 = FindObjectOfType<WP2>();
		if (wp2 != null)
		{
			wp2.SetPercentFromExternal(0);
		}
		
		CP cp = FindObjectOfType<CP>();
		if (cp != null)
		{
			cp.SetPercentFromExternal(0);
		}
		
		SV1 sv1 = FindObjectOfType<SV1>();
		if (sv1 != null)
		{
			sv1.SetPercentFromExternal(0); 
		}
		
		SV2 sv2 = FindObjectOfType<SV2>();
		if (sv2 != null)
		{
			sv2.SetPercentFromExternal(0); 
		}

		WV1 wv1 = FindObjectOfType<WV1>();
		if (wv1 != null)
		{
			wv1.SetPercentFromExternal(0); 
		}
		
		WV2 wv2 = FindObjectOfType<WV2>();
		if (wv2 != null)
		{
			wv2.SetPercentFromExternal(0); 
		}
		
		using (UnityWebRequest req = UnityWebRequest.Put($"{GlobalConfig.BASE_URL}system/initialState",""))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting Initial State Scenario: {req.error}");
            }
            else
            {
                //Debug.Log($"Initial State Scenario set successfully: {req.downloadHandler.text}");
            }
        }

		
		// API-Befehle für das Szenario ausführen
		/* yield return StartCoroutine(SetRodPosition(80)); // Control Rods auf 80%
		yield return StartCoroutine(UpdatePump("WP1", 1500)); // WP1 auf 1500 RPM
		yield return StartCoroutine(UpdatePump("WP2", 0)); // WP2 auf 0 RPM
		yield return StartCoroutine(UpdatePump("CP", 1800)); // CP auf 1800 RPM

		yield return StartCoroutine(UpdateValveStatus("SV1", false)); // SV1 schließen
		yield return StartCoroutine(UpdateValveStatus("SV2", false)); // SV2 schließen
		yield return StartCoroutine(UpdateValveStatus("WV1", true)); // WV1 öffnen
		yield return StartCoroutine(UpdateValveStatus("WV2", false)); // WV2 schließen */

		// state machine
		if (!loadscenario) {
			scenario = 1;
			loadscenario = true;
		}

		//Debug.Log("Initial State Scenario applied successfully.");
	}

    private IEnumerator UpdateSimulationState()
    {
        while (true)
        {
            // Fetch all data
            Task[] tasks = new Task[]
            {
                FetchReactorState(),
                FetchCondenserState(),
                FetchGeneratorState(),
                FetchComponentHealth(),
                FetchPumps(),
                FetchValves(),
                FetchSystemStatus()
            };

            // Wait for all tasks to complete
            yield return new WaitUntil(() => Task.WhenAll(tasks).IsCompleted);

            // Update animator with new state
            animatorController.UpdateAnimatorParameters(simulation);

			// update animator with new scenario

			if (loadscenario) {
				animatorController.updateScenario(scenario);
				loadscenario = !loadscenario;
				scenario = 0;
			}
    
            //Debug.Log("All states updated at: " + DateTime.Now);
        
            // Wait for interval
            yield return new WaitForSeconds(GlobalConfig.CLIENT_UPDATE_INTERVAL);
        }
    }
    
    private void Update()
    {
	    if (Time.frameCount % 60 == 0)
	    {
		    if(simulation.ComponentHealth.components[2].broken) ausfallAnzeigenManager.TurnOn("WP1");
		    if(simulation.ComponentHealth.components[3].broken) ausfallAnzeigenManager.TurnOn("WP2");
		    if(simulation.ComponentHealth.components[7].broken) ausfallAnzeigenManager.TurnOn("CP");
		    if(simulation.ComponentHealth.components[4].broken) ausfallAnzeigenManager.TurnOn("RKS");
		    if(simulation.ComponentHealth.components[5].broken) ausfallAnzeigenManager.TurnOn("RKT");
		    if(simulation.ComponentHealth.components[10].broken) ausfallAnzeigenManager.TurnOn("KNT");
		    if(simulation.ComponentHealth.components[11].broken) ausfallAnzeigenManager.TurnOn("AU");
		    if(simulation.ComponentHealth.components[6].broken) ausfallAnzeigenManager.TurnOn("TBN");
		    if (simulation.Reactor.overheated) ausfallAnzeigenManager.TurnAllOn();
	    }
    }
}
