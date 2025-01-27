using UnityEngine;

public class disableDistractions : MonoBehaviour
{
    public GameObject[] distractions;
    public bool disableDistractionsOnStart = false;
    void Start()
    {
        if (disableDistractionsOnStart)
        {
            foreach (GameObject distraction in distractions)
            {
                foreach (Transform child in distraction.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }


}
