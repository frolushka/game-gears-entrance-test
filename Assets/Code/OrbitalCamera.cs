using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Camera))]
public class OrbitalCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Space] 
    [SerializeField] private CameraModel settings;

    private Camera _camera;
    private Transform _transform;

    private float _targetFov;
    private float _deltaFov;
    private float _nextFovChange;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _transform = transform;
    }

    private void Start()
    {
        _nextFovChange = settings.fovDelay;

        _targetFov = settings.fovMax;
        _camera.fieldOfView = _targetFov;
    }

    private void FixedUpdate()
    {
        UpdateTransform(Time.fixedTime);
        UpdateCamera(Time.fixedTime);
    }

    private void UpdateTransform(float time)
    {
        var angle = 2 * Mathf.PI / settings.roundDuration * time; 
        var sinAngle = 2 * Mathf.PI / settings.roamingDuration * time;
        
        _transform.position = new Vector3(
            settings.roundRadius * Mathf.Sin(angle), 
            target.position.y + settings.height + settings.roamingRadius * Mathf.Sin(sinAngle), 
            settings.roundRadius * Mathf.Cos(angle));
        _transform.rotation = Quaternion.LookRotation(target.position - _transform.position + Vector3.up * settings.lookAtHeight);
    }

    private void UpdateCamera(float time)
    {
        _camera.fieldOfView = Mathf.MoveTowards(_camera.fieldOfView, _targetFov, Time.fixedDeltaTime * _deltaFov / settings.fovDuration);
        
        if (time >= _nextFovChange)
        {
            _targetFov = Random.Range(settings.fovMin, settings.fovMax);
            _deltaFov = Mathf.Abs(_targetFov - _camera.fieldOfView);
            _nextFovChange = time + settings.fovDelay;
        }
    }
}