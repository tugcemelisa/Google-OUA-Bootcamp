using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class WolfManager : MonoBehaviourSingletonPersistent<WolfManager>
{
    enum WolfState
    {
        BeforeHunt,
        AfterHunt
    }
    WolfState wolfState;
    [HideInInspector] public static Action OnHuntOver;
    [Header("Targets")]
    //[SerializeField] Transform[] attackableTargets;
    private List<Transform> attackableTargets = new();
    [SerializeField] Transform[] circleTargets;
    [SerializeField] Transform centerOfTheCircle;
    [SerializeField] Transform[] gettingOutTransforms;

    [Header("Wolves")]
    [SerializeField] List<WolfController> normalWolves = new List<WolfController>();
    List<WolfController> circleWolves = new List<WolfController>();
    List<WolfController> scariedWolves = new List<WolfController>();

    [Header("Configurations")]
    [Range(0, 360)]
    float angle = 0f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float timeBetweenAttacks = 5f;

    private void OnEnable()
    {
        wolfState = WolfState.BeforeHunt;

        GameModeManager.OnNightStart += () => Invoke("StartAttack", 5f);
        PlayerSimulationController.OnTranshumingStart += UpdateAttackableList;
        ReturnVillageButton.OnReturnVillageRequest += StartReturn;
    }
    private void OnDisable()
    {
        GameModeManager.OnNightStart -= () => Invoke("StartAttack", 5f);
        PlayerSimulationController.OnTranshumingStart -= UpdateAttackableList;
        ReturnVillageButton.OnReturnVillageRequest -= StartReturn;
    }

    private void UpdateAttackableList(List<AnimalBase> animalsToAttack)
    {
        for (int i = 0; i < animalsToAttack.Count; i++)
        {
            attackableTargets.Add(animalsToAttack[i].transform);
        }
    }

    private void StartAttack()
    {
        if (attackableTargets.Count >= 1)
        {
            CircleWaveStart();
        }
        else
        {
            ChatBubble.Create(null, transform.position, IconType.Informative, "You have to bring some animals to the meadow");
        }
    }

    private void Update()
    {
        if (GameModeManager.Instance.executingGameMode != ExecutingGameMode.Night) return;

        angle += Time.deltaTime * rotationSpeed;
        centerOfTheCircle.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    void CircleWaveStart()
    {
        int i = 0;
        foreach (var wolf in normalWolves)
        {
            //wolf.transform.position = circleTargets[i].position;
            wolf.StartCircleRun(circleTargets[i]);
            i++;
            i %= circleTargets.Length;
        }
        normalWolves.Clear();
    }

    IEnumerator WolfAttacks()
    {

        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            ChooseAndSendAWolfToAttack();
        }
    }

    void ChooseAndSendAWolfToAttack()
    {
        Debug.Log("WOLF COUNT IS: " + circleWolves.Count);
        if (circleWolves.Count <= 0) return;
        int randomIndex = Random.Range(0, circleWolves.Count);
        var selectedWolf = circleWolves[randomIndex];
        Debug.Log(selectedWolf.name);

        if (selectedWolf == null) return;
        circleWolves.Remove(selectedWolf);
        selectedWolf.transform.parent = this.transform;
        selectedWolf.AssignNewTarget(attackableTargets[Random.Range(0, attackableTargets.Count)], true);
        normalWolves.Add(selectedWolf);
    }

    public void AddWolfToTheCircle(WolfController wolf, Transform target)
    {
        wolf.transform.forward = (centerOfTheCircle.position - target.position).normalized;
        wolf.transform.forward = -wolf.transform.right;
        wolf.transform.position = target.position;
        wolf.transform.parent = target;

        circleWolves.Add(wolf);
        Debug.Log("Count: " + circleWolves.Count);
        if (circleWolves.Count == circleTargets.Length)
        {
            foreach (var item in circleWolves)
            {
                item.SetState(WolfStates.RunInsideCircle);
            }
            GameModeManager.Instance.executingGameMode = ExecutingGameMode.Night;
            StartCoroutine(WolfAttacks());
        }
    }

    public void RunAway(WolfController wolf)
    {
        //if (circleWolves.Count <= 0) return;

        if (normalWolves.Contains(wolf))
            normalWolves.Remove(wolf);
        if (circleWolves.Contains(wolf))
            circleWolves.Remove(wolf);

        var selectedGettingOutTransform = findClosestTransform(gettingOutTransforms, wolf.transform.position);
        wolf.transform.parent = selectedGettingOutTransform;
        wolf.AssignNewTarget(selectedGettingOutTransform, false);

        scariedWolves.Add(wolf) ;

        if (circleWolves.Count <= 0)
        {
            wolfState = WolfState.AfterHunt;
            StartCoroutine(ShowHelpers());
            //Debug.Log("NIGHT END");
        }
    }

    Transform findClosestTransform(Transform[] transforms, Vector3 pos)
    {
        var closestOne = transforms[0];
        var closestDistance = (closestOne.position - pos).magnitude;

        foreach (var transform in transforms)
        {
            var distance = (transform.position - pos).magnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestOne = transform;
            }
        }

        return closestOne;
    }

    private void StartReturn()
    {
        if (wolfState == WolfState.AfterHunt)
        {
            OnHuntOver?.Invoke();
            //StartCoroutine(ShowHelpers());
            wolfState = WolfState.BeforeHunt;
        }
        Debug.Log(wolfState.ToString());
        HelperController.Instance.ShowHelper(HelpType.BarnPanel);
    }
    private IEnumerator ShowHelpers()
    {
        HelperController.Instance.ShowHelper(HelpType.CollectPanel);
        yield return new WaitForSeconds(30f);
        HelperController.Instance.ShowHelper(HelpType.ReturnPanel);
    }

    public void ResetWolfManagerFor(MeadowWolfManagerInfoHolder meadow)
    {
        int i = 0;
        foreach (var wolf in scariedWolves)
        {
            wolf.ResetWolf(meadow.SpawnPositionsForWolves[i++]);
        }
        i = 0;
        foreach (var wolf in normalWolves)
        {
            wolf.ResetWolf(meadow.SpawnPositionsForWolves[i++]);
        }

        centerOfTheCircle = meadow.centerOfTheCircle;
        circleTargets = meadow.circleTargets;
        gettingOutTransforms = meadow.gettingOutTransforms;

        //attackableTargets = new();
        //UpdateAttackableList();
    }
}
