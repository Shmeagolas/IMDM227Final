using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approacher : MonoBehaviour
{
   public float approachSpeed;
   private float stuckDuration = 3f;
   private float playerRing = 4f;
   private float positionRange = 0.01f;

   private string playerName = "playerPrefab";
   private Vector3 playerPosition;
   private Vector3 lastPosition;
   private float stuckTime = 0f;
   private float distanceToPlayer;
   private bool updatePos = true;
   private Transform playerTransform;

   private float nextDistanceCheckTime = -1f, checkDelay = .1f;

   private bool isDay;
   private Vector3 dayDirection;

   void Start()
   {   
       playerTransform = GameObject.Find(playerName).transform;
       playerPosition = playerTransform.position;
   }

   void Update()
   {
       if (isDay)
       {
           if (dayDirection == Vector3.zero)
           {
               dayDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
           }
           transform.position += dayDirection * approachSpeed * Time.deltaTime;
           return;
       }
       else
       {
           dayDirection = Vector3.zero;
       }

       if (updatePos)
       {
           if (Time.time >= nextDistanceCheckTime || nextDistanceCheckTime == -1f)
           {
               nextDistanceCheckTime = Time.time + checkDelay;
               distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

               if (Vector3.Distance(transform.position, lastPosition) < positionRange) 
               {
                   stuckTime += Time.deltaTime;

                   if (stuckTime >= stuckDuration && distanceToPlayer > playerRing) 
                   {
                       getUnstuck();
                       stuckTime = 0f;
                   }
               } 
               else 
               {
                   stuckTime = 0f;
               }

               transform.LookAt(playerTransform);
           }
           
           lastPosition = transform.position;

           if (distanceToPlayer > playerRing) {
               transform.position = Vector3.MoveTowards(transform.position, playerPosition, approachSpeed * Time.deltaTime);
           }
           else
           {
               GetComponent<Shootable>().Attack();
           }
       }
   }

   void getUnstuck() 
   {
       Vector3 directionToPlayer = playerPosition - transform.position;
       Vector3 moveDirection;
       if (Vector3.Dot(transform.right, directionToPlayer) > 0) {
           moveDirection = transform.right;
       } else {
           moveDirection = -(transform.right);
       }

       Vector3 targetPos = transform.position + moveDirection * 1f;
       transform.position = Vector3.MoveTowards(transform.position, targetPos, approachSpeed * Time.deltaTime);
   }

   public void stopMoving()
   {
       updatePos = false;
   }

   public void setDay(bool isday)
   {
        isDay = isday;
   }
}
