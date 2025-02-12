using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
/// <summary>
/// This class is used to store the state of the reactor and its components.
/// </summary>
public class NPPReactorState{

    /// <param name="SystemStatus">System status</param>
    public SystemStatus SystemStatus;
    /// <param name="Reactor">reactor component</param>
    public ReactorState Reactor;
    /// <param name="SV1">steam valve 1 component</param>
    public ValveState SV1;
    /// <param name="SV2">steam valve 2 component</param>
    public ValveState SV2;
    /// <param name="WV1">water valve 1 component</param>
    public ValveState WV1;
    /// <param name="WV2">water valve 2 component</param>
    public ValveState WV2;
    /// <param name="WP1">water pump 1 component</param>
    public PumpState WP1;
    /// <param name="WP2">water pump 2 component</param>
    public PumpState WP2;
    /// <param name="CP">condenser pump component</param>
    public PumpState CP;
    /// <param name="Condenser">condenser component</param>
    public CondenserState Condenser;
    /// <param name="Generator">generator component</param>
    public GeneratorState Generator;
    /// <param name="ComponentHealth">array of components</param>
    public ComponentHealth ComponentHealth;
}

//system endpofloat classes
[Serializable]
/// <summary>
/// This class is used to store the state of the condenser component.
/// </summary>
public class CondenserState {

    /// <param name="pressure">float storing the pressure inside the condenser tank</param>
    public float pressure;
    /// <param name="waterLevel"> float storing the water level inside the condenser tank</param>
    public float waterLevel;
    /// <param name="operational"> boolean tracking whether the condenser tank is operational </param>
    public bool operational;
}

[Serializable]
/// <summary>
/// This class is used to store the state of the generator component.
/// </summary>
public class GeneratorState {


    /// <param name="name">string storing the name of the generator</param>
    public string name;
    /// <param name="blown">boolean tracking whether the generator is blown</param>
    public bool blown;    
    /// <param name="power">float storing the power output of the generator</param>
    public float power;
}

[Serializable]
/// <summary>
/// This class is used to store the state of the reactor component.
/// </summary>
public class ReactorState {

    /// <param name="pressure">float storing the pressure inside the reactor tank</param>
    public float pressure;
    /// <param name="waterLevel">float storing the water level inside the reactor tank</param>
    public float waterLevel;
    /// <param name="operational">boolean tracking whether the reactor tank is operational</param>
    public bool operational;    
    /// <param name="intactact">boolean tracking whether the reactor tank is blown</param>
    public bool intactact;      
    /// <param name="rodPosition">float storing the position of the control rods</param>
    public float rodPosition;
    /// <param name="restheat">int storing the rest heat inside the reactor tank</param>
	public int restheat;
    /// <param name="overheated">boolean tracking whether the reactor is overheated</param>
	public bool overheated;
}

[Serializable]
/// <summary>
/// This class is used to store the state of a pump components.
/// </summary>
public class PumpState {

    /// <param name="name">string storing the name of the pump</param>
    public string name;
    /// <param name="blown">boolean tracking whether the pump is blown</param>
    public bool blown;          
    /// <param name="rpm">float storing the rotations per minute of the pump</param>
    public float rpm;
    /// <param name="setRpm">float storing the set rotations per minute of the pump</param>
    public float setRpm;
    /// <param name="maxRpm">float storing the maximum rotations per minute of the pump</param>
    public float maxRpm;
    /// <param name="operational">boolean tracking whether the pump is operational</param>
    public bool operational;    
}

[Serializable]
/// <summary>
/// This class is used to store the state of a valve component.
/// </summary>
public class ValveState {

    /// <param name="name">string storing the name of the valve</param>
    public string name;
    /// <param name="blown">boolean tracking whether the valve is blown</param>
    public bool blown;
    /// <param name="status">boolean tracking whether the valve is open</param>
    public bool status;
} 

[JsonObject]
/// <summary>
/// This class is used to store the state of all components, see ComponentState.
/// </summary>
public class ComponentHealth {

    /// <param name="components"> Array storing the state of all components</param>
    public ComponentState[] components;
}

[JsonObject]
/// <summary>
/// This class is used to store the state of a component.
/// </summary>
public class ComponentState {

    [JsonProperty("name")]
    /// <param name="name">string storing the name of the component</param>
    public string name;
    [JsonProperty("broken")]
    /// <param name="broken">boolean tracking whether the component is broken</param>
    public bool broken;

    [JsonProperty("status")]
    /// <param name="status">boolean tracking whether the component is operational</param>
    public bool status = true;
}

//system endpofloat classes
[Serializable]
/// <summary>
/// This class is used to store the state of the simulation.
/// </summary>
public class SystemStatus {

    /// <param name="runningSince">string storing the time the simulation has been running</param>
    public string runningSince;
    /// <param name="running">boolean tracking whether the simulation is running</param>
    public bool running;

    ///<summary> Returns a DateTime object from the runningSince</summary>
    public DateTime getDate() {
        return DateTime.Parse(runningSince);
    }

}

[Serializable]
/// <summary>
/// This class is used to store error states.
/// </summary>
public class ErrorState {

    /// <param name="error">string storing the error</param>
    public string error;
    /// <param name="message">string storing the error message</param>
    public string message;
}