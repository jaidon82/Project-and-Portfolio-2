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
    int selectedGun;
    Vector3 moveDir;
    Vector3 playerVel;

    [Header("------ Gun Stats -----")]
    [SerializeField] List<gunStats> gunList= new List<gunStats>();
    [SerializeField] GameObject gunModel;
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

        if(gunList.Count > 0 && Input.GetButton("Shoot") && !isShooting)
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

        selectGun();
    }


    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)), out hit, shootDist))
        {
            Debug.Log(hit.collider.name);
            Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation); //display hit effect
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

    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        shootDamage = gun.shootDamage;
        shootRate=gun.shootRate;
        shootDist = gun.shootDist;
        gunModel.GetComponent<MeshFilter>().sharedMesh=gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;
    }


    void selectGun()
    {
        if(Input.GetAxis("Mouse ScrollWheel")>0 && selectedGun < gunList.Count-1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun> 0)
        {
            selectedGun--;
            changeGun();    
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootRate = gunList[selectedGun].shootRate;
        shootDist = gunList[selectedGun].shootDist;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
