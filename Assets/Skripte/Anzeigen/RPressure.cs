using UnityEngine;
using System.Collections;

public class RPressure : MonoBehaviour
{
    private AnzeigeSteuerung anzeigeSteuerung;

    private GameObject clientObject;

/// <summary>
/// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
/// The AnzeigeSteuerung Script is responsible for updating the display with the current pressure filling level of the reactor tank. Because the maximum capacity of the reactor tank is set to 500 in the simulation, the update for the display is computed by the following formula: current pressure / 500 * 100.
/// The NPPClient Script is responsible for fetching the current pressure inside the reactor tank from the simulation via the REST Server API. The current reactor pressure is stored in Reactor.pressure field of the NPPReactorState object simulation.
/// </summary>

    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.pressure / 500 * 100;
        }
    }

/// <summary>
/// Update() updates the display for the reactors pressure filling level within each frame by fetching the pressure inside the reactor tank stored in the Reactor.pressure field of the NPPClient Scripts simulation object and updating the pressure filling level by computing: current pressure / 500 * 100.
/// </summary>

    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.pressure / 500 * 100;
    }

}
