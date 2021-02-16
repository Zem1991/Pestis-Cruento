using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doodad : MonoBehaviour
{
    [Header("Identification")]
    [SerializeField] protected string doodadName = "Unknown Doodad";

    [Header("Health")]
    [SerializeField] protected bool isDestroyable = false;
    [SerializeField] protected int currentHealth = 10;
    [SerializeField] protected int maximumHealth = 10;
}
