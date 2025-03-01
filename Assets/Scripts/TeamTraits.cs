﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamTraits", menuName = "ScriptableObjects/TeamTraits", order = 1)]
public class TeamTraits : ScriptableObject
{
    [Header("Trait Distributions - all Normally Distributed")]


    [Tooltip("Weight")]
    public float weightMean;
    public float weightSD;


    [Tooltip("MaxVelocity")]
    public float maxVeloMean;
    public float maxVeloSD;


    [Tooltip("Agressiveness")]
    public float agressivenessMean;
    public float agressivenessSD;


    [Tooltip("MaxExhaustion")]
    public float maxExhaustionMean;
    public float maxExhaustionSD;

    [Tooltip("Urge")]
    public float urgeMean;
    public float urgeSD;

    [Tooltip("JokerSpawnChance")]
    public float jokerSpawnChance;


    [Header("Spawn info")]
    public Vector3 spawnOrigin;
    public float spawnRadius;
    public Color color;
    public Color jokerColor;


    [Header("Movement Force Weight")]
    public float snitchWeight;
    public float collisionAvoidanceWeight;

    public List<GameObject> players;
}
