using System.Collections;
using TMPro;
using UnityEngine;


public class AnzeigeSteuerung5 : MonoBehaviour
{
    
    //Nicht anfassen
    
    [Space(10)]
    
    /// <param name="start_Number"> specifies the lower bound of the display</param>
    public float start_Number = 0;
    /// <param name="end_Number"> specifies the upper bound of the display</param>
    public float end_Number = 2000;
    /// <param name="komponente"> specifies the component corresponding to the display</param>
    public string komponente = "WP1";
    
    [Space(30)]
    
    /// <param name="CHANGEpercentage"> specifies the current percentage to be indicated by the display's bar</param>
    public float CHANGEpercentage = 0f;

    [Space(10)]
    
    /// <param name="toManipulate8"> is a MeshFilter component of the red lower mesh</param>
    public MeshFilter toManiuplate8;
    /// <param name="mesh8">is a Mesh component of the red lower mesh</param>
    private Mesh mesh8;
    /// <param name="originalVertices8"> is an array of Vector3s containing the original vertices of the red lower mesh</param>
    private Vector3[] originalVertices8;
    /// <param name="lastPercentage8"> specifies the percentage indicated by the red lower bar in the previous frame</param>
    private float lastPercentage8;
    /// <param name="percentage8"> specifies the current percentage te bo indicated by the red lower bar</param>
    public int percentage8 = 50;
    /// <param name="minIndex18"> specifies the index of the first minimum vertex of the red lower mesh (lower vertex)</param>
    private int minIndex18;
    /// <param name="minIndex28"> specifies the index of the second minimum vertex of the red lower mesh (upper vertex)</param>
    private int minIndex28;
    /// <param name="maxIndex8"> specifies the index of the maximum vertex of the red lower mesh</param>
    private int maxIndex8;
    
    /// <param name="toManipulate9"> is aMeshFilter component of the lower orange mesh</param>
    public MeshFilter toManiuplate9;
    /// <param name="mesh9"> is a Mesh component of the lower orange mesh</param>
    private Mesh mesh9;
    /// <param name="originalVertices9"> is an array of Vector3s containing the original vertices of the lower orange mesh</param>
    private Vector3[] originalVertices9;
    /// <param name="lastPercentage9"> specifies the percentage indicated by the lower orange bar in the previous frame</param>
    private float lastPercentage9;
    /// <param name="percentage9"> specifies the current percentage to be indicated by the lower orange bar</param>
    public int percentage9 = 50;
    /// <param name="minIndex19"> specifies the index of the first minimum vertex of the lower orange mesh (lower vertex)</param>
    private int minIndex19;
    /// <param name="minIndex29"> specifies the index of the second minimum vertex of the lower orange mesh (upper vertex)</param>
    private int minIndex29;
    /// <param name="maxIndex9"> specifies the index of the maximum vertex of the lower orange mesh</param>
    private int maxIndex9;
    
    /// <param name="toManiuplateNORMAL"> is a MeshFilter component of the normal Mesh</param>
    public MeshFilter toManiuplateNORMAL;
    /// <param name="meshNORMAL"> is a Mesh component of the normal Mesh</param>
    private Mesh meshNORMAL;
    /// <param name="originalVertices"> is an array of Vector3s containing the original vertices of the normal Mesh</param>
    private Vector3[] originalVertices;
    /// <param name="lastPercentage"> specifies the percentage indicated by the normal bar in the previous frame</param>
    private float lastPercentage;
    /// <param name="percentage"> specifies the current percentage to be indicated by the normal bar</param>
    public int percentage = 50;
    /// <param name="minIndex1"> specifies the index of the first minimum vertex of the normal Mesh (lower vertex)</param>
    private int minIndex1;
    /// <param name="minIndex2"> specifies the index of the second minimum vertex of the normal Mesh (upper vertex)</param>
    private int minIndex2;
    /// <param name="maxIndex"> specifies the index of the maximum vertex of the normal Mesh</param>
    private int maxIndex;

