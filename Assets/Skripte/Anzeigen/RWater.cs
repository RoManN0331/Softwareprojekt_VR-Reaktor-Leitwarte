using UnityEngine;
using System.Collections;

/// <summary>
/// This class is responsible for updating the display showing the current water level inside the reactor tank.
/// </summary>
public class RWater : MonoBehaviour
{
    /// <param name="anzeigeSteuerung"> Reference to the AnzeigeSteuerung script of the display </param>
    private AnzeigeSteuerung5 anzeigeSteuerung;
    /// <param name="clientObject"> Reference to the NPPClient object </param>
    private GameObject clientObject;

    /// <summary>
    /// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
    /// The AnzeigeSteuerung Script is responsible for updating the display with the water level of the reactor tank. Because the maximum capacity of the reactor tank is set in the simulation to 2900, the update of the water level is computed by the following formula: current water level / 2900 * 100.
    /// The NPPClient Script is responsible for fetching the current water level of the reactor tank from the simulation via the REST Server API. The current water level is stored in Reactor.waterLevel field of the NPPReactorState object simulation.
    /// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung5>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.waterLevel / 2900 * 100;
        }
    }

    /// <summary>
    /// Update() updates the display for the reactor water level within each frame by fetching the water level stored in the Reactor.waterLevel field of the NPPClient Scripts simulation object and updating the water level by computing: current water level / 2900 * 100.
    /// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.waterLevel / 2900 * 100;
    }

}