using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------
// Script do jogador
//---------------------

public class Player : MonoBehaviour
{  
    public Transform lLeg, rLeg;
    [SerializeField]
    private GameObject miniPlayer;
    private ConstantForce consForce;

    public bool Legs, isGrounded;
    public float impulse, count, countD;
    
    CreateLegs CL;
    Controller c;
    void Start()
    {
        consForce = GetComponent<ConstantForce>();
        consForce.enabled = false;

        c = FindObjectOfType(typeof(Controller)) as Controller;

        //Recebe variaveis de outros scripts para facilitar a utilização dos objetos
        CL = FindObjectOfType(typeof(CreateLegs)) as CreateLegs;
        CreateLegs.gotLegs = false;
        CL.leftLegPivot = lLeg;
        CL.rightLegPivot = rLeg;
    }

        //---------------------
        // Movimentação do player
        //---------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Solo")
        {
            isGrounded = true;
            consForce.enabled = false;
        }
        //---------------------
        // Player recebe sua vitória
        //---------------------

        if (collision.gameObject.tag == "Finisher")
        {
            AudioManager.playSound(sound.Win);
            Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            for (int i = 0; i <= 100; i++)
            {
                Instantiate(miniPlayer, pos, Quaternion.identity);
            }
            StartCoroutine(WaitPosWin());
        }

        //---------------------
        // Player explode ao cair da fase
        //---------------------

        else if (collision.gameObject.tag == "Death" && countD == 0)
        {
            AudioManager.playSound(sound.Pop);
            Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            for (int i = 0; i <= 100; i++)
            {
                Instantiate(miniPlayer, pos, Quaternion.identity);
            }
            transform.localScale = new Vector3(0,0,0);
            countD++;
            StartCoroutine(WaitPosDeath());
        }
    }

    //---------------------
    // Força para o movimento do Jogador
    //---------------------

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        if (CreateLegs.gotLegs == true && isGrounded == false)
        {
            print("colidiu");
            consForce.enabled = true;
        }
    }

    //---------------------
    // Consede a oportunidade de jogar novamente em caso de vitória ou derrota
    //---------------------

    //VITÓRIA

    IEnumerator WaitPosWin()
    {
        yield return new WaitForSeconds(2);
        c.Panel[2].SetActive(false);
        c.Panel[3].SetActive(true);
        Destroy(this.gameObject);

    }

    //DERROTA

    IEnumerator WaitPosDeath()
    {
        yield return new WaitForSeconds(2); 
        c.Panel[2].SetActive(false);
        c.Panel[4].SetActive(true);
        Destroy(this.gameObject);
    }
}
