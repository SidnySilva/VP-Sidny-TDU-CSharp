using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------
// Controles gerais
//---------------------

public class Controller : MonoBehaviour
{

    public float coinCollect = 0;
    public int Counter = 0, PlayerID, levelID;

    public Text coinQuantity;

    public Transform Spawner;
    public GameObject[] Players, Panel, Level;


    void Update()
    {
        //---------------------
        // Mostra a quantidade de moedas coletadas
        //---------------------
        coinQuantity.text = coinCollect.ToString();
    }

    //---------------------
    // Seleção do estilo do player
    //---------------------
    public void PlayerSelect(int id)
    {
        if (id == 0)
        {
            PlayerID = id;
        }
        else if (id == 1)
        {
            PlayerID = id;
        }
        else if (id == 2)
        {
            PlayerID = id;
        }
        Panel[0].SetActive(false);
        Panel[1].SetActive(true);
    }

    //---------------------
    // Seleção da fase e Instancia da fase
    //---------------------

    public void FaseSelect(int id)
    {
        if (id == 0)
        {
            levelID = id;
            Vector3 levelPos = new Vector3(Spawner.position.x - 2, Spawner.position.y - 3, Spawner.position.z);
            Instantiate(Level[levelID], levelPos, Quaternion.identity);

        }
        else if (id == 1)
        {
            levelID = id;
            Vector3 levelPos = new Vector3(Spawner.position.x + 10, Spawner.position.y - 3, Spawner.position.z + 2f);
            Instantiate(Level[levelID], levelPos, Quaternion.identity);

        }
        else if (id == 2)
        {
            levelID = id;
            Vector3 levelPos = new Vector3(Spawner.position.x + 20, Spawner.position.y - 3, Spawner.position.z + 2.5f);
            Instantiate(Level[levelID], levelPos, Quaternion.identity);

        }

        //---------------------
        // Instancia o Player
        //---------------------

        Panel[1].SetActive(false);
        Vector3 SpawnPlayer = new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z);
        Instantiate(Players[PlayerID], SpawnPlayer, Quaternion.identity);
        Panel[2].SetActive(true);
    }

    //---------------------
    // Destroi a fase após a morte do player
    //---------------------

    public void UnDo()
    {
        GameObject a = GameObject.Find("Fase1(Clone)");
        GameObject b = GameObject.Find("Fase2(Clone)");
        GameObject c = GameObject.Find("Fase3(Clone)");
        if(a != null)
        {
            Destroy(a.gameObject);
        }
        else if (b != null)
        {
            Destroy(b.gameObject);
        }
        else if (c != null)
        {
            Destroy(c.gameObject);
        }

    }

}
