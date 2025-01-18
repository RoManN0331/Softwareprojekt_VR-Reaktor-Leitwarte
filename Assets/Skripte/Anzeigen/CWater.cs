using UnityEngine;
using System.Collections;

public class CWater : MonoBehaviour
{
    private AnzeigeSteuerung anzeigeSteuerung;

    private GameObject clientObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.waterLevel/5000*100;
            Debug.Log("CWater: " + anzeigeSteuerung.CHANGEpercentage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Condenser.waterLevel/5000*100;
    }

}
