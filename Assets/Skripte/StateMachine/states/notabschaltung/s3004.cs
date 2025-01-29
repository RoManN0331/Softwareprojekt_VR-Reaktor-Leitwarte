using UnityEngine;

public class s3004 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
        set Condenser pressure to 0 */

    private GameObject target;
    //private GameObject target2;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    //private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        //gazeGuidingPathPlayer2 = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();            
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        
        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(5);
        target = GameObject.Find("CPressure").gameObject;
        //target2 = GameObject.Find("RWaterLvl").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("CPressure", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        //gazeGuidingPathPlayer2.TriggerTargetNAME("RWaterLvl", target2.GetComponent<GazeGuidingTarget>().isTypeOf); 
        
        
        
        
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("CPressure", GazeGuidingTarget.TargetType.Anzeige, 0);
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RPressure", GazeGuidingTarget.TargetType.Anzeige, 0);
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see gazeGuidingPathPlayer.Update();
        and
        see gazeGuidingPathPlayer2.Update();        
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
    }
    
}
