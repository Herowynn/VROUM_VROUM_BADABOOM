using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultipleInputManager : MonoBehaviour
{
    [Header("Instance")] 
    public GameObject PrefabToInstantiate;
    public InputActionReference JoinAction;
    public int NumberOfPlayer;
    public GameObject[] PlayersPrefab;
    
    [HideInInspector] public PlayerInputManager PlayerInputManager;

    public void JoinPlayerFromAction(InputAction.CallbackContext context)
    {
        
    }

    private void Start()
    {
        for (int i = 0; i < NumberOfPlayer; i++)
        {
            PlayersPrefab[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (JoinAction.action.triggered)
            Debug.Log("oui");
    }
}
