using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmAnimalManager : MonoBehaviourSingletonPersistent<FarmAnimalManager>
{
    [HideInInspector] private List<AnimalBase> animals = new();
    private void OnEnable()
    {
        PlayerSimulationController.OnHerdLeaveBarn += Straggle;
        PlayerSimulationController.OnTranshumingStart += UpdateAnimalsList;
    }
    private void OnDisable()
    {
        PlayerSimulationController.OnHerdLeaveBarn -= Straggle;
        PlayerSimulationController.OnTranshumingStart -= UpdateAnimalsList;
    }

    private void UpdateAnimalsList(List<AnimalBase> animalsToAttack)
    {
        for (int i = 0; i < animalsToAttack.Count; i++)
        {
            animals.Add(animalsToAttack[i]);
        }
    }

    public void Straggle()
    {
        PlanStraggle();
    }

    private IEnumerator PlanStraggle()
    {
        int duration = Random.Range(20, 50);
        yield return new WaitForSeconds(duration);

        if (animals.Count > 0)
        {
            int animalIndex = Random.Range(0, animals.Count);
            AnimalBase animal = animals[animalIndex];
            animal.StartStraggle();
        }
    }
}
