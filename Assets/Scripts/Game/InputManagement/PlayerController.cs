using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public PlayerState PlayerState;
    public int Score;

    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private Vector2 _movementInput;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE)
            transform.Translate(new Vector3(_movementInput.x, 0, _movementInput.y) * speed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DestructorComponent>())
            GameManager.Instance.TriggerPlayerDestructionEvent(this);
    }

    public void DiedEvent()
    {
        _meshRenderer.enabled = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _boxCollider.enabled = false;
        PlayerState = PlayerState.DEAD;
    }

    public void RebornEvent()
    {
        _meshRenderer.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _boxCollider.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                 RigidbodyConstraints.FreezeRotationZ;
        
        PlayerState = PlayerState.ALIVE;
    }
}
