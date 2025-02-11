using UnityEngine;
using System.Collections;

public class CPRPM : MonoBehaviour
{
    private AnzeigeSteuerung anzeigeSteuerung;

    private GameObject clientObject;

/// <summary>
/// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
/// The AnzeigeSteuerung Script is responsible for updating the display with the current speed ratio of the condenser pump. Because the maximum RPM of the condenser pump is set in the simulation to 2000, the update of the speed ratio is computed by the following formula: current RPM / 2000 * 100.
/// The NPPClient Script is responsible for fetching the current RPM of the condenser pump from the simulation via the REST Server API. The current RPM is stored in CP.rpm field of the NPPReactorState object simulation.
/// </summary>

    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.CP.rpm / 2000f * 100;
        }
    }

/// <summary>
/// Update() updates the display for the condenser pumps speed ratio within each frame by fetching its RPM stored in the CP.rpm field of the NPPClient Scripts simulation object and updating the speed ration by computing: current RPM / 2000 * 100.
/// </summary>

    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.CP.rpm / 2000f * 100;
    }

}
