using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<GameObject> Targets = new List<GameObject>();
    public Vector3 Offset;
    Vector3 _velocity;
    public float SmoothTime = .5f;

    public float MinZoom = 40f;
    public float MaxZoom = 10f;
    public float ZoomLimiter = 50f;

    private void Start()
    {
        if (Targets.Count == 0)
        {
            transform.position = Offset;
        }
    }

    private void LateUpdate()
    {
        if (Targets.Count == 0) return;

        Move();
        Zoom();
    }

    public void AddTargets()
    {
        foreach (var player in GameManager.Instance.Players)
        {
            if (!Targets.Contains(player)) Targets.Add(player);
        }
    }
    
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + Offset, ref _velocity, SmoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(MaxZoom, MinZoom, GetGreaterDistance() / ZoomLimiter);

        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, newZoom, Time.deltaTime);
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
