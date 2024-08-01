using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviourSingletonPersistent<ParticleManager>
{
    [SerializeField] List<ParticleData> particles;
    public void PlayParticle(ParticleType particleType, Transform parent, Vector3 pos)
    {
        foreach (var particle in particles)
        {
            if(particle.pacticleType == particleType)
            {
                Transform spawned= Instantiate(particle.prefab, parent);
                spawned.position = pos;
                break;
            }
        }
    }
}

public enum ParticleType
{
    Die,
    Disappear,
    BloodSpill,
    Fog,
    Bite,
    MachineWorking,

}

[System.Serializable]
public class ParticleData
{
    public ParticleType pacticleType;
    public string Name;
    public Transform prefab;
}
