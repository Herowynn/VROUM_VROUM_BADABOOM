using UnityEngine;

public class HarvesterBlades : MonoBehaviour
{
    [Header("GD")]
    public float RotationSpeed;

    private Harvester _harvesterBody;

    private void Start()
    {
        _harvesterBody = transform.parent.GetComponent<Harvester>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.RACING && _harvesterBody.CanMove)
            transform.RotateAround(transform.forward, -RotationSpeed * Time.deltaTime);
    }
}
