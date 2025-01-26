using UnityEngine;

public class s000 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GazeGuidingPathPlayer gazeGuidingPathPlayer = FindObjectOfType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.TriggerTargetNAME("SV2", GazeGuidingTarget.TargetType.Binaer);
    }
    
}
