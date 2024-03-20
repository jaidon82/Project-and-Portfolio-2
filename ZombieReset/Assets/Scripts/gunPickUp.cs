using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //transfer the gun stats to the player
            GameManager.Instance.playerScript.getGunStats(gun);
            Destroy(gameObject);

        }
    }
}
