using UnityEngine;

public class s1004 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        set ReactorWaterLevel to 2100 via WP1RPM  */

    private GameObject target;
    private GameObject target2;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer2 = FindAnyObjectByType<GazeGuidingPathPlayerSecondPath>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  
        
        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(5);        
        target = GameObject.Find("ModPos").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("ModPos", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        //either or
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RWaterLvl", GazeGuidingTarget.TargetType.Anzeige, 2100);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("ModPos", true);
            gazeGuidingPathPlayer.ToggleBlur("controlRods", true);
            gazeGuidingPathPlayer.ToggleBlur("RPressure", true);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("ModPos", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("controlRods", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RPressure", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", true);
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
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("ModPos", false);
            gazeGuidingPathPlayer.ToggleBlur("controlRods", false);
            gazeGuidingPathPlayer.ToggleBlur("RPressure", false);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("ModPos", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("controlRods", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RPressure", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", false);
        }
    }
}
