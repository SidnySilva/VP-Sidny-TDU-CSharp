using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eu pretendia nomear o using por aqui mas, quando vi
//o nome do script era o mesmo do comando


public class SceneManager : MonoBehaviour
{
    //---------------------
    // Controle de cena
    //---------------------

    public void LoadScene(string A)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(A);
    }
}
