using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------
// Controle da camera
//---------------------

public class CameraControll : MonoBehaviour
{
    private Player Player;
    [SerializeField]
    public float speed = 9;

    //---------------------
    // Camera segue o Player
    //---------------------

    void Update()
    {
        Player = FindObjectOfType(typeof(Player)) as Player;
        if (Player != null)
        {
            Player = FindObjectOfType(typeof(Player)) as Player;
            float step = speed;
            Vector3 P = new Vector3(Player.transform.position.x, Player.transform.position.y + 3, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, P, step);
        }

    }
}
