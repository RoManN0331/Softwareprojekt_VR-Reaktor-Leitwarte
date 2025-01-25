using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GazeGuidingPathPlayer : MonoBehaviour
{
    public bool DirectionCueEnabled = true;
    public float DirectionCueFadeDuration = 1f;
    
    public bool DirectionArrowEnabled = true;    
    
    public bool Arrow3DEnabled = true;
    
    public List<GazeGuidingTarget> targets;
    public float pathDisplayDistance = 5.0f;
    public float animationDuration = 1.0f; // Duration of the path drawing animation
    public Material lineMaterial; 
    private LineRenderer lineRenderer;
    private GazeGuidingTarget currentTarget;
    private Coroutine animatePathCoroutine;
    private bool isAnimating = false;

    //Fügt einen Roten-Rand am Bildschirm ein um den Nutzer zu signalisieren, dass er sich drehen soll
    private Image DirectionCue;
    private Image DirectionCue2;
    
    private GameObject arrow3DPrefab; // Reference to the Arrow3D prefab
    private GameObject arrow3DInstance; // Instance of the Arrow3D prefab
    
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
        
        
        // Load and instantiate the UI prefab
        GameObject uiPrefab = Resources.Load<GameObject>("Prefabs/UI/UI");
        GameObject uiInstance = Instantiate(uiPrefab);
        
        // Initialize the UI elements
        initUI(uiInstance);
        
        // Load the Arrow3D prefab
        arrow3DPrefab = Resources.Load<GameObject>("Prefabs/Arrow3D");
    }

    private void initUI(GameObject uiInstance)
    {
        // Assign the DirectionCue and DirectionCue2 images
        DirectionCue = uiInstance.transform.Find("Canvas/DirectionCue").GetComponent<Image>();
        DirectionCue2 = uiInstance.transform.Find("Canvas/DirectionCue2").GetComponent<Image>();

        // Set initial states
        DirectionCue.gameObject.SetActive(false);
        DirectionCue2.gameObject.SetActive(false);
            
        // Find the main camera in the XR Origin (XR Rig) > Camera Offset > Main Camera
        Camera mainCamera = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").GetComponent<Camera>();

        // Set the render camera of the Canvas component
        Canvas canvas = uiInstance.transform.Find("Canvas").GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        // Set the plane distance to bring the UI closer to the camera
        canvas.planeDistance = 0.05f;

        // Set the sorting order to ensure the UI is rendered on top of other objects
        canvas.sortingOrder = 100;
        
    }
    
    private bool arrow3DInstanceCreated = false; // Flag to track if Arrow3D instance has been created
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
            }

            if (Time.frameCount % 4 == 0 && DirectionCueEnabled)
            {
                calcDirectionCue();
            }
            else if (!(isDirectionCue2Fading && isDirectionCueFading) && !DirectionCueEnabled)
            {
                DirectionCue.gameObject.SetActive(false);
                DirectionCue2.gameObject.SetActive(false);
            }
        }
    }
    
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
    
    public void RemoveArrow3D()
    {
        if (arrow3DInstance != null)
        {
            Destroy(arrow3DInstance);
            arrow3DInstance = null;
        }

        arrow3DInstanceCreated = false;
    }
    
    
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

    public void ClearLine()
    {
        // Remove the Arrow3D instance
        RemoveArrow3D();
        
        DirectionCue.gameObject.SetActive(false);
        DirectionCue2.gameObject.SetActive(false);
        
        currentTarget = null;
        isAnimating = false;
        if (animatePathCoroutine != null)
        {
            StopCoroutine(animatePathCoroutine);
            animatePathCoroutine = null;
        }
        lineRenderer.positionCount = 0;
    }

    public void triggerTEST()
    {
        ClearLine();
        currentTarget = targets[0];
    }
    public void triggerTEST2(bool Flip3DArrow)
    {
        ClearLine();
        currentTarget = targets[0];
        if (Flip3DArrow)
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = true;
        }
        else
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = false;
        }
    }


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

    private bool isDirectionCueFading = false;
    private bool isDirectionCue2Fading = false;

    public void calcDirectionCue()
    {
        Vector3 directionToTarget = currentTarget.transform.position - transform.position;
        float angle = Mathf.DeltaAngle(transform.eulerAngles.y, Quaternion.LookRotation(directionToTarget).eulerAngles.y);

        bool directionCueActive = Mathf.Abs(angle) > 30;
        DirectionCue.gameObject.SetActive(directionCueActive);

        if (directionCueActive)
        {
            RectTransform directionCueRect = DirectionCue.rectTransform;
            RectTransform directionCue2Rect = DirectionCue2.rectTransform;

            if (angle > 0)
            {
                directionCueRect.anchorMin = new Vector2(1, 0.5f); // Right middle
                directionCueRect.anchorMax = new Vector2(1, 0.5f); // Right middle
                directionCueRect.anchoredPosition = new Vector2(-311.3f, directionCueRect.anchoredPosition.y);
                directionCueRect.localRotation = Quaternion.Euler(0, 0, 0); // Rotate the DirectionCue
            }
            else
            {
                directionCueRect.anchorMin = new Vector2(0, 0.5f); // Left middle
                directionCueRect.anchorMax = new Vector2(0, 0.5f); // Left middle
                directionCueRect.anchoredPosition = new Vector2(311.3f, directionCueRect.anchoredPosition.y);
                directionCueRect.localRotation = Quaternion.Euler(0, -180, 0); // Rotate the DirectionCue
            }

            // Calculate the alpha value based on the angle
            float alpha = Mathf.InverseLerp(30, 180, Mathf.Abs(angle));
            if (!isDirectionCueFading && Mathf.Abs(DirectionCue.canvasRenderer.GetAlpha() - alpha) > 0.075f)
            {
                isDirectionCueFading = true;
                DirectionCue.CrossFadeAlpha(alpha, DirectionCueFadeDuration, false);
                StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCueFading = false));
            }

            bool directionCue2Active = Mathf.Abs(angle) > 160 || isDirectionCue2Fading;
            if (!DirectionCue2.gameObject.activeSelf)
            {
                DirectionCue2.gameObject.SetActive(directionCue2Active);
            }
            
            if (directionCue2Active)
            {
                if (angle > 160)
                {
                    directionCue2Rect.anchorMin = new Vector2(0, 0.5f); // Opposite anchor
                    directionCue2Rect.anchorMax = new Vector2(0, 0.5f); // Opposite anchor
                    directionCue2Rect.anchoredPosition = new Vector2(311.3f, directionCue2Rect.anchoredPosition.y); // Opposite position
                    directionCue2Rect.localRotation = Quaternion.Euler(0, -180, 0); // Rotate the DirectionCue2
                }
                else if (angle < -160)
                {
                    directionCue2Rect.anchorMin = new Vector2(1, 0.5f); // Opposite anchor
                    directionCue2Rect.anchorMax = new Vector2(1, 0.5f); // Opposite anchor
                    directionCue2Rect.anchoredPosition = new Vector2(-311.3f, directionCue2Rect.anchoredPosition.y); // Opposite position
                    directionCue2Rect.localRotation = Quaternion.Euler(0, 0, 0); // Rotate the DirectionCue2
                }
                if (!isDirectionCue2Fading && Mathf.Abs(DirectionCue2.canvasRenderer.GetAlpha() - alpha) > 0.075f)
                {
                    isDirectionCue2Fading = true;
                    DirectionCue2.CrossFadeAlpha(alpha, DirectionCueFadeDuration, false);
                    StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCue2Fading = false));
                }
            }
            else
            {
                if (!isDirectionCue2Fading && DirectionCue2.canvasRenderer.GetAlpha() > 0 && !directionCue2Active)
                {
                    isDirectionCue2Fading = true;
                    DirectionCue2.CrossFadeAlpha(0, DirectionCueFadeDuration, false);
                    StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCue2Fading = false));
                }
            }
        }
        else
        {
            if (!isDirectionCueFading && DirectionCue.canvasRenderer.GetAlpha() > 0)
            {
                isDirectionCueFading = true;
                DirectionCue.CrossFadeAlpha(0, DirectionCueFadeDuration, false);
                StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCueFading = false));
            }
            if (!isDirectionCue2Fading && DirectionCue2.canvasRenderer.GetAlpha() > 0)
            {
                isDirectionCue2Fading = true;
                DirectionCue2.CrossFadeAlpha(0, DirectionCueFadeDuration, false);
                StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCue2Fading = false));
            }
        }
    }

    private IEnumerator ResetFadingFlag(float duration, System.Action resetAction)
    {
        yield return new WaitForSeconds(duration);
        resetAction();
    }
    
    
}
