using UnityEngine;

public class s3006 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
        set WP1RPM, WP2RPM. CPRPM to 0  */

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
        
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(7);
        target = GameObject.Find("WP1RPM").gameObject;
        target2 = GameObject.Find("WP2RPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf,true);
        gazeGuidingPathPlayer2.TriggerTargetNAME("WP2RPM", target2.GetComponent<GazeGuidingTarget>().isTypeOf,true);  
        setGroup1 = true;
        //unvollstaendig
    
    }

    private bool setGroup1;
    private bool setCPRPM;
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Zuerst WP1RPM auf 0 und WP2RPM auf 0 danach CPRPM auf 0
        if (Mathf.Abs(simulation.WP1.rpm) < 0.0001f && Mathf.Abs(simulation.WP2.rpm) < 0.0001f)
        {
            if(!setCPRPM){
                target = GameObject.Find("CPRPM").gameObject;
                gazeGuidingPathPlayer.TriggerTargetNAME("CPRPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
                
                gazeGuidingPathPlayer2.ClearLine();
                setCPRPM = true;
                setGroup1 = false;
            }
        }else{
            
            if(!setGroup1){
                target = GameObject.Find("WP1RPM").gameObject;
                target2 = GameObject.Find("WP2RPM").gameObject;
                gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf,true);
                gazeGuidingPathPlayer2.TriggerTargetNAME("WP2RPM", target2.GetComponent<GazeGuidingTarget>().isTypeOf,true);
                setGroup1 = true;
                setCPRPM = false;
            }
        }
    }
    

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
        gazeGuidingPathPlayer.ClearLine();
        gazeGuidingPathPlayer2.ClearLine();

        // reset the scenario
        FindObjectOfType<AnimatorController>().updateScenario(0);


    }
    
}
