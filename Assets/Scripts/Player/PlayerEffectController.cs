using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem walkingParticles = default;
    [SerializeField] ParticleSystem shootingParticles = default;

    public void EnableWalkParticles()
    {
        if (!walkingParticles) return;
        walkingParticles.Play();
    }

    public void DisableWalkParticles()
    {
        if (!walkingParticles) return;
        walkingParticles.Stop();
    }

    public void EnableShootingParticles()
    {
        if (!shootingParticles) return;
        shootingParticles.Play();
    }
}
