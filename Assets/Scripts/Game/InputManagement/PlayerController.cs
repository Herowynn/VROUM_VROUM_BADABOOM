using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private Vector2 movementInput;

    private void Update()
    {
        if (true)
            transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context) => movementInput = context.ReadValue<Vector2>();
}
