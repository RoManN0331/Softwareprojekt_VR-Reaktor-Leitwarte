using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This class implements logic for gaze guiding in a virtual environment.
/// <summary>
public class GazeGuidingPathPlayerSecondPath : MonoBehaviour
{

    /// <param name="DirectionArrowEnabled">Boolean to enable or disable the 3D arrow indicating the direction the player should turn</param>
    public bool DirectionArrowEnabled = true;    
    /// <param name="Arrow3DEnabled">Boolean to enable or disable the 3D arrow indicating the direction the player should turn</param>
    public bool Arrow3DEnabled = true;
    /// <param name="Arrow3DBinearEnabled">Boolean to enable or disable the 3D arrow indicating the direction the player should turn</param>
    public bool Arrow3DBinearEnabled = true;    
    /// <param name="targets"> List of GazeGuidingTarget objects</param>
    public List<GazeGuidingTarget> targets;
    /// <param name="pathDisplayDistance"> Float to set the distance at which a path as visual cue is displayed</param>
    public float pathDisplayDistance = 5.0f;
    /// <param name="animationDuration"> Float to set the duration of the path animation</param>
    public float animationDuration = 1.0f;
    /// <param name="lineMaterial"> Material used to render a path as visual cue</param>
    public Material lineMaterial; 
    /// <param name="lineRenderer"> LineRenderer object used to render a path as visual cue</param>
    private LineRenderer lineRenderer;
    /// <param name="currentTarget"> GazeGuidingTarget currently used for gaze guiding</param>
    private GazeGuidingTarget currentTarget;
    /// <param name="animatePathCoroutine"> Coroutine used to animate a path as visual cue</param>
    private Coroutine animatePathCoroutine;
    /// <param name="isAnimating"> Boolean tracking if a path as visual cue is currently being animated</param>
    private bool isAnimating = false;


    /// <param name="arrow3DPrefab"> Reference to the Arrow3D prefab used to render continuous rotational 3DArrows</param>
    private GameObject arrow3DPrefab; 
    /// <param name="arrow3DInstance"> Instance of the Arrow3D prefab used to render continuous rotational 3DArrows<param>
    private GameObject arrow3DInstance; 
    /// <param name="arrow3DBinaerPrefab"> Reference to the Arrow3D prefab used to render binary rotational 3DArrows</param>
    private GameObject arrow3DBinaerPrefab; 
    /// <param name="arrow3DBinaerInstance"> Instance of the Arrow3D prefab used to render binary rotational 3DArrows<param>
    private GameObject arrow3DBinaerInstance; // Instance of the Arrow3D prefab

    /// <summary>
    /// This method starts the gaze guiding path player and performs necessary initialisations. 
    /// </summary>
    void Start()
    {
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;

        lineRenderer.positionCount = 0;
        lineRenderer.material = lineMaterial; 

        // Set texture mode to Tile
        lineRenderer.textureMode = LineTextureMode.Tile;

        // Set texture scale
        lineRenderer.textureScale = new Vector2(2.36f, 0.57f);
        
        // Load the Arrow3D prefab
        arrow3DPrefab = Resources.Load<GameObject>("Prefabs/Arrow3D");
        arrow3DBinaerPrefab = Resources.Load<GameObject>("Prefabs/Arrow3DBinaer");
        
        targets = new List<GazeGuidingTarget>(FindObjectsOfType<GazeGuidingTarget>());
    }
    
    /// <param name="arrow3DInstanceCreated"> Flag to track if Arrow3D instance has been created </param>
    private bool arrow3DInstanceCreated = false; 
    /// <param name="arrow3DBinaerInstanceCreated"> Flag to track if Arrow3D instance has been created </param>
    private bool arrow3DBinaerInstanceCreated = false; 

    /// <summary>
    /// This method updates all active gaze guiding tools in each frame.
    /// </summary>
    void Update()
    {   
        if (currentTarget != null)
        {
            if (Time.frameCount % 2 == 0 && DirectionArrowEnabled)
            {
                float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
                if (distance < pathDisplayDistance)
                {
                    lineRenderer.positionCount = 0;
                    isAnimating = false;
                    if (animatePathCoroutine != null)
                    {
                        StopCoroutine(animatePathCoroutine);
                        animatePathCoroutine = null;
                    }
                    
                    // Instantiate Arrow3D prefab if not already instantiated
                    if (!arrow3DInstanceCreated)
                    {
                        Arrow3D();
                        arrow3DInstanceCreated = true;
                    }

                    if (!arrow3DBinaerInstanceCreated)
                    {
                        Arrow3DBinaer();
                        arrow3DBinaerInstanceCreated = true;
                    }
                }
                else
                {
                    
                    // Remove the Arrow3D instance if the distance is too high
                    RemoveArrow3D();
                    if (!isAnimating)
                    {
                        animatePathCoroutine = StartCoroutine(AnimatePath(currentTarget.transform.position));
                    }
                }
            }
            else if(!DirectionArrowEnabled)
            {
                if (animatePathCoroutine != null)
                {
                    StopCoroutine(animatePathCoroutine);
                    animatePathCoroutine = null;
                }
                isAnimating = false;
                if (animatePathCoroutine != null)
                {
                    StopCoroutine(animatePathCoroutine);
                    animatePathCoroutine = null;
                }
                lineRenderer.positionCount = 0;
            }
        }
    }

