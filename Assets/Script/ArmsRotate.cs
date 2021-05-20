using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------
// Comando para os braços do player
//---------------------

public class ArmsRotate : MonoBehaviour
{
    public static ArmsRotate Instance;
    public GameObject L, R;
    public Rigidbody rdb;

    //---------------------
    // Rotação dos braços
    //---------------------

    void Update()
    {
        Instance = this;
        transform.Rotate(new Vector3(0, 0, -350 * Time.deltaTime));
    }

    //---------------------
    // Ajusta as posições e tamanho dos braços após serem criados
    //---------------------

    public void GrowUp()
    {

        R.transform.localScale = new Vector3(2, 2, 0.5f);
        R.transform.Rotate(new Vector3(0, 0, 180));
        L.transform.Rotate(new Vector3(0, 0, 0));
        L.transform.localScale = new Vector3(2, 2, 0.5f);

    }

    //---------------------
    // Reajuste dos braços após serem substituidos
    //---------------------

    public void GoDown()
    {
        R.transform.localScale = new Vector3(1, 1, 1);
        R.transform.Rotate(new Vector3(0, 0, 0));
        L.transform.Rotate(new Vector3(0, 0, 0));
        L.transform.localScale = new Vector3(1, 1, 1);
    }

}
