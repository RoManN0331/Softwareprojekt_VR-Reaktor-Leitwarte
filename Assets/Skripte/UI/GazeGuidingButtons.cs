using System.Collections;
using UnityEngine;

public class GazeGuidingButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GazeGuidingPathPlayer pathPlayer;
    private GazeGuidingPathPlayerSecondPath pathPlayer2;
    void Start()
    {
        pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        pathPlayer2 = FindAnyObjectByType<GazeGuidingPathPlayerSecondPath>();
        
        HUDPrefab = Resources.Load<GameObject>("Prefabs/UI/HUD");
    }

    public void DirectionCue(bool TurnOn)
    {
        pathPlayer.DirectionCueEnabledGlobal = TurnOn;
    }


    public void DirectionArrow(bool TurnOn)
    {
        pathPlayer.DirectionArrowEnabled = TurnOn;
        pathPlayer2.DirectionArrowEnabled = TurnOn;
    }

    public void Arrow3D(bool TurnOn)
    {
        pathPlayer.Arrow3DEnabled = TurnOn;
        pathPlayer2.Arrow3DEnabled = TurnOn;
    }

    public void Arrow3DBinear(bool TurnOn)
    {
        pathPlayer.Arrow3DBinearEnabled = TurnOn;
        pathPlayer2.Arrow3DBinearEnabled = TurnOn;
    }

    public void DirectionArrowOnScreen(bool TurnOn)
    {
        pathPlayer.DirectionArrowOnScreen = TurnOn;
    }

    public void AnzeigenMarkierung(bool TurnOn)
    {
        pathPlayer.AnzeigenMarkierungEnabled = TurnOn;
    }

    public void TriggerBlur(bool state)
    {
        pathPlayer.SetBlur(state);
    }

    public void TriggerDetach(bool state)
    {
        pathPlayer.SetDetach(state);
    }
    
    public void Distractions(bool state)
    {
        FindAnyObjectByType<disableDistractions>().disableDistraction(state);
    }
    
    public void ClipboardHighlight(bool state)
    {
        if (state)
        {
            GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
            pathPlayer.ClipBoardTextColor = "<color=#00FF00>";
            pathPlayer.removeHighlightFromClipboardForButton();
        }
        else
        {
            GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
            pathPlayer.ClipBoardTextColor = "<color=#000000>";
            pathPlayer.removeHighlightFromClipboardForButton();
        }
    }
    
    public void AnzeigenHighlight(bool state)
    {
        if (state)
        {
            GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
            pathPlayer.DisplayHighlightEnabled = true;
            pathPlayer.unsetDisplayHighlight();
            pathPlayer.setDisplayHighlight(pathPlayer.lastCalledHighlight);
        }
        else
        {
            GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
            pathPlayer.DisplayHighlightEnabled = false;
            pathPlayer.unsetDisplayHighlight();
        }
    }


    private GameObject HUDPrefab;
    GameObject HUDInstance;
    public void HUD(bool TurnOn)
    {
        if (TurnOn)
        {
            HUDInstance = Instantiate(HUDPrefab);
            StartCoroutine(DelayedGiveTextToHUD());
        }
        else
        {
            Destroy(HUDInstance);
        }
    }
    
    private IEnumerator DelayedGiveTextToHUD()
    {
        GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        yield return new WaitForSeconds(1f);
        pathPlayer.giveTexttoHUD();
    }
}
