using UnityEngine;

public class s1007 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        set RWaterLvl to 2100 via WP1RPM and
        set power Output to 700 via ModPos      */

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


        // state specific
        target = GameObject.Find("WP1RPM").gameObject;
        target2 = GameObject.Find("ModPos").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        gazeGuidingPathPlayer2.TriggerTargetNAME("ModPos", target2.GetComponent<GazeGuidingTarget>().isTypeOf);        
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
        //gazeGuidingPathPlayer.ClearLine();
        //gazeGuidingPathPlayer2.ClearLine();
    }
}
