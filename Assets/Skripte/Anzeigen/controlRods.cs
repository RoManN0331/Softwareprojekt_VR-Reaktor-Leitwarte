using UnityEngine;
using System.Collections;

public class ControlRods : MonoBehaviour
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
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.rodPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Reactor.rodPosition;
    }

}