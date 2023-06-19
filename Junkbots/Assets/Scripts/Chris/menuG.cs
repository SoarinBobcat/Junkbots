using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuG : MonoBehaviour
{
    [SerializeField] private Button resBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private GameObject pan;
    [SerializeField] public int sceneIndex = 0;

    public bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        pan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pan.SetActive(true);
            pause = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (pause)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    public void resume()
    {
        pan.SetActive(false);
        pause = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
