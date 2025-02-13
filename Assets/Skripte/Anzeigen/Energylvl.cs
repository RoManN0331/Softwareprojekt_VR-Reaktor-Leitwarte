using UnityEngine;

/// <summary>
/// This class implements logic for displaying the current power output of the generator.
/// </summary>
public class Energylvl : MonoBehaviour
{
    /// <param name="anzeigeSteuerung"> is a reference to the displays AnzeigeSteuerung component </param>
    private AnzeigeSteuerung anzeigeSteuerung;

    /// <param name="clientObject"=> is a reference to the scene's clientObject</param>
    private GameObject clientObject;

/// <summary>
/// This method initializes the AnzeigeSteuerung component, clientObject and the display by calling the NPPReactorState object in NPPClient to fetch the current power output of the genrator.</summary>
/// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Generator.power/1500*100;
        }
        
    }

/// <summary>
/// This method updates the display by reading the current power output from the Generator.power field of the NPPReactorState object in NPPClient. 
/// </summary>
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Generator.power/1500*100;
    }
}
