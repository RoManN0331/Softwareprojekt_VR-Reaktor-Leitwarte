using UnityEngine;

public class s3003 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
    set RWaterLvl to 2100 via WP2    */

    private GameObject target;
    private GameObject target2;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;
    private NPPReactorState simulation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        simulation = FindAnyObjectByType<NPPClient>().simulation;
        
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer2 = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();            
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        
        
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(4);
        target = GameObject.Find("WP2RPM").gameObject;
        target2 = GameObject.Find("RWaterLvl").gameObject;
        
        gazeGuidingPathPlayer.TriggerTargetNAME("WP2RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf,true);
        
        gazeGuidingPathPlayer2.TriggerTargetNAME("RWaterLvl", target2.GetComponent<GazeGuidingTarget>().isTypeOf);   
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RWaterLvl", GazeGuidingTarget.TargetType.Anzeige, 2100);
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Wenn Wasserstand steigt -> WP2 zudrehen
        if (simulation.Reactor.waterLevel > 2100)
        {
            gazeGuidingPathPlayer.TriggerTargetNAME("WP2RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf,true);
        }else if (simulation.Reactor.waterLevel < 2100)
        {
            //Wenn Wasserstand fällt -> WP2 aufdrehen
            gazeGuidingPathPlayer.TriggerTargetNAME("WP2RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        }
    }
    

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
    }
    
}
