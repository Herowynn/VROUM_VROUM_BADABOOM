using UnityEngine;

public class HarvesterBlades : MonoBehaviour
{
    [Header("GD")]
    public float RotationSpeed;

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING)
        {
            transform.RotateAround(transform.forward, -RotationSpeed * Time.deltaTime);
        }
    }
}
