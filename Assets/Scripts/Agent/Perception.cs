using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    public string tagName;
    [SerializeField] [Range(1, 40)] public float distance = 1;
    [SerializeField] [Range(0, 180)] public float angle = 0;
    public abstract GameObject[] GetGameObjects();
}
