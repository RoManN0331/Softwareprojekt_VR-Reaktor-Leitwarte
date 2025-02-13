using UnityEngine;
using System.Collections;

/// <summary>
/// This class implements logic for displaying the current water level inside the condenser tank.
/// </summary>
public class CWater : MonoBehaviour
{
    /// <param name="anzeigeSteuerung"> is a reference to the displays AnzeigeSteuerung component </param>
    private AnzeigeSteuerung5 anzeigeSteuerung;

    /// <param name="clientObject"=> is a reference to the scene's clientObject</param>
    private GameObject clientObject;

/// <summary>
/// This method initializes the AnzeigeSteuerung component, clientObject and the display by calling the NPPReactorState object in NPPClient to fetch the current water level inside the condenser tank.</summary>
/// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung5>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.waterLevel/5000*100;
        }
    }

/// <summary>
/// This method updates the display by reading the current water level inside the condenser tank from the Condenser.waterLevel field of the NPPReactorState object in NPPClient. 
/// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.waterLevel/5000*100;
    }

}
