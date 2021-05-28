using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------
// Captura o toque e cria as pernas para o jogador
//---------------------

public class CreateLegs : MonoBehaviour
{
    public Transform leftLegPivot, rightLegPivot;
    public GameObject drawLine, drawPref;
    [SerializeField]
    private GameObject leftLeg, rightLeg;

    public Material legColor;
    private LineRenderer drawRend;

    private List<Vector3> drawPoints;
    private List<GameObject> createLegs;

    public bool isDrawing, ownLegs;
    public static bool gotLegs;

    public float drawSize = 0.1f;
    private float drawCD;
    private float rayDepth = 1;

    private int drawIndex;


    void Start()
    {
        ownLegs = false;
        drawPoints = new List<Vector3>();
        createLegs = new List<GameObject>();
        drawIndex = 0;
    }

    void Update()
    {
        gotLegs = ownLegs;
        drawCD -= Time.deltaTime;

        //---------------------
        // Verifica se o jogador esta desenhando dentro do painel de desenho
        //---------------------

        var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Ray, out hit))
        {
            
            if (hit.collider.tag == "Painel")
            {

                //---------------------
                // Inicio do processo de criação onde define as coordenadas do primeiro ponto para "Pivot"
                //---------------------

                if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Time.timeScale = 0.01f;
                    ownLegs = false;
                    isDrawing = true;

                    GameObject g = Instantiate(drawPref, Vector3.zero, drawPref.transform.rotation);
                    drawRend = g.GetComponent<LineRenderer>();
                    drawRend.positionCount = 2;
                    drawRend.startWidth = drawSize;
                    drawRend.endWidth = drawSize;
                    drawPoints.Add(GetMousePosition());
                    drawPoints.Add(GetMousePosition());
                    drawRend.SetPosition(drawIndex, drawPoints[drawIndex]);
                    drawIndex++;
                    drawRend.SetPosition(drawIndex, drawPoints[drawIndex]);
                    drawIndex++;
                    drawCD = 0.002f;
                }

                //---------------------
                // Processo de desenhar as pernas
                //---------------------

                else if (Input.GetMouseButton(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && isDrawing && drawCD < 0)
                {
                    CreateLeg();
                    drawCD = 0.002f;  
                }

                //---------------------
                // finalização e criação das pernas
                //---------------------

                else if (Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && isDrawing)
                {
                    createLegs = new List<GameObject>();
                    ownLegs = true;
                    EraseLegs();
                    ArmsRotate.Instance.GoDown();
                    CreateLeg();
                    drawCD = 0.002f;
                    CubeCreator();
                    OneForAll();
                    ArmsRotate.Instance.GrowUp();
                    EraseDraw();
                    Time.timeScale = 1;
                }
            }
            else
            {
                if (Input.GetMouseButton(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Time.timeScale = 1;
                    EraseLegs();
                    EraseDraw();
                }
            }
        }
    }

    //---------------------
    // Metodo que cria as pernas
    //---------------------

    void CreateLeg()
    {
        drawRend.positionCount++;
        drawPoints.Add(GetMousePosition());
        drawRend.SetPosition(drawIndex, drawPoints[drawIndex]);
        drawIndex++;
    }

    //---------------------
    // Metodo que unifica o mesh de todos gameObjects da perna
    //---------------------

    void OneForAll()
    {
        //Cria o objeto que sera a perna
        GameObject obj = new GameObject();
        obj.AddComponent<MeshFilter>();

        List<MeshFilter> meshFilters = new List<MeshFilter>();

        //Adiciona um MeshFilter em todos os gameObjects criados
        foreach (GameObject g in createLegs)
        {
            g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, g.transform.position.z);
            meshFilters.Add(g.GetComponent<MeshFilter>());
        }

        CombineInstance[] unite = new CombineInstance[meshFilters.Count];

        int i = 0;
        while (i < meshFilters.Count)
        {
            unite[i].mesh = meshFilters[i].sharedMesh;
            unite[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        //unificação do mesh filter
        obj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        obj.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(unite, true);
        obj.transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        obj.transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        obj.transform.GetComponent<MeshFilter>().mesh.Optimize();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = legColor;
        obj.AddComponent<MeshCollider>();
        obj.GetComponent<MeshCollider>().convex = true;

        //Cria a segunda perna sendo uma cópia do primeiro
        GameObject obj2 = Instantiate(obj);
        obj2.transform.position = new Vector3(obj2.transform.position.x, obj2.transform.position.y, obj2.transform.position.z - 1.2f);
        obj.transform.SetParent(leftLegPivot);
        obj2.transform.SetParent(rightLegPivot);

        leftLeg = obj;
        rightLeg = obj2;
    }

    //---------------------
    // Metodo que cria a forma das pernas 
    //---------------------

    void CubeCreator()
    {
        Vector3 LastPoint;
        LastPoint = leftLegPivot.transform.position;

        for (int i = 0; i < drawRend.positionCount; i++)
        {
            if (i + 1 < drawRend.positionCount)
            {
                Vector3 spawnPos = drawRend.GetPosition(i + 1) - drawRend.GetPosition(i);
                GameObject g = Instantiate(drawPref, LastPoint + spawnPos, Quaternion.identity);
                LastPoint = g.transform.position;
                createLegs.Add(g);
            }
        }
    }

    //---------------------
    // Metodo que apaga o desenho da tela de desenho
    //---------------------

    public void EraseDraw()
    {
        isDrawing = false;
        drawPoints = new List<Vector3>();
        drawIndex = 0;
    }

    //---------------------
    // Metodo que troca as pernas
    //---------------------

    public void EraseLegs()
    {
        Destroy(leftLeg);
        Destroy(rightLeg);
        Destroy(drawRend.gameObject);
    }

    //---------------------
    // Captura da posição do Mouse 
    //---------------------

    Vector3 GetMousePosition()
    {
        Ray ray;
        Vector3 MousePos;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        MousePos = ray.origin + (ray.direction * rayDepth);
        return MousePos;
    }
}
