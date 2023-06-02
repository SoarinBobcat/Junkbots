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
        }
    }

    public void resume()
    {
        pan.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
