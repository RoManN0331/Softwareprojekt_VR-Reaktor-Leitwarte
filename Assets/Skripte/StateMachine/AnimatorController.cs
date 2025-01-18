using System.Linq;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found!");
        }
    }

    public void UpdateAnimatorParameters(NPPReactorState state)
    {
        //Set System parameter
        animator.SetBool("SimRunning", state.SystemStatus.running);
        //Set health parameter
        animator.SetBool("ReactorStatus",     GetComponent("RKS", state.ComponentHealth).status);
        animator.SetBool("ReactorTankStatus", GetComponent("RKT", state.ComponentHealth).status);
        animator.SetBool("CondenserStatus",   GetComponent("KNT", state.ComponentHealth).status);
        animator.SetBool("TurbinStatus",      GetComponent("TBN", state.ComponentHealth).status);
        animator.SetBool("WP1Status",         GetComponent("WP1", state.ComponentHealth).status);
        animator.SetBool("WP2Status",         GetComponent("WP2", state.ComponentHealth).status);
        animator.SetBool("CPStatus",          GetComponent("CP", state.ComponentHealth).status);
        animator.SetBool("AtomicsStatus",     GetComponent("AU", state.ComponentHealth).status); 
        //Set valve parameter
        animator.SetBool("SV1Status", state.SV1.status);
        animator.SetBool("SV2Status", state.SV2.status);
        animator.SetBool("WV1Status", state.WV1.status);
        animator.SetBool("WV2Status", state.WV2.status);
        //Set pump parameter
        animator.SetInteger("WP1RPM", (int) state.WP1.rpm);
        animator.SetInteger("WP2RPM", (int) state.WP2.rpm);
        animator.SetInteger("CPRPM",  (int) state.CP.rpm);
        //Set reactor parameter
        animator.SetInteger("ControlRodsLvl",  (int) state.Reactor.rodPosition);
        animator.SetInteger("ReactorWaterLvl", (int) state.Reactor.waterLevel);
        animator.SetInteger("ReactorPressure", (int) state.Reactor.pressure);
        //Set condenser parameter
        animator.SetInteger("CondenserWaterLvl", (int) state.Condenser.waterLevel);
        animator.SetInteger("CondenserPressure", (int) state.Condenser.pressure);
        //set generator parameter
        animator.SetInteger("GeneratorOutput", (int) state.Generator.power);
    }

    private ComponentState GetComponent(string name, ComponentHealth health)
    {
        ComponentState component = health.components.FirstOrDefault(c => c.name == name);
        return (component == null) ? new ComponentState { name = name, status = false } : component;
    }
}
