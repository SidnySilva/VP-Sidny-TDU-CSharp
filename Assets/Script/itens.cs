using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------
// Configuração de Itens
//---------------------

public class itens : MonoBehaviour
{
    Controller c;
    void Start()
    {
        c = FindObjectOfType(typeof(Controller)) as Controller;
    }

    //---------------------
    // Rotaciona a moeda
    //---------------------

    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0));
    }

    //---------------------
    // Adiciona moeda coletada ao score
    //---------------------

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.playSound(sound.Coin);
        c.coinCollect++;
        Destroy(this.gameObject);
    }
}
