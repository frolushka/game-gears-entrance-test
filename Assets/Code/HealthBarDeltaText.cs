using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthBarDeltaText : MonoBehaviour
{
    [HideInInspector] public Text text;
    
    [SerializeField] private float distance;
    [SerializeField] private float destroyTime;

    private Vector3 _targetPosition;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    
    public void StartMove()
    {
        _targetPosition = transform.position + Vector3.up * distance;
        StartCoroutine(Move());
        
        IEnumerator Move()
        {
            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, 1f / destroyTime);
                yield return null;
            }
        }
    }
}
