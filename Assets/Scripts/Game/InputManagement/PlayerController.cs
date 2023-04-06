using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private Vector2 _movementInput;

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
            transform.Translate(new Vector3(_movementInput.x, 0, _movementInput.y) * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();
}
