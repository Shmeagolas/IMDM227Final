// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Approacher : MonoBehaviour
// {
//    //[SerializeField] public GameObject player;
//    public float approachSpeed;
//    private float stuckDuration = 3f;

//    //manually setting player position
//    private Vector3 playerPosition = new Vector3(0, 1, -10);

//    private Vector3 lastPosition;
//    private float stuckTime = 0f;


//     // Update is called once per frame
//     void Update()
//     {

//         if (transform.position == lastPosition) {
//             stuckTime += Time.deltaTime;
//             if (stuckTime >= stuckDuration) {
//                 getUnstuck();
//                 stuckTime = 0f;
//             }
//         } else {
//             stuckTime = 0f;
//         }

//         lastPosition = transform.position;

//         //making object move towards the playerPosition
//         transform.position = Vector3.MoveTowards(transform.position, playerPosition, approachSpeed * Time.deltaTime);
//     }

//     void getUnstuck() {
//         //if finds whether object is stuck to the left or right
//         if (transform.position.x < playerPosition.x) {
//             //move left
//             transform.position += Vector3.left * 2f;
//         } else {
//             //move right
//             transform.position += Vector3.right * 2f;
//         }
//     }
// }





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

        if (Vector3.Distance(transform.position, lastPosition) < positionRange)
        {
            stuckTime += Time.deltaTime;

            if (stuckTime >= stuckDuration)
            {
                getUnstuck();
                stuckTime = 0f;
            }
        }
        else
        {
            stuckTime = 0f;
        }

        lastPosition = transform.position;

 
        if (distanceToPlayer > playerRing)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, approachSpeed * Time.deltaTime);
        }
    }



    void getUnstuck() {

        Vector3 directionToPlayer = playerPosition - transform.position;

        if (Vector3.Dot(transform.right, directionToPlayer) > 0)
        {
            // Object is on the right side of the player, move right
            transform.position += transform.right * 0.5f;
        }
        else
        {
            // Object is on the left side of the player, move left
            transform.position -= transform.right * 0.5f;
        }
    }
}

