using UnityEngine;

public class HarvesterBlades : MonoBehaviour
{
    [Header("GD")]
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            transform.RotateAround(Vector3.zero, -_rotationSpeed * Time.deltaTime);
        }
    }
}
