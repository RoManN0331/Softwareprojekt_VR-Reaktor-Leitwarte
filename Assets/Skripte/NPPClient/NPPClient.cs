using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class NPPClient : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();
    
    public NPPReactorState simulation = new NPPReactorState();
    private AnimatorController animatorController;

    void Awake()
    {
        animatorController = GetComponent<AnimatorController>();
    }

    void Start()
    {
        StartCoroutine(UpdateSimulationState());
    }

    private async Task FetchReactorState()
    {
        string response = await GetJsonAsync($"{GlobalConfig.BASE_URL}simulation/reactor");
        simulation.Reactor = JsonConvert.DeserializeObject<ReactorState>(response);
        //Debug.Log("Reactor State: " + response);
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
    
            //Debug.Log("All states updated at: " + DateTime.Now);
        
            // Wait for interval
            yield return new WaitForSeconds(GlobalConfig.CLIENT_UPDATE_INTERVAL);
        }
    }
}
