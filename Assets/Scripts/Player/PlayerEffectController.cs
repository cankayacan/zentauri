using System;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem walkingParticles = default;
    [SerializeField] ParticleSystem shootingParticles = default;

    private ParticleSystem walkingParticleSystem;
    private ParticleSystem shootingParticleSystem;

    private void Awake()
    {
        if (walkingParticles)
        {
            walkingParticleSystem = Instantiate(walkingParticles, transform);    
        }

        if (shootingParticles)
        {
            shootingParticleSystem = Instantiate(shootingParticles, transform);
        }
    }

    public void EnableWalkParticles()
    {
        if (!walkingParticleSystem) return;
        walkingParticleSystem.Play();
    }

    public void DisableWalkParticles()
    {
        if (!walkingParticleSystem) return;
        walkingParticleSystem.Stop();
    }

    public void EnableShootingParticles()
    {
        if (!shootingParticleSystem) return;
        shootingParticleSystem.Play();
    }
}
