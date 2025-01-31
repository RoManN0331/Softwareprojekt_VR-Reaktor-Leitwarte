using System.Collections.Generic;
using UnityEngine;

public class AusfallAnzeigenManager : MonoBehaviour
{
    //Aus einem anderen Skript aufrufbar mit z.B.:
    //AusfallAnzeigenManager ausfallAnzeigenManager = FindObjectOfType<AusfallAnzeigenManager>();
    //ausfallAnzeigenManager.TurnOn("WP1");
    //dadruch wird dann die lampe WP1 eingeschaltet
    //die restlichen eingaben wären RKS,RKT,KNT,TBN,WP1,WP2,CP und AU
    
    public List<AusfallAnzeige> ausfallAnzeigen;
    


    public void TurnOn(string name)
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            if (anzeige.name == name)
            {
                anzeige.turnOn();
                break;
            }
        }
    }
    
    public void TurnAllOn()
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            anzeige.turnOn();
        }
        
        //Zusätzlich noch Lampen im Raum rot machen
        SetAllLampsToRed();
    }
    
    public void SetAllLampsToRed()
    {
        Light[] lamps = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light lamp in lamps)
        {
            Debug.Log("here");
            lamp.color = Color.red;
        }
    }

    public void TurnOff(string name)
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            if (anzeige.name == name)
            {
                anzeige.turnOff();
                break;
            }
        }
    }
}