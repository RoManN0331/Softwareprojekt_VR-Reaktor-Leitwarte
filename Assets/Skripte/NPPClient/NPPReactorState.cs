using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class NPPReactorState{

    public SystemStatus SystemStatus;
    public ReactorState Reactor;
    public ValveState SV1, SV2, WV1, WV2;
    public PumpState WP1, WP2, CP;
    public CondenserState Condenser;
    public GeneratorState Generator;
    public ComponentHealth ComponentHealth;
}

//simulation endpofloat classes
[Serializable]
public class CondenserState {
    public float pressure;
    public float waterLevel;
    public bool operational;
}

[Serializable]
public class GeneratorState {
    public string name;
    public bool blown;    
    public float power;
}

[Serializable]
public class ReactorState {
    public float pressure;
    public float waterLevel;
    public bool operational;
    public bool intactact;
    public float rodPosition;
    public bool overheated;
    public int restheat;
}

[Serializable]
public class PumpState {
    public string name;
    public bool blown;
    public float rpm;
    public float setRpm;
    public float maxRpm;
    public bool operational;
}

[Serializable]
public class ValveState {
    public string name;
    public bool blown;
    public bool status;
} 

[JsonObject]
public class ComponentHealth {
    public ComponentState[] components;
}

[JsonObject]
public class ComponentState {
    [JsonProperty("name")]
    public string name;
    [JsonProperty("broken")]
    public bool broken;

    [JsonProperty("status")]
    public bool status = true;
}

//system endpofloat classes
[Serializable]
public class SystemStatus {
    public string runningSince;
    public bool running;

    public DateTime getDate() {
        return DateTime.Parse(runningSince);
    }

}

[Serializable]
public class ErrorState {
    public string error;
    public string message;
}