using UnityEngine;

public class s2000 : StateMachineBehaviour
{

    /*  script: herunterfahren
        set RWaterLevel to 2100 via WP1
        set power Output to 200 via ModPos  */
    
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

        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        // state specific

        gazeGuidingPathPlayer.SetGazeGuidingClipboard("POS2");
        gazeGuidingPathPlayer.HighlightClipboard(1);
        target = GameObject.Find("WP1RPM").gameObject;
        target2 = GameObject.Find("ModPos").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf, true);
        gazeGuidingPathPlayer2.TriggerTargetNAME("ModPos", target2.GetComponent<GazeGuidingTarget>().isTypeOf, true);   
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("Energy", GazeGuidingTarget.TargetType.Anzeige, 200);
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
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer2.ClearLine();
    }
    
}