    /// <summary>
    /// This method adds a continuous rotating 3D arrow rendered above an exact rotary switch to indicate the direction in which the player is supposed to turn the switch.
    /// </summary>
    public void Arrow3D()
    {
        if (Arrow3DEnabled && currentTarget.isTypeOf == GazeGuidingTarget.TargetType.Genau)
        {
            if (arrow3DInstance == null)
            {
                arrow3DInstance = Instantiate(arrow3DPrefab, 
                    currentTarget.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
            
        }
    }

    /// <summary>
    /// This method adds a binary rotating 3D arrow rendered above a binary rotary switch to indicate the direction in which the player is supposed to turn the switch.
    /// </summary>
    public void Arrow3DBinaer()
    {
        if (Arrow3DBinearEnabled && currentTarget.isTypeOf == GazeGuidingTarget.TargetType.Binaer)
        {
            if (arrow3DBinaerInstance == null)
            {
                arrow3DBinaerInstance = Instantiate(arrow3DBinaerPrefab, 
                    currentTarget.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
            
        }
    }

    /// <summary>
    /// This method removes a rotating 3D arrow (continuous or binary) rendered above an exact rotary switch.
    /// </summary>
    public void RemoveArrow3D()
    {
        if (arrow3DInstance != null)
        {
            Destroy(arrow3DInstance);
            arrow3DInstance = null;
        }

        arrow3DInstanceCreated = false;

        if (arrow3DBinaerInstance != null)
        {
            Destroy(arrow3DBinaerInstance);
            arrow3DBinaerInstance = null;
        }
        
        arrow3DBinaerInstanceCreated = false;
    }

    // deprecated, not included in documentation    
    
    public void TriggerTarget(GazeGuidingTarget target, bool Flip3DArrow = false)
    {
        ClearLine();
        currentTarget = target;
        if (Flip3DArrow)
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = true;
        }
        else
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = false;
        }
    }

    
    
    //Aus einem anderen Skript aufrufbar mit z.B.:
    //GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayerSecondPath = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();
    //gazeGuidingPathPlayerSecondPath.TriggerTargetNAME("WP1RPM", GazeGuidingTarget.TargetType.Anzeige);
    //dadruch wird dann das gazeGuiding auf WP1RPM-anzeige gesetzt
    //die restlichen eingaben sind die GameObjektnamen mit dem GazeGuidingTarget skript

    /// <summary>
    /// This method sets an object as the current target for the gaze-guiding player and all active gaze guiding tools. Additionally if the target is a rotary switch the method enables a 3D arrow indicating the direction the player is supposed to turn the switch in.
    /// </summary>
    /// <param name="targetName"> String containing the name of the switch to target for gaze guiding </param>
    /// <param name="type"> Enum specifying the type of rotary switch the player is suppsed to use </param>
    /// <param name="Flip3DArrow"> Boolean indicating the direction in which the 3D arrow should rotate </param>
    public void TriggerTargetNAME(string targetName,  GazeGuidingTarget.TargetType type, bool Flip3DArrow = false)
    {
        ClearLine();
        currentTarget = targets.Find(t => t.name == targetName && t.isTypeOf == type);
        if (currentTarget != null)
        {
            if (type == GazeGuidingTarget.TargetType.Genau)
            {
                if (Flip3DArrow)
                {
                    arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = true;
                }
                else
                {
                    arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = false;
                }
            }else if (type == GazeGuidingTarget.TargetType.Binaer)
            {
                if (Flip3DArrow)
                {
                    arrow3DBinaerPrefab.GetComponent<Rotate3DArrowBinaer>().flipDirection = true;
                }
                else
                {
                    arrow3DBinaerPrefab.GetComponent<Rotate3DArrowBinaer>().flipDirection = false;
                }
            }
        }
        else
        {
            Debug.LogWarning($"Target with name {targetName} and type {type} not found.");
        }
    }

    /// <summary>
    /// This method clears all visual aids for the current target.
    /// </summary>    
    public void ClearLine()
    {
        // Remove the Arrow3D instance
        RemoveArrow3D();
        
        currentTarget = null;
        isAnimating = false;
        if (animatePathCoroutine != null)
        {
            StopCoroutine(animatePathCoroutine);
            animatePathCoroutine = null;
        }
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// This method animates a path of arrows guiding the player towards the component the player is supposed to interact with.
    /// </summary>
    /// <param name="targetPosition">Vector3 position of the endpoint of the path </param>
    private IEnumerator AnimatePath(Vector3 targetPosition)
    {
        isAnimating = true;
        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / animationDuration);
                Vector3 currentPoint = Vector3.Lerp(transform.position, targetPosition, t);

                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, currentPoint);

                yield return null;
            }

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetPosition);

            float waitTime = 0.5f;
            while (waitTime > 0)
            {
                lineRenderer.SetPosition(0, transform.position);
                waitTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
