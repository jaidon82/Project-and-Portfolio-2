using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    [Header("------ Gun Stats -----")]
    public int shootDamage;
    public int shootDist;
    public float shootRate;

    public int ammoCur;
    public int ammoMax;

    public GameObject model;
    
    public GameObject bullet;

    public AudioClip shootSound;
    [Range(0f, 10)] public float shootSoundVol;

}
