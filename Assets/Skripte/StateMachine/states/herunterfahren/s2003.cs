using UnityEngine;

public class s2003 : StateMachineBehaviour
{

    /*  script: herunterfahren
        set ReactorWaterLvl to 0 via WP1
        set WP1RPM to 0
        set power Output to 0 via ModPos
        set ModPos to 100                   */

    private GameObject target;
    private GameObject target2;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer2 = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        
        
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  
        
        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(4);
        
        target = GameObject.Find("WP1RPM").gameObject;
        target2 = GameObject.Find("ModPos").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf, true);
        gazeGuidingPathPlayer2.TriggerTargetNAME("ModPos", target2.GetComponent<GazeGuidingTarget>().isTypeOf, true);
        
        // show arrows
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("WP1RPM_display", GazeGuidingTarget.TargetType.Anzeige, 0);
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("Energy", GazeGuidingTarget.TargetType.Anzeige, 0);
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RWaterLvl", GazeGuidingTarget.TargetType.Anzeige, 2100);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM", true);
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("ModPos", true);
            gazeGuidingPathPlayer.ToggleBlur("controlRods", true);
            gazeGuidingPathPlayer.ToggleBlur("RPressure", true);
            gazeGuidingPathPlayer.ToggleBlur("Energy", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("ModPos", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("controlRods", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RPressure", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("Energy", true);
        }
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see GazeGuidingPathPlayer.updade()
        and
        see GazeGuidingPathPlayerSecondPath.updade()
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer2.ClearLine();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM", false);
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("ModPos", false);
            gazeGuidingPathPlayer.ToggleBlur("controlRods", false);
            gazeGuidingPathPlayer.ToggleBlur("RPressure", false);
            gazeGuidingPathPlayer.ToggleBlur("Energy", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("ModPos", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("controlRods", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RPressure", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("Energy", false);
        }
    }
    
}
