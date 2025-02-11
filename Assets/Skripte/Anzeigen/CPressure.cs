using UnityEngine;
using System.Collections;

/// <summary>
/// T/// The script is responsible for constantly updating the display showing the position of the control rods.
/// </summary>

public class CPressure : MonoBehaviour
{
    private AnzeigeSteuerung anzeigeSteuerung;

    private GameObject clientObject;

/// <summary>
/// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
/// The AnzeigeSteuerung Script is responsible for updating the display with the current pressure filling level of the condenser tank. Because the maximum capacity of the condenser tank is set to 140 in the simulation, the update for the display is computed by the following formula: current pressure / 140 * 100.
/// The NPPClient Script is responsible for fetching the current pressure inside the condenser tank from the simulation via the REST Server API. The current condenser pressure is stored in Condenser.pressure field of the NPPReactorState object simulation.
/// </summary>

    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.pressure / 140 * 100;
        }
    }

/// <summary>
/// Update() updates the display for the condensers pressure filling level within each frame by fetching the pressure inside the condenser stored in the Condenser.pressure field of the NPPClient Scripts simulation object and updating the pressure filling level by computing: current pressure / 140 * 100.
/// </summary>

    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.pressure / 140 * 100;
    }

}
