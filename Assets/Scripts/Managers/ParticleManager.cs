using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviourSingleton<ParticleManager>
{
    [SerializeField] List<ParticleData> particles;
    public void SpawParticle(ParticleType particleType, Transform parent, Vector3 pos)
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
    BloodSpill,
    Fog,
    Bite,

}

[System.Serializable]
public class ParticleData
{
    public ParticleType pacticleType;
    public string Name;
    public Transform prefab;
}