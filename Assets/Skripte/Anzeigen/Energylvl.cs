using UnityEngine;

public class Energylvl : MonoBehaviour
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
            anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Generator.power/1500*100;
            //Debug.Log("Energy: " + anzeigeSteuerung.CHANGEpercentage);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        anzeigeSteuerung.CHANGEpercentage = clientObject.GetComponent<NPPClient>().simulation.Generator.power/1500*100;
    }
}
