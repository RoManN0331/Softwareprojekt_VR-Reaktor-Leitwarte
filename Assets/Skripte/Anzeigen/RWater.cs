using UnityEngine;
using System.Collections;

public class RWater : MonoBehaviour
{
    private AnzeigeSteuerung5 anzeigeSteuerung;

    private GameObject clientObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung5>();
        if (anzeigeSteuerung != null)
        {

            clientObject = GameObject.Find("NPPclientObject");
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.waterLevel / 2900 * 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.waterLevel / 2900 * 100;
    }

}