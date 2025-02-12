using UnityEngine;

/// <summary>
/// This class implements logic to disable specific elements in the scene categorised as distractions and added to the distractions array. 
/// </summary>
public class disableDistractions : MonoBehaviour
{
    /// <param name="distractions">Array containing objects specified as distractions</param>
    public GameObject[] distractions;
    /// <param name="disableDistransOnStart"> boolean checking whether distractions are rendered on simulation start or not </param>
    public bool disableDistractionsOnStart = false;

    /// <summary>
    /// This method disables all objects added to the distractions array at the start of the simulation, if specified by the player.
    /// </summary>
    void Start()
    {
        if(disableDistractionsOnStart) disableDistraction(true);
        
    }
    
    /// <summary>
    /// This method implements the logic to disable or enable the distractions in the scene.
    /// </summary>
    /// <param name="value">boolean specifying whether distractions are enabled / disabled </param>
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
