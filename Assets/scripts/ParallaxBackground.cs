using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    
    private Vector3 startPosition;
    private float repeatWidth;
    public float speed;
    void Start()
    {
        repeatWidth = GetComponent<BoxCollider>().size.x;
       
        startPosition = transform.position;
        
    }

   
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x < startPosition.x - repeatWidth) 
        {
            transform.position = startPosition;
        
        }

     
        
    }
}
  