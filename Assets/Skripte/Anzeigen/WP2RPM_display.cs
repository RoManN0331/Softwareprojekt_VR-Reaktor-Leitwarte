using UnityEngine;
using System.Collections;

/// <summary>
/// This class implements logic for displaying the current RPM of water pump 2.
/// </summary>
public class WP2RPM : MonoBehaviour
{
    /// <param name="anzeigeSteuerung"> is a reference to the displays AnzeigeSteuerung component </param>
    private AnzeigeSteuerung anzeigeSteuerung;

    /// <param name="clientObject"=> is a reference to the scene's clientObject</param>
    private GameObject clientObject;

/// <summary>
/// This method initializes the AnzeigeSteuerung component, clientObject and the display by calling the NPPReactorState object in NPPClient to fetch the current RPM of water pump 2.</summary>
/// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.WP2.rpm / 2000f * 100;
        }
    }

/// <summary>
/// This method updates the display by reading the current RPM of water pump 2 from the WP2.rpm field of the NPPReactorState object in NPPClient. 
/// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.WP2.rpm / 2000f * 100;
    }

}
