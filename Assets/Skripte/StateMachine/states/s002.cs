using UnityEngine;

public class s002 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GazeGuidingPathPlayer gazeGuidingPathPlayer = FindObjectOfType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.TriggerTargetNAME("WV1", GazeGuidingTarget.TargetType.Binaer);
    }
}
