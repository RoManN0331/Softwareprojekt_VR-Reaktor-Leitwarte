using UnityEngine;

public class s2005 : StateMachineBehaviour
{

    /*  script: herunterfahren
        set CPRPM to 0          */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler palte
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.removeHighlightFromClipboard();  
        

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(6);

        target = GameObject.Find("CPRPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("CPRPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        // show arrows
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("CPRPM_display", GazeGuidingTarget.TargetType.Anzeige, 0);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("CPRPM", true);
            gazeGuidingPathPlayer.ToggleBlur("CPRPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", true);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", true);
        }
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see GazeGuidingPathPlayer.updade()
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("CPRPM", false);
            gazeGuidingPathPlayer.ToggleBlur("CPRPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", false);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", false);
        }
    }
    
}
