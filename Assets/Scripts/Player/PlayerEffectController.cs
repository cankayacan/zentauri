using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem walkingParticles = default;
    [SerializeField] ParticleSystem shootingParticles = default;

    public void EnableWalkParticles()
    {
        walkingParticles.Play();
    }

    public void DisableWalkParticles()
    {
        walkingParticles.Stop();
    }

    public void EnableShootingParticles()
    {
        shootingParticles.Play();
    }
}
