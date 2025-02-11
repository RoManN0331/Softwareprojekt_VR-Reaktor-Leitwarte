using UnityEngine;

public class s1003 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        set WP1RPM to 200   */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  
        gazeGuidingPathPlayer.unsetDisplayHighlight();
        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(4);
        target = GameObject.Find("WP1RPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        // either or
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("WP1RPM_display", GazeGuidingTarget.TargetType.Anzeige, 200);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM", true);
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", true);
        }


        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", true);
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

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM", false);
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", false);
        }         
    }

}
