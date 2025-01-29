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
        
        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(5);
        //target = GameObject.Find("RWaterLvl").gameObject;
        target2 = GameObject.Find("WP1RPM").gameObject;
        //gazeGuidingPathPlayer.TriggerTargetNAME("RWaterLvl", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        gazeGuidingPathPlayer2.TriggerTargetNAME("WP1RPM", target2.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        /* Wäre es nicht Sinnvoller auf Modpos zu zeigen? anstatt RWaterLvl?
         da es ja sowieso mit dem Pfeil gehighlightet wird */
        target = GameObject.Find("ModPos").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("ModPos", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RWaterLvl", GazeGuidingTarget.TargetType.Anzeige, 2100);
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
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
        gazeGuidingPathPlayer2.ClearLine();
    }
}
