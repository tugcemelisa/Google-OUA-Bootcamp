
using System;
using UnityEngine;
using UnityEngine.AI;

public interface IPlayer 
{
    void StartMilk();
    void Milk(Transform animalTransform);
    void Shear(Transform animalTransform);
    void ScareAnimal(NavMeshAgent animalAgent);
}
    

