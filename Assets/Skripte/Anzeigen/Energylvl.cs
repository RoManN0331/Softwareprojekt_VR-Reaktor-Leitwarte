using UnityEngine;

/// <summary>
/// This class is responsible for updating the display showing the current output of the generator.
/// </summary>
public class Energylvl : MonoBehaviour
{
    /// <param name="anzeigeSteuerung"> Reference to the AnzeigeSteuerung script of the display </param>
    private AnzeigeSteuerung anzeigeSteuerung;
    /// <param name="clientObject"> Reference to the NPPClient object </param>
    private GameObject clientObject;

    /// <summary>
    /// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script. 
    /// The AnzeigeSteuerung Script is responsible for updating the display with the energy level supplied by the generator. Because no maximum energy level is set within the simulation a value of 1500 was chosen and the update of the energy level is computed by the following formula: current energy level / 1500 * 100.
    /// The NPPClient Script is responsible for fetching the current energy level supplied by the generator from the simulation via the REST Server API. The current water level is stored in Generator.power field of the NPPReactorState object simulation.
    /// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Generator.power/1500*100;
            //Debug.Log("Energy: " + anzeigeSteuerung.CHANGEpercentage);
        }
        
    }

    /// <summary>
    /// Update() updates the display for the energy level within each frame by fetching the energy level stored in the Generator.power field of the NPPClient Scripts simulation object and updating the energy level by computing: current energy level / 1500 * 100.
    /// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Generator.power/1500*100;
    }
}