     /// <param name="toManiuplateORANGE"> is a MeshFilter component of the upper orange Mesh</param>
    public MeshFilter toManiuplateORANGE;
    /// <param name="meshORANGE"> is a Mesh component of the upper orange Mesh</param>
    private Mesh meshORANGE;
    /// <param name="originalVertices2"> an array of Vector3s containing the original vertices of the upper orange Mesh</param>
    private Vector3[] originalVertices2;
    /// <param name="lastPercentage2"> specifies the percentage indicated by the upper orange bar in the previous frame</param>
    private float lastPercentage2;
    /// <param name="percentage2"> specifies the current percentage to be indicated by the upper orange bar</param>
    public int percentage2 = 50;
    /// <param name="minIndex12">specifies the index of the first minimum vertex of the upper orange Mesh (lower vertex)</param>
    private int minIndex12;
    /// <param name="minIndex22">specifies the index of the second minimum vertex of the upper orange  Mesh (upper vertex)</param>
    private int minIndex22;
    /// <param name="maxIndex2"> specifies the index of the maximum vertex of the upper orange  Mesh</param>
    private int maxIndex2;

/// <param name="toManiuplateRED"> is a MeshFilter component of the upper red Mesh</param>
    public MeshFilter toManiuplateRED;
    /// <param name="meshRED">is a Mesh component of the upper red Mesh</param>
    private Mesh meshRED;
    /// <param name="originalVertices3"> is an array of Vector3s containing the original vertices of the upper red Mesh</param>
    private Vector3[] originalVertices3;
    /// <param name="lastPercentage3"> specifies the percentage indicated by the upper red bar in the previous frame</param>
    private float lastPercentage3;
    /// <param name="percentage3"> specifies the current percentage to be indicated by the upper red bar</param>
    public int percentage3 = 50;
    /// <param name="minIndex13"> specifies the index of the first minimum vertex of the upper red Mesh (lower vertex)</param>
    private int minIndex13;
    /// <param name="minIndex23"> specifies the index of the second minimum vertex of the upper red  Mesh (upper vertex)</param>
    private int minIndex23;
    /// <param name="maxIndex3"> specifies the index of the maximum vertex of the upper red Mesh</param>
    private int maxIndex3;

    /// <param name="isDIGITAL">boolean specifying whether the display is digital</param>
    public bool isDIGITAL = false;
    
    /// <summary>
    /// This method initialises the display. It initiates the initialisation of the meshes and the bars, and starts the animation.
    /// </summary>
    void Start()
    {
        InitializeText(komponente, start_Number, end_Number);
        
        InitializeMesh(toManiuplate8, ref mesh8, ref originalVertices8,ref minIndex18, ref minIndex28,ref maxIndex8, ref lastPercentage8, percentage8);
        InitializeMesh(toManiuplate9, ref mesh9, ref originalVertices9,ref minIndex19, ref minIndex29,ref maxIndex9, ref lastPercentage9, percentage9);
        InitializeMesh(toManiuplateNORMAL, ref meshNORMAL, ref originalVertices, ref minIndex1, ref minIndex2, ref maxIndex, ref lastPercentage, percentage);
        InitializeMesh(toManiuplateORANGE, ref meshORANGE, ref originalVertices2, ref minIndex12, ref minIndex22, ref maxIndex2, ref lastPercentage2, percentage2);
        InitializeMesh(toManiuplateRED, ref meshRED, ref originalVertices3, ref minIndex13, ref minIndex23, ref maxIndex3, ref lastPercentage3, percentage3);
        
        setBar(0);
        setBar(1);
        setBar(2);
        setBar(3);
        setBar(4);
        
        StartCoroutine(AnimatePercentage());
    }

    /// <param name="CHANGEpercentageanimate"> specifies the percentage indicated by display's bar in the current frame</param>
    private float CHANGEpercentageanimate;

