using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approacher : MonoBehaviour
{
    // public GameObject targetPosition;
   public float approachSpeed;

   //manually setting player position
   private Vector3 playerPosition = new Vector3(0, 1, -10);


    // Update is called once per frame
    void Update()
    {
        //making object move towards the playerPosition
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, approachSpeed * Time.deltaTime);
    }
}
