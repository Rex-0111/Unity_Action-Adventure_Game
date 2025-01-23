using System;
using UnityEngine;
using UnityEngine.VFX;
 
public class Particle_Spawn_Despawn : MonoBehaviour
{
    private DissolveObjectPlayer _DissolveObjectPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float Value;
    private VisualEffect vfx;
    DissolveObjectPlayer dissolveObjectPlayer;
    void Start()
    {
        dissolveObjectPlayer = GetComponentInChildren<DissolveObjectPlayer>();
        vfx = GetComponentInChildren<VisualEffect>();

        dissolveObjectPlayer.Spawn += ParticlePlay;
        dissolveObjectPlayer.DeSpawn += ParticleStop;
    }

    private void ParticleStop()
    {
        vfx.Stop();
    }

    private void ParticlePlay()
    {
        vfx.Play();
    }

    
}