    /// <summary>
    /// This method animates the display's bar in three steps. First the bar is set to indicate 100%. Then the bar is set to inndicate 0%. Finally the bar is set to indicate the actual value CHANGEpercentage.
    /// </summary>
    private IEnumerator AnimatePercentage()         //initalisiere alle meshes
    {
        CHANGEpercentageanimate = CHANGEpercentage;
        float duration = 1.0f; // Duration for the animation
        float elapsedTime = 0f;
        
        // step 1: animate the bars from 0 to 100, spiking the display within the specified duration i.e. 2 seconds

        while (elapsedTime < duration)
        {
            CHANGEpercentage = Mathf.Lerp(0, 100, elapsedTime / duration);
            UpdateBars();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // step 2: animate the bars from 100 to 0, dropping the display within the specified duration i.e. 2 seconds

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            CHANGEpercentage = Mathf.Lerp(100, 0, elapsedTime / duration);
            UpdateBars();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // step 3: animate the bars to the desired CHANGEpercentage within the specified duration i.e. 2 seconds

        float targetPercentage = CHANGEpercentageanimate; // Replace with the desired value
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            CHANGEpercentage = Mathf.Lerp(0, targetPercentage, elapsedTime / duration);
            UpdateBars();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set to the final desired CHANGEpercentage
        CHANGEpercentage = targetPercentage;
        UpdateBars();
    }

    /// <summary>
    /// This method updates the display's bar if the current frame's percentage is different from last frame's percentage.
    /// </summary>
    void Update()
    {
        if(isDIGITAL) DigitalText.text = Mathf.FloorToInt(CHANGEpercentage * (end_Number / 100f)).ToString();;
        
        if (CHANGEpercentage != lastPercentage || CHANGEpercentage != lastPercentage2 || CHANGEpercentage != lastPercentage3
            || CHANGEpercentage != lastPercentage8 || CHANGEpercentage != lastPercentage9)
        {
            UpdateBars();
        }
    }
    
    /// <param name="DigitalText"> is a reference to a TextMeshPro component</param>    
    private TextMeshPro DigitalText;
    
    /// <summary>
    /// This method labels the scale of the display.
    /// </summary>
    /// <param name="komponente"> contains the name of the component the display corresponds to</param>
    /// <param name="startNumber">specifies the lower bound for the display</param>
    /// <param name="endNumber"> specifies the upper bound of the display</param>
    private void InitializeText(string komponente, float startNumber, float endNumber)
    {
        Transform anzeigeBasic = transform.Find("AnzeigeBasic");
        if (anzeigeBasic == null) return;

        SetText(anzeigeBasic, "Text_komponente", komponente);
        SetText(anzeigeBasic, "Text_StartNummer", startNumber.ToString());
        SetText(anzeigeBasic, "Text_NummerEnde", endNumber.ToString());

        for (int i = 1; i <= 7; i++)
        {
            float fraction = (i / 8f) * endNumber;
            SetText(anzeigeBasic, $"Text_Nummer{i}-8", fraction.ToString());
        }

        if (isDIGITAL)
        {
            DigitalText = anzeigeBasic.transform.Find("TEXT_DIGITAL").gameObject.GetComponent<TextMeshPro>();
        }
    }

    
    /// <summary>
    /// This method sets a text to a child component of the display.
    /// </summary>
    /// <param name="parent"> is a Transform of a AnzeigeBasic object</param>
    /// <param name="childName"> contains the name of a subcomponent of an AnzeigeBasic object</param>
    /// <param name="text"> contains the text that will be set to a subcomponent of an AnzeigeBasic object </param>
    private void SetText(Transform parent, string childName, string text)
    {
        Transform child = parent.transform.Find(childName);
        if (child != null)
        {
            TextMeshPro textMeshPro = child.gameObject.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                textMeshPro.text = text;
            }
        }
    }
    
    /// <summary>
    /// This method initializes the meshes of the display's bar.
    /// </summary>
    /// <param name="meshFilter"> is the MeshFilter component of the bar</param>
    /// <param name="mesh"> is the Mesh component of the bar</param>
    /// <param name="originalVertices"> is an array of Vector3s containing the original vertices of the Mesh</param>
    /// <param name="minIndex1"> specifies the index of the first minimum vertex of the Mesh (lower vertex)</param>
    /// <param name="minIndex2"> specifies the index of the second minimum vertex of the Mesh (upper vertex)</param>
    /// <param name="maxIndex"> specifies the index of the maximum vertex of the Mesh</param>
    /// <param name="lastPercentage"> specifies the previous percentage indicated by the bar</param>
    /// <param name="percentage"> specifies the current percentage to be indicated bar</param>
    private void InitializeMesh(MeshFilter meshFilter, ref Mesh mesh, ref Vector3[] originalVertices, ref int minIndex1, ref int minIndex2, ref int maxIndex, ref float lastPercentage, int percentage)
    {
        if (meshFilter == null) return;

        mesh = meshFilter.mesh;
        originalVertices = mesh.vertices.Clone() as Vector3[];
        lastPercentage = percentage;

        minIndex1 = 0;
        minIndex2 = 1;
        maxIndex = 0;
        for (int i = 1; i < originalVertices.Length; i++)
        {
            if (originalVertices[i].x < originalVertices[minIndex1].x)
            {
                minIndex2 = minIndex1;
                minIndex1 = i;
            }
            else if (originalVertices[i].x < originalVertices[minIndex2].x)
            {
                minIndex2 = i;
            }

            if (originalVertices[i].x > originalVertices[maxIndex].x)
            {
                maxIndex = i;
            }
        }
    }

    /// <param name="isBar1Reset"> tracks whether the first bar has been reset </param>
    private bool isBar1Reset = false;
    /// <param name="isBar2Reset"> tracks whether the second bar has been reset </param>
    private bool isBar2Reset = false;
    /// <param name="isBar3Reset"> tracks whether the third bar has been reset </param>
    private bool isBar3Reset = false;
    /// <param name="isBar4Reset"> tracks whether the fourth bar has been reset </param>
    private bool isBar4Reset = false;
    
