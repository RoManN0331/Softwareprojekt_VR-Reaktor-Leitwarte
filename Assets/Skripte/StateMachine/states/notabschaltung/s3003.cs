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
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(4);

        target = GameObject.Find("WP2RPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP2RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf,true);
        
        // show arrows        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RWaterLvl", GazeGuidingTarget.TargetType.Anzeige, 2100);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM", true);
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM_display", true);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM_display", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", true);
        }
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        /* Entfernen?! */

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
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM", false);
            gazeGuidingPathPlayer.ToggleBlur("WP2RPM_display", false);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("CWaterLvl", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WP2RPM_display", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CWaterLvl", false);
        }

    }
    
}
