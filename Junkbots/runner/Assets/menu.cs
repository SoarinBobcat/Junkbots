using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    [SerializeField] public int sceneIndex = 0;

    [SerializeField] private float bufferTime = 1f;


    [SerializeField] private Button playBtn;
    [SerializeField] private Button optiBtn;
    [SerializeField] private Button opti2Btn;
    [SerializeField] private Button quitBtn;

    private float timer = -1;
    private int currentButtonIndex = 2;

    private void Awake()
    {
        opt.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentButtonIndex)
        {
            case -1:
                currentButtonIndex = 2;
                break;
            case 2:
                playBtn.Select();
                //Debug.Log(currentButtonIndex);
                break;
            case 1:
                optiBtn.Select();
                //Debug.Log(currentButtonIndex);
                break;
            case 0:
                quitBtn.Select();
                //Debug.Log(currentButtonIndex);
                break;
            case 3:
                currentButtonIndex = 0;
                break;
        }

        if (timer < 0)
        {
            float move = Input.GetAxisRaw("Vertical");
            if (move > 0)
            {
                currentButtonIndex++;
            }
            else if (move < 0)
            {
                currentButtonIndex--;
            }
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= bufferTime)
            {
                timer = -1;
            }
        }
    }

    public void LoadNewGame()
    {
        if (currentButtonIndex == 2)
        {
            Debug.Log("play");
            SceneManager.LoadScene(sceneIndex);
        }
        currentButtonIndex = 2;
    }

    [TextArea]
    public string dialogue;
    [SerializeField] private GameObject opt;

    public void Options()
    {
        if (currentButtonIndex == 1)
        {
            Debug.Log("options");
            opt.SetActive(true);
        }
        currentButtonIndex = 1;
    }

    public void Options2()
    {
        Debug.Log("options OUT");
        opt.SetActive(false);
    }

    public void QuitGame()
    {
        if (currentButtonIndex == 0)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        currentButtonIndex = 0;
    }
}
