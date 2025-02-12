using UnityEngine;
using System.Collections;

/// <summary>
/// This class is responsible for updating the display showing the current RPM of water pump 1.
/// </summary>
public class WP1RPM : MonoBehaviour
{
    /// <param name="anzeigeSteuerung"> Reference to the AnzeigeSteuerung script of the display </param>
    private AnzeigeSteuerung anzeigeSteuerung;
    /// <param name="clientObject"> Reference to the NPPClient object </param>
    private GameObject clientObject;

    /// <summary>
    /// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
    /// The AnzeigeSteuerung Script is responsible for updating the display with the current speed ratio of water pump 1 (WP1). Because the maximum RPM of WP1 is set in the simulation to 2000, the update of the speed ratio is computed by the following formula: current RPM / 2000 * 100.
    /// The NPPClient Script is responsible for fetching the current RPM of WP1 from the simulation via the REST Server API. The current RPM is stored in WP1.rpm field of the NPPReactorState object simulation.
    /// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.WP1.rpm / 2000f * 100;
        }
    }

    /// <summary>
    /// Update() updates the display for the water pumps speed ratio within each frame by fetching its RPM stored in the WP1.rpm field of the NPPClient Scripts simulation object and updating the speed ration by computing: current RPM / 2000 * 100.
    /// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.WP1.rpm / 2000f * 100;
    }

}