    /// <summary>
    /// This method updates the display's bar.
    /// </summary>  
    private void UpdateBars()
    {
        CHANGEpercentage = Mathf.Clamp(CHANGEpercentage,0 ,100);
        
        if (Mathf.Abs(animatePercentage - CHANGEpercentage) < 1)
        {
            return;
        }
        else if (animatePercentage < CHANGEpercentage)
        {
            animatePercentage++;
        }
        else if (animatePercentage > CHANGEpercentage)
        {
            animatePercentage--;
        }
        
        if (animatePercentage <= percentage8)
        {
            setBar(0);
        }

        if (animatePercentage >= percentage8 && animatePercentage <= percentage9)
        {
            setBar(1);
            isBar1Reset = false;
        }
        else if (animatePercentage < percentage8 && !isBar1Reset)
        {
            ResetBar(1);
            isBar1Reset = true;
        }

        if (animatePercentage >= percentage9 && animatePercentage <= percentage)
        {
            setBar(2);
            isBar2Reset = false;
        }
        else if (animatePercentage < percentage9 && !isBar2Reset)
        {
            ResetBar(2);
            isBar2Reset = true;
        }
        
        if (animatePercentage >= percentage && animatePercentage <= percentage2)
        {
            setBar(3);
            isBar3Reset = false;
        }
        else if (animatePercentage < percentage && !isBar3Reset)
        {
            ResetBar(3);
            isBar3Reset = true;
        }
        
        if (animatePercentage >= percentage2 && animatePercentage <= percentage3)
        {
            setBar(4);
            isBar4Reset = false;
        }
        else if (animatePercentage < percentage2 && !isBar4Reset)
        {
            ResetBar(4);
            isBar4Reset = true;
        }
    }
    
    /// <summary>
    /// This method resets the display's bar by changing the mesh.
    /// </summary>
    /// <param name="ID"> specifies the bar to be reset</param> 
    private void ResetBar(int ID)
    {
        Mesh mesh = null;
        switch (ID)
        {
            case 0:
                mesh = mesh8;
                break;
            case 1:
                mesh = mesh9;
                break;
            case 2:
                mesh = meshNORMAL;
                break;
            case 3:
                mesh = meshORANGE;
                break;
            case 4:
                mesh = meshRED;
                break;
        }


        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].x = 0;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        
    }

    /// <param name="animatePercentage"> specifies the current value that is being animated in setBar()</param>
    private float animatePercentage;

    /// <summary>
    /// This method animates the display's bar by changing the mesh, vertices and indices.
    /// </summary>
    /// <param name="ID"> specifies the bar to be animated</param>
    private void setBar(int ID)
    {
            
        
        Vector3[] vertices = null;
        float t = 0;
        int minIndex1 = 0, minIndex2 = 0, maxIndex = 0;
        Mesh mesh = null;
        int percentage = 0;

        switch (ID)
        {
            case 0:
                vertices = originalVertices8.Clone() as Vector3[];
                mesh = mesh8;
                minIndex1 = this.minIndex18;
                minIndex2 = this.minIndex28;
                maxIndex = this.maxIndex8;
                percentage = this.percentage8;
                break;
            case 1:
                vertices = originalVertices9.Clone() as Vector3[];
                mesh = mesh9;
                minIndex1 = this.minIndex19;
                minIndex2 = this.minIndex29;
                maxIndex = this.maxIndex9;
                percentage = this.percentage9;
                break;
            case 2:
                vertices = originalVertices.Clone() as Vector3[];
                mesh = meshNORMAL;
                minIndex1 = this.minIndex1;
                minIndex2 = this.minIndex2;
                maxIndex = this.maxIndex;
                percentage = this.percentage;
                break;
            case 3:
                vertices = originalVertices2.Clone() as Vector3[];
                mesh = meshORANGE;
                minIndex1 = minIndex12;
                minIndex2 = minIndex22;
                maxIndex = maxIndex2;
                percentage = percentage2;
                break;
            case 4:
                vertices = originalVertices3.Clone() as Vector3[];
                mesh = meshRED;
                minIndex1 = minIndex13;
                minIndex2 = minIndex23;
                maxIndex = maxIndex3;
                percentage = percentage3;
                break;
        }

        if (animatePercentage < percentage)
        {
            t = 1 - (animatePercentage / 100f);
        }
        else
        {
            t = 1 - (percentage / 100f);
        }

        vertices[minIndex1].x = Mathf.Lerp(vertices[minIndex1].x, vertices[maxIndex].x, t);
        vertices[minIndex2].x = Mathf.Lerp(vertices[minIndex2].x, vertices[maxIndex].x, t);

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        switch (ID)
        {
            case 0:
                lastPercentage8 = animatePercentage;
                break;
            case 1:
                lastPercentage9 = animatePercentage;
                break;
            case 2:
                lastPercentage = animatePercentage;
                break;
            case 3:
                lastPercentage2 = animatePercentage;
                break;
            case 4:
                lastPercentage3 = animatePercentage;
                break;
        }
    }
    
    
}