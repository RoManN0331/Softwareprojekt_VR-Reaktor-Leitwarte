using UnityEngine;

/// <summary>
/// This class is used to enable or disable the different gaze-guiding features set via the gaze-guiding panel in the VR application. 
/// </summary>
public class GazeGuidingButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <param name="pathPlayer"> is a reference to the GazeGuidingPathPlayer instance in the scene</param>
    private GazeGuidingPathPlayer pathPlayer;
    /// <param name="pathPlayer2"> is a reference to the GazeGuidingPathPlayerSecondPath instance in the scene</param>
    private GazeGuidingPathPlayerSecondPath pathPlayer2;

    /// <summary>
    /// This method initialises the pathPlayer, pathPlayer2 and the HUDPrefab references.
    /// </summary>
    void Start()
    {
        pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        pathPlayer2 = FindAnyObjectByType<GazeGuidingPathPlayerSecondPath>();
        
        HUDPrefab = Resources.Load<GameObject>("Prefabs/UI/HUD");
    }

    /// <summary>
    /// This method enables or disables the direction cue feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the direction cue feature is enabled or disabled</param>
    public void DirectionCue(bool TurnOn)
    {
        pathPlayer.DirectionCueEnabledGlobal = TurnOn;
    }

    /// <summary>
    /// This method enables or disables the direction arrow feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the direction arrow feature is enabled or disabled</param>
    public void DirectionArrow(bool TurnOn)
    {
        pathPlayer.DirectionArrowEnabled = TurnOn;
        pathPlayer2.DirectionArrowEnabled = TurnOn;
    }

    /// <summary>
    /// This method enables or disables the continuous 3D arrow feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the continuous 3D arrow feature is enabled or disabled</param>
    public void Arrow3D(bool TurnOn)
    {
        pathPlayer.Arrow3DEnabled = TurnOn;
        pathPlayer2.Arrow3DEnabled = TurnOn;
    }

    /// <summary>
    /// This method enables or disables the binary 3D arrow feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the binary 3D arrow feature is enabled or disabled</param>
    public void Arrow3DBinear(bool TurnOn)
    {
        pathPlayer.Arrow3DBinearEnabled = TurnOn;
        pathPlayer2.Arrow3DBinearEnabled = TurnOn;
    }

    /// <summary>
    /// This method enables or disables the direction arrow feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the direction arrow feature is enabled or disabled</param>
    public void DirectionArrowOnScreen(bool TurnOn)
    {
        pathPlayer.DirectionArrowOnScreen = TurnOn;
    }

    /// <summary>
    /// This method enables or disables the display annotations feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the display annotations feature is enabled or disabled</param>
    public void AnzeigenMarkierung(bool TurnOn)
    {
        pathPlayer.AnzeigenMarkierungEnabled = TurnOn;
    }

    /// <summary>
    /// This method enables or disables the camera blur feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the camera blur feature is enabled or disabled</param>
    public void TriggerBlur(bool state)
    {
        pathPlayer.SetBlur(state);
    }

    /// <summary>
    /// This method enables or disables the detach feature.
    /// </summary>
    /// <param name="TurnOn"> toggles whether the detach feature is enabled or disabled</param>
    public void TriggerDetach(bool state)
    {
        pathPlayer.SetDetach(state);
    }


    /// <param name="HUDPrefab"> is a prefab for a HUD</param>
    private GameObject HUDPrefab;
    /// <param name="HUDInstance"> is a reference to an instance of a HUD</param>
    GameObject HUDInstance;

    /// <summary>
    /// This method enables or disables the HUD.
    /// </summary>
    /// <param name="TurnOn"> toggles the HUD either to enabled or disabled</param>
    public void HUD(bool TurnOn)
    {
        if (TurnOn)
        {
            HUDInstance = Instantiate(HUDPrefab);
        }
        else
        {
            Destroy(HUDInstance);
        }
    }
}
