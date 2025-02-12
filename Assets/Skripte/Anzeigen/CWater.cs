using UnityEngine;
using System.Collections;

/// <summary>
/// This class is responsible for updating the display showing the current water level inside the condenser tank.
/// </summary>
public class CWater : MonoBehaviour
{

    /// <param name="anzeigeSteuerung"> Reference to the AnzeigeSteuerung script of the display </param>
    private AnzeigeSteuerung5 anzeigeSteuerung;
    /// <param name="clientObject"> Reference to the NPPClient object </param>
    private GameObject clientObject;

    /// <summary>
    /// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
    /// The AnzeigeSteuerung Script is responsible for updating the display with the water level of the condenser tank. Because the maximum capacity of the condenser tank is set in the simulation to 5000, the update of the water level is computed by the following formula: current water level / 5000 * 100.
    /// The NPPClient Script is responsible for fetching the current water level of the condenser tank from the simulation via the REST Server API. The current water level is stored in Condenser.waterLevel field of the NPPReactorState object simulation.
    /// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung5>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.waterLevel/5000*100;
            //Debug.Log("CWater: " + anzeigeSteuerung.CHANGEpercentage);
        }
    }

    /// <summary>
    /// Update() updates the display for the condenser water level within each frame by fetching the water level stored in the Condenser.waterLevel field of the NPPClient Scripts simulation object and updating the water level by computing: current water level / 5000 * 100.
    /// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.waterLevel/5000*100;
    }

}
