using UnityEngine;
using System.Collections;


/// <summary>
/// This class is responsible for updating the display showing the current position of the control rods.
/// </summary>
public class ControlRods : MonoBehaviour
{



    /// <param name="anzeigeSteuerung"> Reference to the AnzeigeSteuerung script of the display </param>
    private AnzeigeSteuerung anzeigeSteuerung;
    /// <param name="clientObject"> Reference to the NPPClient object </param>
    private GameObject clientObject;

    /// <summary>
    /// Start () initializes the display update procedure by fetching the AnzeigeSteuerung script and if successful, fetching the NPPClient script afterwards. 
    /// The AnzeigeSteuerung Script is responsible for updating the display with the current position of the control rods. The current position of the control rods is displayed as a percentage fo the engagement of the rods, i.e. 100% = control rods fully engaged and 0% = control rods fully disengaged.
    /// The NPPClient Script is responsible for fetching the current position of the control rods from the simulation via the REST Server API. The current rod position is stored in Reactor.rodPosition field of the NPPReactorState object simulation.
    /// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.rodPosition;
        }
    }

    /// <summary>
    /// Update() updates the display for the control rods within each frame by fetching the value stored in the Reactor.rodPosition field of the NPPClient Scripts simulation object. 
    /// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.rodPosition;
    }

}