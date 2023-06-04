using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraController is a script that controls the camera for the game.
/// It uses the positions of every alive player in game. It moves and zoom in and out in a late update call.
/// </summary>

public class CameraController : MonoBehaviour
{
    [Header("Instance")]
    public List<GameObject> Targets = new List<GameObject>();
    public Camera Camera;

    [Header("GD")]
    public Vector3 Position;
    public Vector3 Rotation;
    public float MinZoom = 40f;
    public float MaxZoom = 10f;
    public float ZoomLimiter = 50f;
    
    [Header("GA")]
    public float SmoothTime = .5f;

    private Vector3 _velocity;
    private List<GameObject> _targetsToFollow = new List<GameObject>();

    private void Start()
    {
        if (Camera == null)
            Camera = GetComponent<Camera>();
        
        transform.position = Position;
        
        Quaternion rot = transform.rotation;
        rot.eulerAngles = Rotation;
        
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        if (Targets.Count == 0) return;

        Move();
        Zoom();
    }

    public void AddTargets()
    {
        Targets = new List<GameObject>();

        foreach (var player in GameManager.Instance.PlayersManager.Players)
        {
            Targets.Add(player);
        }
    }

    public void RemoveDeadTargetEvent()
    {
        Targets.RemoveAll(item => item.GetComponent<GlobalController>().PlayerState == PlayerState.DEAD);
    }
    
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + Position, ref _velocity, SmoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(MaxZoom, MinZoom, GetGreaterDistance() / ZoomLimiter);

        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, newZoom, Time.deltaTime);
    }

    private float GetGreaterDistance()
    {
        var bounds = new Bounds(Targets[0].transform.position, Vector3.zero);

        for (int i = 0; i < Targets.Count; i++)
        {
            bounds.Encapsulate(Targets[i].transform.position);
        }

        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if(Targets.Count == 1) return Targets[0].transform.position;

        var bounds = new Bounds(Targets[0].transform.position, Vector3.zero);

        for (int i = 0; i < Targets.Count; i++)
        {
            bounds.Encapsulate(Targets[i].transform.position);
        }

        return bounds.center;
    }
}
