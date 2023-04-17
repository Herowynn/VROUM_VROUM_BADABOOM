using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Info")]
    public Color Color;
    public float speed = 5;
    public PlayerState PlayerState;

    //Need to move to separate file (?)
    public int Score;

    //Need to move to separate file (?)
    [Header("UI")]
    public PointsUI PointsUI;
    public ProfileUI ProfileUI;

    // Intern Var
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private Vector2 _movementInput;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        CreatePlayerUis();

        PointsUI.ChangeVisualColoration(Color);
        PointsUI.ChangePointsCount(Score);
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && PlayerState == PlayerState.ALIVE)
            transform.Translate(new Vector3(_movementInput.x, 0, _movementInput.y) * speed * Time.deltaTime);
    }

    #region Related to in-game actions

    public void OnMove(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

    public void OnBoost(InputAction.CallbackContext context) => ProfileUI.UseBoost();

    public void OnAttack(InputAction.CallbackContext context) => ProfileUI.UseWeapon();

    public void OnNewBoost(InputAction.CallbackContext context) => ProfileUI.TookBoost();

    public void OnNewWeapon(InputAction.CallbackContext context) => ProfileUI.TookWeapon();

    #endregion

    #region Related to state in-game

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.GetComponent<DestructorComponent>())
            //GameManager.Instance.TriggerPlayerDestructionEvent(this);*/   
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

    public void AddPointsToScore(int points)
    {
        Score += points;
        PointsUI.ChangePointsCount(Score);
    }

    #endregion

    #region Related to UI

    private void CreatePlayerUis()
    {
        //GameManager.Instance.TriggerUiCreationForPlayerEvent(this);
    }

    #endregion
}
