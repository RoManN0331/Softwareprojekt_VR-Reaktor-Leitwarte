using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GazeGuidingPathPlayer : MonoBehaviour
{
    public bool DirectionCueEnabled = true;
    public float DirectionCueFadeDuration = 1f;
    
    public bool DirectionArrowEnabled = true;    
    
    public bool Arrow3DEnabled = true;
    
    public bool Arrow3DBinearEnabled = true;

    public bool DirectionArrowOnScreen = true;
    
    public bool AnzeigenMarkierungEnabled = true;
    
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
    
    private GameObject arrow3DBinaerPrefab; // Reference to the Arrow3D prefab
    private GameObject arrow3DBinaerInstance; // Instance of the Arrow3D prefab
    
    // GazeGuiding for clipboards

    private TextMeshPro clipboardText;
    private GazeGuidingClipboard GGClipboard;


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
        arrow3DBinaerPrefab = Resources.Load<GameObject>("Prefabs/Arrow3DBinaer");
        
        // Automatically set all GazeGuidingTarget objects
        targets = new List<GazeGuidingTarget>(FindObjectsOfType<GazeGuidingTarget>());
        
        
        text1 = GameObject.Find("POS1").transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();
        text2 = GameObject.Find("POS2").transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();
        text3 = GameObject.Find("POS3").transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();
        
        animator = FindObjectOfType<AnimatorController>();
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

        // Set the sorting order to ensure the UI is rendered on top of other objects but behind the HUD
        canvas.sortingOrder = 99;
        
    }

    private AnimatorController animator;
    private bool arrow3DInstanceCreated = false; // Flag to track if Arrow3D instance has been created
    private bool arrow3DBinaerInstanceCreated = false; // Flag to track if Arrow3D instance has been created
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
                    
                    isAnimating = false;
                    if (animatePathCoroutine != null)
                    {
                        StopCoroutine(animatePathCoroutine);
                        animatePathCoroutine = null;
                    }
                    lineRenderer.positionCount = 0;
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

            if (DirectionArrowOnScreen)
            {
                renderDirectionArrow();
            }else
            {
                removeDirectionArrow();
            }

            if (AnzeigenMarkierungEnabled && !Anzeigeninitialized)
            {
                for(int i = 0; i < anzeigenTargets.Count; i++)
                {
                    Transform anzeigenMarkerTransform = anzeigenTargets[i].transform.Find("AnzeigenMarker");
                    if (anzeigenMarkerTransform != null)
                    {
                        anzeigenMarkerTransform.gameObject.SetActive(true);
                        anzeigenMarkerTransform.GetComponent<AnzeigenMarker>().targetNumber = anzeigenNumbers[i];
                    }
                }
                Anzeigeninitialized = true;
            }
            else if(!AnzeigenMarkierungEnabled && Anzeigeninitialized)
            {
                foreach (var target in anzeigenTargets)
                {
                    Transform anzeigenMarkerTransform = target.transform.Find("AnzeigenMarker");
                    if (anzeigenMarkerTransform != null)
                    {
                        anzeigenMarkerTransform.gameObject.SetActive(false);
                    }
                }
                Anzeigeninitialized = false;
            }
           
        }

        //Stellt wirklich sicher das alles gelöscht wurde, da manche states es nicht richtig hinbekommen :/
        if (animator.getScenario() == 0 && !reset)
        {
            removeHighlightFromClipboard();
            ClearAnzeigenMarkierung();
            ClearLine();
            FindAnyObjectByType<GazeGuidingPathPlayerSecondPath>().ClearLine();
            UnsetGazeGuidingClipboard();
            reset = true;
        }
        else if(reset && (animator.getScenario() == 1 ||
                          animator.getScenario() == 2 ||
                          animator.getScenario() == 3))
        {
            reset = false;
        }
    }

    private bool reset = true;
    
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
    //GazeGuidingPathPlayer gazeGuidingPathPlayer = FindObjectOfType<GazeGuidingPathPlayer>();
    //gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", GazeGuidingTarget.TargetType.Anzeige);
    //dadruch wird dann das gazeGuiding auf WP1RPM-anzeige gesetzt
    //die restlichen eingaben sind die GameObjektnamen mit dem GazeGuidingTarget skript
    
    public void TriggerTargetNAME(string targetName,  GazeGuidingTarget.TargetType type, bool Flip3DArrow = false)
    {
        ClearLine();
        currentTarget = targets.Find(t => t.name == targetName && t.isTypeOf == type);

        if (targetName == "WP1RPM" || targetName == "WP2RPM" || targetName == "CPRPM" || targetName == "ModPos")
            setDisplayHighlight(targetName);

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
    
    
    public void ClearLine()
    {
        // Remove the Arrow3D instance
        RemoveArrow3D();

        removeDirectionArrow();
        
        ClearAnzeigenMarkierung();

        unsetDisplayHighlight();
        
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

    //3D-Pfeil wie in Need for Speed
    private GameObject arrowInstance; // Store the instance of the arrow

    public void renderDirectionArrow()
    {
        // Check if an instance already exists
        if (arrowInstance != null)
        {
            return; // Exit the method if an instance already exists
        }

        // Load the 3DPfeilNfS prefab from the Resources folder
        GameObject arrowPrefab = Resources.Load<GameObject>("Prefabs/3DPfeilNfS");

        if (arrowPrefab != null)
        {
            // Instantiate the prefab
            arrowInstance = Instantiate(arrowPrefab);
            arrowInstance.GetComponent<PfeilNfS>().target = currentTarget;
            
            // Find the main camera
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                // Calculate the position at the top of the screen
                Vector3 screenPosition = new Vector3(Screen.width / 2, Screen.height, mainCamera.nearClipPlane + 0.1f);
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

                // Set the position of the instantiated prefab
                arrowInstance.transform.position = worldPosition;

                // Optionally, set the parent to the camera to keep it in the same position relative to the screen
                arrowInstance.transform.SetParent(mainCamera.transform, true);
            }
            else
            {
                Debug.LogWarning("Main camera not found.");
            }
        }
        else
        {
            Debug.LogWarning("3DPfeilNfS prefab not found in Resources/Prefabs.");
        }
    }
    
    public void removeDirectionArrow()
    {
        // Check if an instance exists
        if (arrowInstance != null)
        {
            // Destroy the instance
            Destroy(arrowInstance);
            arrowInstance = null;
        }
    }
    
    

    private IEnumerator ResetFadingFlag(float duration, System.Action resetAction)
    {
        yield return new WaitForSeconds(duration);
        resetAction();
    }

    private List<GazeGuidingTarget> anzeigenTargets = new List<GazeGuidingTarget>();
    private List<float> anzeigenNumbers = new List<float>();
    
    bool Anzeigeninitialized = false;

    public void TriggerAnzeigenMarkierung(string targetName, GazeGuidingTarget.TargetType type, float NumberToHighlight)
    {
        GazeGuidingTarget target = targets.Find(t => t.name == targetName && t.isTypeOf == type);
        if (target != null) anzeigenTargets.Add(target);
        anzeigenNumbers.Add(NumberToHighlight);
        
        if (AnzeigenMarkierungEnabled)
        {
            if (target != null)
            {
                Transform anzeigenMarkerTransform = target.transform.Find("AnzeigenMarker");
                if (anzeigenMarkerTransform != null && !anzeigenMarkerTransform.gameObject.activeSelf)
                {
                    anzeigenMarkerTransform.gameObject.SetActive(true);
                    anzeigenMarkerTransform.GetComponent<AnzeigenMarker>().targetNumber = NumberToHighlight;
                    Anzeigeninitialized = true;
                }
                else
                {
                    Debug.LogError("Child GameObject 'AnzeigenMarker' not found.");
                }
            }
            else
            {
                Debug.LogWarning($"Target with name {targetName} and type {type} not found.");
            }
        }
    }
    
    public void ClearAnzeigenMarkierung()
    {
        foreach (var target in anzeigenTargets)
        {
            Transform anzeigenMarkerTransform = target.transform.Find("AnzeigenMarker");
            if (anzeigenMarkerTransform != null)
            {
                anzeigenMarkerTransform.gameObject.SetActive(false);
            }
        }
        anzeigenTargets.Clear();
        Anzeigeninitialized = false;
    }

    /*******************************
    ** gazeguiding for clipboards **
    *******************************/


    private TextMeshPro text1;
    private TextMeshPro text2;
    private TextMeshPro text3;
    
    private string initalText;
    private bool init = false;
    public void SetGazeGuidingClipboard (string clipboardName){

        /* activates gaze guiding for clipboard clipboardName */

        
        //Clipboards wurden nicht gefunden, wenn sie gehalten wurden?? -> vorher alle clipboards initialisieren
        if (clipboardName.Equals("POS1")) clipboardText = text1;
        if (clipboardName.Equals("POS2")) clipboardText = text2;
        if (clipboardName.Equals("POS3")) clipboardText = text3;
        
        
        if (!init)
        {
            initalText = clipboardText.text;
            init = true;
        }
        GGClipboard = new GazeGuidingClipboard(clipboardText.text);
    }

    public void UnsetGazeGuidingClipboard(){

        /* deactivates gaze guiding for a clipboard */

        GGClipboard = null;
        clipboardText = null;

    }
    
    public void HighlightClipboard(int index){
        /* highlights a portion of the text on the clipboard specified by index */

        if (GGClipboard == null || clipboardText == null){

            Debug.LogError("No clipboard set for gaze guiding");
            
        } else {

            GGClipboard.HighlightTask(index);
            clipboardText.text = GGClipboard.GetFormattedClipboardText();
        }
        
        //Give Text to the HUD

        HUD hud = FindAnyObjectByType<HUD>();

        hud.setText(GGClipboard.GetFormattedClipboardText());
    }


    public void removeHighlightFromClipboard(){

        /* removes highlighting by reinitialising the clipboard */

        if (clipboardText == null){

            Debug.LogError("No clipboard set for gaze guiding");
            
        } else {

            GGClipboard = new GazeGuidingClipboard(initalText);
            clipboardText.text = initalText;
            init = false;
        }
        
        //Clear HUD
        HUD hud = FindAnyObjectByType<HUD>();

        hud.clearText();
    }


    /**********************************
    ** Highlighting displays utility **
    **********************************/

    private bool WP1RPMisGlowing = false;
    private bool WP2RPMisGlowing = false;
    private bool CPRPMisGlowing = false;
    private bool ControlRodsisGlowing = false;
    private bool EnergyisGlowing = false;
    private bool RPressureisGlowing = false;
    private bool CPressureisGlowing = false;
    private bool RWaterLvlisGlowing = false;
    private bool CWaterLvlisGlowing = false;


    private void setDisplayHighlight(string targetName)
    {

        Debug.Log("setDisplayHighlight");
        /* toggles highlight for displays when setting pumps or moderator position */

        if(targetName == "WP1RPM")
        {
            /* sets highlight for the rpm display for WP1, reactor and condenser water levels*/

            if (!WP1RPMisGlowing)
            {
                GameObject.Find("WP1RPM_display").GetComponent<Glow>().setGlowing();
                WP1RPMisGlowing = true;
            }
            
            if (!RWaterLvlisGlowing)
            {
                GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
                RWaterLvlisGlowing = true;
            }
            
            if (!CWaterLvlisGlowing)
            {
                GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
                CWaterLvlisGlowing = true;

            }
        }
        else if (targetName == "WP2RPM")
        {
            /* sets highlight for the rpm display for WP2, reactor and condenser water levels*/

            if (!WP2RPMisGlowing)
            {
                GameObject.Find("WP2RPM_display").GetComponent<Glow>().setGlowing();
                WP2RPMisGlowing = true;                    
            }
            
            if (!RWaterLvlisGlowing)
            {
                GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
                RWaterLvlisGlowing = true;
            }

            
            if (!CWaterLvlisGlowing)
            {
                GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
                CWaterLvlisGlowing = true;
            }
        }
        else if (targetName == "CPRPM")
        {
            Debug.Log("CP");

            /* sets hightlight for the rpm display for CP, condenser pressure and condenser water level */
            if (!CPRPMisGlowing)
            {
                GameObject.Find("CPRPM_display").GetComponent<Glow>().setGlowing();
                CPRPMisGlowing = true;
                Debug.Log("CPRPMisGlowing");
            }
            
            if (!CPressureisGlowing)
            {
                GameObject.Find("CPressure").GetComponent<Glow>().setGlowing();
                CPressureisGlowing = true;
                Debug.Log("CPressureisGlowing");
            }
            
            if (!CWaterLvlisGlowing)
            {
                GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
                CWaterLvlisGlowing = true;
                Debug.Log("CWaterLvlisGlowing");
            }

        }
        else if (targetName == "ModPos")
        {
            /* sets highlight for moderator position, reactor water and reactor pressure level */

            if (!ControlRodsisGlowing)
            {
                GameObject.Find("controlRods").GetComponent<Glow>().setGlowing();
                ControlRodsisGlowing = true;
            }

            if (!RWaterLvlisGlowing)
            {
                GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
                RWaterLvlisGlowing = true;
            }
            
            if (!RPressureisGlowing)
            {
                GameObject.Find("RPressure").GetComponent<Glow>().setGlowing();
                RPressureisGlowing = true;
            }
            
            if (!EnergyisGlowing)
            {
                GameObject.Find("Energy").GetComponent<Glow>().setGlowing();
                EnergyisGlowing = true;
            }
        }
    }

    public void unsetDisplayHighlight()
    {
        if (WP1RPMisGlowing)
        {
            GameObject.Find("WP1RPM_display").GetComponent<Glow>().setGlowing();
            WP1RPMisGlowing = false;
        }
        if (WP2RPMisGlowing)
        { 
            GameObject.Find("WP2RPM_display").GetComponent<Glow>().setGlowing();
            WP2RPMisGlowing = false;
        }
        if (CPRPMisGlowing)
        {
            GameObject.Find("CPRPM_display").GetComponent<Glow>().setGlowing();
            CPRPMisGlowing = false;
        }
        if (ControlRodsisGlowing)
        {
            GameObject.Find("controlRods").GetComponent<Glow>().setGlowing();
            ControlRodsisGlowing = false;
        }
        if (EnergyisGlowing)
        {
            GameObject.Find("Energy").GetComponent<Glow>().setGlowing();
            EnergyisGlowing = false;
        }
        if (RPressureisGlowing)
        {
            GameObject.Find("RPressure").GetComponent<Glow>().setGlowing();
            RPressureisGlowing = false;
        }
        if (CPressureisGlowing)
        {
            GameObject.Find("CPressure").GetComponent<Glow>().setGlowing();
            CPressureisGlowing = false;
        }
        if (RWaterLvlisGlowing)
        {
            GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
            RWaterLvlisGlowing = false;
        }
        if (CWaterLvlisGlowing)
        {
            GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
            CWaterLvlisGlowing = false;
        }
    }


    /***********************************
    ** toggling visibility of objects **
    ***********************************/


    public bool detached = false;
    private bool checkCullingMask = true;


    public void SetDetach(bool on)
    {
        if (on)
            {
                if (checkCullingMask && (Camera.main.cullingMask & (1 << LayerMask.NameToLayer("detached"))) != 0)
                {
                    /* checks camera settings on initialization */

                    Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("detached"));
                    checkCullingMask = false;
                }

                GameObject.Find("Anzeigen").GetComponent<ChangeLayer>().setLayer("detached");
                GameObject.Find("Regler").GetComponent<ChangeLayer>().setLayer("detached");
                detached = true;
            }
        else
            {
                GameObject.Find("Anzeigen").GetComponent<ChangeLayer>().setLayer("Default");
                GameObject.Find("Regler").GetComponent<ChangeLayer>().setLayer("Default");
                detached = false;
            }
    }

    public void ToggleObjectVisibility(string target, bool on)
    {
        /*  toggles an objects visibility by moving it between layers */

        if (on)
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("Ddefault");
        else
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("detached");
    }


    /****************
    ** camera blur **
    ****************/


    public bool blur = false;
    private Camera focusCamera;
    private Volume mainVolume;
    private Volume focusVolume;


    public void SetBlur(bool on)
    {
        if (on)
            BlurCamera();
        else
            UnblurCamera();
    }

    public void ToggleBlur(string target, bool on)
    {
        if (on)
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("Focused");
        else
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("Default");
    }

    private void BlurCamera()
    {
        Debug.Log("Blurring Camera");

        /* sets up the blur effect */

        if(GameObject.Find("FocusCamera"))
            focusCamera = GameObject.Find("FocusCamera").GetComponent<Camera>();
        else
            Debug.LogError("Focus Camera not found");


        // setting the culling Mask of main camera to exclude "focused" and "detached"
        
        Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Focused"));
        Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Detached"));
    
        // setting occlusion culling for main camera

        if (!Camera.main.useOcclusionCulling)
            Camera.main.useOcclusionCulling = true;
    
        // setting the Depth of Field for main camera

        if (Camera.main.GetComponent<Volume>())
        {
            mainVolume = Camera.main.GetComponent<Volume>();
            
            if (!mainVolume.enabled)
                mainVolume.enabled = true;

            if (mainVolume.profile.TryGet(out DepthOfField mainDepth))
            {
                mainDepth.active = true;
                mainDepth.focusDistance.overrideState = true;
                mainDepth.focusDistance.value = 0.9f;
                mainDepth.focalLength.overrideState = true;
                mainDepth.focalLength.value = 80;
                mainDepth.aperture.overrideState = true;
                mainDepth.aperture.value = 32f;
            } else {
                Debug.LogWarning("Depth of Field not found in Main Camera Volume");
            }
        }

        // setting up the focus camera

        if (!focusCamera.enabled)
            focusCamera.enabled = true;

        // setting the culling mask of focus camera to only include "focused"

        focusCamera.cullingMask = 1 << LayerMask.NameToLayer("Focused");

        // setting occlusing culling for focus camera

        if (!focusCamera.useOcclusionCulling)
            focusCamera.useOcclusionCulling = true;
        
        // setting the Depth of Field for focus camera

        if(focusCamera.GetComponent<Volume>())
        {
            focusVolume = focusCamera.GetComponent<Volume>();

            if (!focusVolume.enabled)
                focusVolume.enabled = true;
            
            if (focusVolume.profile.TryGet(out DepthOfField focusDepth))
            {
                focusDepth.active = true;
                focusDepth.focusDistance.overrideState = true;
                focusDepth.focusDistance.value = 0.75f;
                focusDepth.focalLength.overrideState = true;
                focusDepth.focalLength.value = 30.7f;
                focusDepth.aperture.overrideState = true;
                focusDepth.aperture.value = 5.6f;
            } else {
                Debug.LogWarning("Depth of Field not found in Focus Camera Volume");
            }
        }

        // ToDo setup controllers here instead of editor

        //GameObject.Find("GazeGuidingController").GetComponent<ChangeLayer>().setLayer("Focused");
        //GameObject.Find("Clipboardstand").GetComponent<ChangeLayer>().setLayer("Focused");

        blur = true;
    }


    private void UnblurCamera()
    {
        Debug.Log("Unblurring Camera");
        // disable the Volume on main camera
        if(Camera.main.GetComponent<Volume>().enabled)
            Camera.main.GetComponent<Volume>().enabled = false;

        // disable focusd camera
        if (focusCamera.enabled)
            focusCamera.GetComponent<Volume>().enabled = false;

        //GameObject.Find("GazeGuidingController").GetComponent<ChangeLayer>().setLayer("Default");
        //GameObject.Find("Clipboardstand").GetComponent<ChangeLayer>().setLayer("Default");

        blur = false;
    }

}