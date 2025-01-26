using UnityEngine;

public class s3003 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
    set RWaterLvl to 2100 via WP2    */

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
        target = GameObject.Find("WP2RPM").gameObject;
        target2 = GameObject.Find("RWaterLvl").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP2RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        gazeGuidingPathPlayer2.TriggerTargetNAME("RWaterLvl", target2.GetComponent<GazeGuidingTarget>().isTypeOf);       
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see gazeGuidingPathPlayer.Update();
        and
        see gazeGuidingPathPlayer2.Update();    
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
    }
    */
}
