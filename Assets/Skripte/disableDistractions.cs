using UnityEngine;

public class disableDistractions : MonoBehaviour
{
    public GameObject[] distractions;
    public bool disableDistractionsOnStart = false;
    void Start()
    {
        if(disableDistractionsOnStart) disableDistraction(true);
        
    }
    
    
    public void disableDistraction(bool value){
        if (value)
        {
            foreach (GameObject distraction in distractions)
            {
                foreach (Transform child in distraction.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (GameObject distraction in distractions)
            {
                foreach (Transform child in distraction.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

}
