using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{

    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [Header("----- Player Stats -----")]
    [SerializeField] int speed;
    [SerializeField] int HP;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    int jumpCount;
    int HPOrig;

    Vector3 moveDir;
    Vector3 playerVel;

    [Header("------ Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    [Header("----- Player States -----")]
    [SerializeField] bool isShooting;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if(Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }


    }

    void move()
    {
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
           controller.Move(moveDir* speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && jumpCount<jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            playerVel.y = jumpSpeed;
        }
        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel *Time.deltaTime);


        if(controller.isGrounded)
        {
            jumpCount = 0; 
            playerVel = Vector3.zero;
        }

        if(Input.GetButtonDown("Crouch"))
        {
            crouch();
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            unCrouch();
        }
    }


    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if(dmg != null) 
            {
                dmg.takeDamage(shootDamage);

            }

        }

        yield return new WaitForSeconds(shootRate);
        isShooting=false;
    }
    void updateplayerHP()
    {
        GameManager.Instance.HPBar.fillAmount = (float)HP / HPOrig;
    }
    public void takeDamage(int amount)
    {
        HP -= amount; 
        updateplayerHP();
        if(HP<=0)
        {
            GameManager.Instance.stateLose();
        }
        
    }
    void crouch()
    {
        controller.height /= 2;
    }
    void unCrouch()
    {
        controller.height *= 2;
    }
}
