using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private AudioSource finishSoundl;

    private bool leverCompleted = false;
    // Start is called before the first frame update
    void Start()
    {
        finishSoundl = GetComponent<AudioSource>();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "player" && !leverCompleted)
        {
            finishSoundl.Play();
            leverCompleted = true;
            Invoke("completeLevel", 2);
        }
    }

    private void completeLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
