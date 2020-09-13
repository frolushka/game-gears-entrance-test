using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthBarDeltaText : MonoBehaviour
{
    private static readonly Func<float, float> Pattern = x => -((x - 1) * (x - 1)) + 1;
    
    [HideInInspector] public Text text;
    
    [SerializeField] private float distance;
    [SerializeField] private float destroyTime;
    
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
        StartCoroutine(Move());
        
        IEnumerator Move()
        {
            var defaultYPosition = transform.position.y;
            for (float time = 0; time < destroyTime; time += Time.deltaTime)
            {
                transform.position = new Vector3(
                    transform.position.x, 
                    defaultYPosition + distance * Pattern(time / destroyTime), 
                    transform.position.z
                );
                yield return null;
            }
        }
    }
}
