using UnityEngine;

public class s1001 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        set CPRPM to 1600   */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        


        gazeGuidingPathPlayer.removeHighlightFromClipboard(); 
        gazeGuidingPathPlayer.ClearLine();
        
        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(2);

        target = GameObject.Find("CPRPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("CPRPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        // show arrows
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("CPRPM_display", GazeGuidingTarget.TargetType.Anzeige, 1600);
    
        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("CPRPM", true);
            gazeGuidingPathPlayer.ToggleBlur("CPRPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", true);
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
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("CPRPM", false);
            gazeGuidingPathPlayer.ToggleBlur("CPRPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", false);
        }


    }
}
