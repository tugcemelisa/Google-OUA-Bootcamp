using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class WolfManager : MonoBehaviourSingletonPersistent<WolfManager>
{
    [SerializeField] Transform[] attackableTargets;

    [SerializeField] Transform[] circleTargets;
    [SerializeField] Transform centerOfTheCircle;

    [SerializeField] List<WolfController> normalWolves = new List<WolfController>();

    [SerializeField] List<WolfController> circleWolves = new List<WolfController>();

    [SerializeField] float timeBetweenAttacks = 5f;


    [Range(0, 360)]
    float angle = 0f;
    [SerializeField] float rotationSpeed = 5f;

    private void Start()
    {
        CircleWaveStart();
        StartCoroutine(WolfAttacks());
    }

    private void Update()
    {
        angle += Time.deltaTime * rotationSpeed;
        centerOfTheCircle.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    void CircleWaveStart()
    {
        int i = 0;
        foreach (var wolf in normalWolves)
        {
            wolf.transform.position = circleTargets[i].position;
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
        int randomIndex = Random.Range(0, circleWolves.Count);
        if (randomIndex < -1) return;
        var selectedWolf = circleWolves[randomIndex];

        if (selectedWolf == null) return;
        circleWolves.Remove(selectedWolf);
        selectedWolf.transform.parent = this.transform;
        selectedWolf.AssignNewTarget(attackableTargets[Random.Range(0, attackableTargets.Length)], true);
        normalWolves.Add(selectedWolf);
    }

    public void AddWolfToTheCircle(WolfController wolf, Transform target)
    {
        wolf.transform.forward = (centerOfTheCircle.position - target.position).normalized;
        wolf.transform.forward = -wolf.transform.right;
        wolf.transform.position = target.position;
        wolf.transform.parent = target;

        circleWolves.Add(wolf);
    }

}
