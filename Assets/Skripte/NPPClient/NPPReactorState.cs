using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
/// <summary>
/// This class is used to store the state of the simulation.
/// </summary>
public class NPPReactorState{

    /// <param name="SystemStatus"> referes to the system status</param>
    public SystemStatus SystemStatus;
    /// <param name="Reactor"> refers to the reactor component</param>
    public ReactorState Reactor;
    /// <param name="SV1"> refers to the steam valve 1 component</param>
    public ValveState SV1;
    /// <param name="SV2"> refers to the steam valve 2 component</param>
    public ValveState SV2;
    /// <param name="WV1"> refers to the water valve 1 component</param>
    public ValveState WV1;
    /// <param name="WV2"> refers to the water valve 2 component</param>
    public ValveState WV2;
    /// <param name="WP1"> refers to the water pump 1 component</param>
    public PumpState WP1;
    /// <param name="WP2"> refers to the water pump 2 component</param>
    public PumpState WP2;
    /// <param name="CP"> refers to the condenser pump component</param>
    public PumpState CP;
    /// <param name="Condenser"> refers to the condenser component</param>
    public CondenserState Condenser;
    /// <param name="Generator"> refers to the generator component</param>
    public GeneratorState Generator;
    /// <param name="ComponentHealth"> refers to anarray of components</param>
    public ComponentHealth ComponentHealth;
}

//system endpofloat classes
[Serializable]
/// <summary>
/// This class is used to store the state of the condenser component.
/// </summary>
public class CondenserState {

    /// <param name="pressure"> stores the pressure inside the condenser tank</param>
    public float pressure;
    /// <param name="waterLevel">  stores the water level inside the condenser tank</param>
    public float waterLevel;
    /// <param name="operational">  tracks whether the condenser tank is operational </param>
    public bool operational;
}

[Serializable]
/// <summary>
/// This class is used to store the state of the generator component.
/// </summary>
public class GeneratorState {


    /// <param name="name"> stores the name of the generator</param>
    public string name;
    /// <param name="blown"> tracks whether the generator is blown</param>
    public bool blown;    
    /// <param name="power"> stores the power output of the generator</param>
    public float power;
}

[Serializable]
/// <summary>
/// This class is used to store the state of the reactor component.
/// </summary>
public class ReactorState {

    /// <param name="pressure"> stores the pressure inside the reactor tank</param>
    public float pressure;
    /// <param name="waterLevel"> stores the water level inside the reactor tank</param>
    public float waterLevel;
    /// <param name="operational"> tracks whether the reactor tank is operational</param>
    public bool operational;    
    /// <param name="intactact"> tracks whether the reactor tank is blown</param>
    public bool intactact;      
    /// <param name="rodPosition"> stores the position of the control rods</param>
    public float rodPosition;
    /// <param name="restheat"> stores the restheat inside the reactor tank</param>
	public int restheat;
    /// <param name="overheated"> tracks whether the reactor is overheated</param>
	public bool overheated;
}

[Serializable]
/// <summary>
/// This class is used to store the state of a pump component.
/// </summary>
public class PumpState {

    /// <param name="name"> stores the name of a pump</param>
    public string name;
    /// <param name="blown"> tracks whether the pump is blown</param>
    public bool blown;          
    /// <param name="rpm"> stores the current RPM of a pump</param>
    public float rpm;
    /// <param name="setRpm">stores the set RPM of a pump</param>
    public float setRpm;
    /// <param name="maxRpm"> stores the maximum RPM of a pump</param>
    public float maxRpm;
    /// <param name="operational"> tracks whether a pump is operational</param>
    public bool operational;    
}

[Serializable]
/// <summary>
/// This class is used to store the state of a valve component.
/// </summary>
public class ValveState {

    /// <param name="name"> stores the name of a valve</param>
    public string name;
    /// <param name="blown"> tracks whether a valve is blown</param>
    public bool blown;
    /// <param name="status"> tracks whether a valve is open</param>
    public bool status;
} 

[JsonObject]
/// <summary>
/// This class is used to store the state of all components, see ComponentState.
/// </summary>
public class ComponentHealth {

    /// <param name="components"> is an array storing the state of all components</param>
    public ComponentState[] components;
}

[JsonObject]
/// <summary>
/// This class is used to store the state of a component.
/// </summary>
public class ComponentState {

    [JsonProperty("name")]
    /// <param name="name"> stores the name of a component</param>
    public string name;
    [JsonProperty("broken")]
    /// <param name="broken"> tracks whether a component is broken</param>
    public bool broken;

    [JsonProperty("status")]
    /// <param name="status"> tracks whether a component is operational</param>
    public bool status = true;
}

//system endpofloat classes
[Serializable]
/// <summary>
/// This class is used to store the state of the simulation.
/// </summary>
public class SystemStatus {

    /// <param name="runningSince"> stores the time the simulation has been running</param>
    public string runningSince;
    /// <param name="running"> tracks whether the simulation is running</param>
    public bool running;

    ///<summary> Returns a DateTime object from the runningSince string</summary>
    public DateTime getDate() {
        return DateTime.Parse(runningSince);
    }

}

[Serializable]
/// <summary>
/// This class is used to store error states.
/// </summary>
public class ErrorState {

    /// <param name="error"> stores the error</param>
    public string error;
    /// <param name="message"> stores the error message</param>
    public string message;
}