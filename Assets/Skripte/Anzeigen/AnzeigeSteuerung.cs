using TMPro;
using UnityEngine;


public class AnzeigeSteuerung : MonoBehaviour
{
    [Space(10)]
    
    public float start_Number = 0;
    public float end_Number = 2000;
    public string komponente = "WP1";
    
    [Space(30)]
    
    public float CHANGEpercentage = 0f;

    [Space(10)]
    
    public MeshFilter toManiuplateNORMAL;
    private Mesh meshNORMAL;
    private Vector3[] originalVertices;
    private float lastPercentage;
    public int percentage = 50;
    private int minIndex1;
    private int minIndex2;
    private int maxIndex;

    public MeshFilter toManiuplateORANGE;
    private Mesh meshORANGE;
    private Vector3[] originalVertices2;
    private float lastPercentage2;
    public int percentage2 = 50;
    private int minIndex12;
    private int minIndex22;
    private int maxIndex2;

    public MeshFilter toManiuplateRED;
    private Mesh meshRED;
    private Vector3[] originalVertices3;
    private float lastPercentage3;
    public int percentage3 = 50;
    private int minIndex13;
    private int minIndex23;
    private int maxIndex3;
    
    public bool isDIGITAL = false;
    
    
    void Start()
    {
        InitializeText(komponente, start_Number, end_Number);
        
        InitializeMesh(toManiuplateNORMAL, ref meshNORMAL, ref originalVertices, ref minIndex1, ref minIndex2, ref maxIndex, ref lastPercentage, percentage);
        InitializeMesh(toManiuplateORANGE, ref meshORANGE, ref originalVertices2, ref minIndex12, ref minIndex22, ref maxIndex2, ref lastPercentage2, percentage2);
        InitializeMesh(toManiuplateRED, ref meshRED, ref originalVertices3, ref minIndex13, ref minIndex23, ref maxIndex3, ref lastPercentage3, percentage3);
        
        
        setBar(0);
        setBar(1);
        setBar(2);
        
        UpdateBars();
    }

    void Update()
    {
        if(isDIGITAL) DigitalText.text = Mathf.FloorToInt(CHANGEpercentage * (end_Number / 100f)).ToString();;
        
        if (CHANGEpercentage != lastPercentage || CHANGEpercentage != lastPercentage2 || CHANGEpercentage != lastPercentage3)
        {
            UpdateBars();
        }
    }

    private TextMeshPro DigitalText;
    
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

    private bool isBar1Reset = false;
    private bool isBar2Reset = false;
    
    private void UpdateBars()
    {
        CHANGEpercentage = Mathf.Clamp(CHANGEpercentage,0 ,100);
        
        if (CHANGEpercentage <= percentage)
        {
            setBar(0);
        }

        if (CHANGEpercentage >= percentage && CHANGEpercentage <= percentage3)
        {
            setBar(1);
            isBar1Reset = false;
        }
        else if (CHANGEpercentage < percentage && !isBar1Reset)
        {
            ResetBar(1);
            isBar1Reset = true;
        }

        if (CHANGEpercentage >= percentage2 && CHANGEpercentage <= percentage3)
        {
            setBar(2);
            isBar2Reset = false;
        }
        else if (CHANGEpercentage < percentage2 && !isBar2Reset)
        {
            ResetBar(2);
            isBar2Reset = true;
        }
    }
    
    private void ResetBar(int ID)
    {
        Mesh mesh = null;
        switch (ID)
        {
            case 1:
                mesh = meshORANGE;
                break;
            case 2:
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
                vertices = originalVertices.Clone() as Vector3[];
                mesh = meshNORMAL;
                minIndex1 = this.minIndex1;
                minIndex2 = this.minIndex2;
                maxIndex = this.maxIndex;
                percentage = this.percentage;
                break;
            case 1:
                vertices = originalVertices2.Clone() as Vector3[];
                mesh = meshORANGE;
                minIndex1 = minIndex12;
                minIndex2 = minIndex22;
                maxIndex = maxIndex2;
                percentage = percentage2;
                break;
            case 2:
                vertices = originalVertices3.Clone() as Vector3[];
                mesh = meshRED;
                minIndex1 = minIndex13;
                minIndex2 = minIndex23;
                maxIndex = maxIndex3;
                percentage = percentage3;
                break;
        }

        if (CHANGEpercentage < percentage)
        {
            t = 1 - (CHANGEpercentage / 100f);
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
                lastPercentage = CHANGEpercentage;
                break;
            case 1:
                lastPercentage2 = CHANGEpercentage;
                break;
            case 2:
                lastPercentage3 = CHANGEpercentage;
                break;
        }
    }

    private int x;
}