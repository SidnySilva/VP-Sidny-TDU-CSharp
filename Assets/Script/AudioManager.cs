using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------
// Gerenciador de audios
//---------------------

public enum sound
{
    Coin, Pop, Win
}
public class AudioManager : MonoBehaviour
{
    public AudioClip coin, pop, win;
    private AudioSource audi;
    public static AudioManager instance;

    //---------------------
    // Preparação das variaveis para serem chamadas por outros scripts
    //---------------------
    void Start()
    {
        audi = GetComponent<AudioSource>();
        instance = this;
    }

    public static void playSound(sound currentSound)
    {
        switch (currentSound)
        {
            case sound.Coin:
                instance.audi.PlayOneShot(instance.coin); //Som da moeda
                break;
            case sound.Pop:
                instance.audi.PlayOneShot(instance.pop);//Som do estouro do player
                break;
            case sound.Win:
                instance.audi.PlayOneShot(instance.win);//Som de vitótia
                break;
        }
    }
}
