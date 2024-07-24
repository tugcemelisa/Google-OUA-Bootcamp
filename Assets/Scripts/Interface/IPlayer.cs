
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IPlayer 
{
    void StartMilk();
    void Milk(Transform animalTransform);
    void Shear(Transform animalTransform);
    void ScareAnimal(NavMeshAgent animalAgent);
    void TakeAnimals(List<AnimalBase> animals);
}
    

