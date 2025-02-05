using UnityEngine;

public class s3006 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
        set WP1RPM, WP2RPM. CPRPM to 0  */

    private bool setGroup1;
    private bool setCPRPM;
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

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM", true);
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM", true);
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM_display", true);            
            gazeGuidingPathPlayer.ToggleBlur("CPRPM", true);
            gazeGuidingPathPlayer.ToggleBlur("CPRPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM_display", true);            
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", true);
        }

    }
    
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

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM", false);
            gazeGuidingPathPlayer.ToggleBlur("WP1RPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM", false);
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM_display", false);            
            gazeGuidingPathPlayer.ToggleBlur("CPRPM", false);
            gazeGuidingPathPlayer.ToggleBlur("CPRPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP1RPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM_display", false);            
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPRPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", false);
        }

        // reset the scenario
        FindObjectOfType<AnimatorController>().updateScenario(0);


    }
    
}
