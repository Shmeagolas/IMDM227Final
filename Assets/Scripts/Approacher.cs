using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approacher : MonoBehaviour
{
   //[SerializeField] public GameObject player;
   public float approachSpeed;
   private float stuckDuration = 3f;
   private float playerRing = 2f;
   private float positionRange = 0.01f;

   //manually setting player position
   private Vector3 playerPosition = new Vector3(0, 1, -10);
   private Vector3 lastPosition;
   private float stuckTime = 0f;



    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        if (Vector3.Distance(transform.position, lastPosition) < positionRange) {
            stuckTime += Time.deltaTime;

            if (stuckTime >= stuckDuration && distanceToPlayer > playerRing) {
                getUnstuck();
                stuckTime = 0f;
            }
        } else {
            stuckTime = 0f;
        }

        lastPosition = transform.position;

 
        if (distanceToPlayer > playerRing) {
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, approachSpeed * Time.deltaTime);
        }
    }



    void getUnstuck() {

        Vector3 directionToPlayer = playerPosition - transform.position;

        Vector3 moveDirection;
        if (Vector3.Dot(transform.right, directionToPlayer) > 0) {
            //right
            moveDirection = transform.right;
        } else {
            //left
            moveDirection = -(transform.right);
        }

        Vector3 targetPos = transform.position + moveDirection * 1f;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, approachSpeed * Time.deltaTime);
    }
}

