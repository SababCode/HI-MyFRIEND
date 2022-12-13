using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject EndingPanel;
    public static bool endgame = false;
    private void Awake()
    {
        EndingPanel.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EndingPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ChoiceEnding(int i)
    {
        endgame = true;
        SceneManager.LoadScene(3+i);
    }
    public void CloseEnding()
    {
        EndingPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
