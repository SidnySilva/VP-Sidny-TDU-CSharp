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
    private Rigidbody rdb;
    public bool Legs, isGrounded;
    public float impulse, count, countD;
    
    CreateLegs CL;
    Controller c;
    void Start()
    {
        consForce = GetComponent<ConstantForce>();
        consForce.enabled = false;
        rdb = GetComponent<Rigidbody>();

        //Recebe variaveis de outros scripts para facilitar a utiliza��o dos objetos
        c = FindObjectOfType(typeof(Controller)) as Controller;
        CL = FindObjectOfType(typeof(CreateLegs)) as CreateLegs;

        CreateLegs.gotLegs = false;
        CL.leftLegPivot = lLeg;
        CL.rightLegPivot = rLeg;
    }

    //---------------------
    // Movimenta��o do player
    //---------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Solo")
        {
            if(CreateLegs.gotLegs == true)
            {
                isGrounded = true;
                rdb.AddRelativeForce(new Vector3(0.5f, 0, 0), ForceMode.Impulse); //Da o Impulso para o player se movimentar
                consForce.enabled = false;
            }
            
        }
        //---------------------
        // Player recebe sua vit�ria
        //---------------------

        if (collision.gameObject.tag == "Finisher")
        {
            AudioManager.playSound(sound.Win); // Som de vit�ria
            Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z); //Capitura as posi��es atuais do Player

            //Gera varios miniBlocos do jogador atual
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
            AudioManager.playSound(sound.Pop); //Som de Explos�o
            Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z); //Capitura as posi��es atuais do Player

            //Gera varios miniBlocos do jogador atual
            for (int i = 0; i <= 100; i++)
            {
                Instantiate(miniPlayer, pos, Quaternion.identity);
            }
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); // Encolhe o player para permitir os outros comandos agirem antes de destruir o mesmo
            countD++;
            StartCoroutine(WaitPosDeath());
        }
    }

    //---------------------
    // For�a para o movimento do Jogador
    //---------------------

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        if (CreateLegs.gotLegs == true && isGrounded == false)
        {
            print("colidiu");
            rdb.AddRelativeForce(new Vector3(2, 0, 0), ForceMode.Impulse);
        }
    }

    //---------------------
    // Consede a oportunidade de jogar novamente em caso de vit�ria ou derrota
    //---------------------

    //VIT�RIA

    IEnumerator WaitPosWin()
    {
        //Congelar� o player e trar� a tela de Vit�ria
        rdb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(1);
        c.Panel[2].SetActive(false);
        c.Panel[3].SetActive(true);
        c.UnDo();
        Destroy(this.gameObject);
    }

    //DERROTA

    IEnumerator WaitPosDeath()
    {
       //Congelar� o player e trar� a tela de Game Over
        rdb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(1);
        c.Panel[2].SetActive(false);
        c.Panel[4].SetActive(true);
        c.UnDo();
        Destroy(this.gameObject);
    }
}
