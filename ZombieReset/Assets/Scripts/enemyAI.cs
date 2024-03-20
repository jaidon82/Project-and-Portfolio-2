using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int viewCone;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animSpeedTrans;
    [SerializeField] float shootRate;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    bool isShooting;
    bool playerInRange;
    bool destinationChosen;
    private float stoppingDistOrig;
    float angleToPlayer;
    Vector3 playerDir;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.updateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans)); //activates the blend tree

        if (playerInRange && !canSeePlayer())
        {

            StartCoroutine(roam());
                    
        }
        else if (!playerInRange)
        {
            StartCoroutine (roam());
        }

    }

    public void takeDamage(int amount)
    {
      HP -= amount;
        StartCoroutine(flashRed());
        anim.SetTrigger("Damage");

        if(HP <= 0)
        {
            GameManager.Instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }



    IEnumerator shoot()
    {
        isShooting= true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.transform.position, transform.rotation);

    }

    IEnumerator flashRed()
    {
        model.material.color= Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    IEnumerator roam()
    {

        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);


            destinationChosen = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.Instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);


        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            Debug.Log(hit.collider);

            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(GameManager.Instance.player.transform.position);
                agent.stoppingDistance = stoppingDistOrig;
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();

                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }


    void faceTarget()
    {
        Quaternion ROT = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, ROT, Time.deltaTime);
    }


}
