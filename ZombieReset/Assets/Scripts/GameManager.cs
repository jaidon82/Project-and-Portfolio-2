using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance;
    public GameObject player;
    public playerController playerScript;

    [SerializeField] GameObject menuPause;
    void Start()
    {
       Instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript= player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
