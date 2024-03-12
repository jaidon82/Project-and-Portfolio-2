using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position,GameManager.Instance.player.transform.position, Time.deltaTime *speed);
        
         
        
    }

    public void takeDamage(int amount)
    {
      HP -= amount;
        StartCoroutine(flashRed());

        if(HP <= 0)
        {
            GameManager.Instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }



    IEnumerator shoot()
    {
        isShooting= true;
        Instantiate(bullet, shootPos.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashRed()
    {
        model.material.color= Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
