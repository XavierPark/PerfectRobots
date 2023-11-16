using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int ammoInMagCurr;
    public int ammoMagMax;
    public bool Electric;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootSoundVol;
}
