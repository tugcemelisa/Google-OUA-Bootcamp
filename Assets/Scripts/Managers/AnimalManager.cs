using System.Collections;
using UnityEngine;


public class AnimalManager : MonoBehaviourSingletonPersistent<AnimalManager>
{

    [SerializeField]
    private Transform meadow;

    public Transform Meadow { get => meadow; set => meadow = value; }
    public Transform Home { get => home; set => home = value; }

    [SerializeField] private Transform home;


}
