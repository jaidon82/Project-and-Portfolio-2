using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public playerController playerScript;

    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuWin;
    public bool isPaused;
    float timescaleOrig;
    public int gameGoal;
    void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            statePaused();
            menuActive = menuPause;

            menuActive.SetActive(isPaused);
        }


    }

    public void statePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        
    }

    public void updateGameGoal(int num)
    {
        gameGoal += num;
        if (gameGoal <= 0)
        {
            statePaused();
            menuActive = menuWin;
            menuActive.SetActive(true);

        }
    }


}
