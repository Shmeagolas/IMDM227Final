using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approacher : MonoBehaviour
{
   //[SerializeField] public GameObject player;
   public float approachSpeed;
   private float stuckDuration = 3f;
   private float playerRing = 4f;
   private float positionRange = 0.01f;

   //manually setting player position
   private string playerName = "playerPrefab";
   private Vector3 playerPosition;
   private Vector3 lastPosition;
   private float stuckTime = 0f;
   private float distanceToPlayer;
    private bool updatePos = true;
    private Transform playerTransform;
   //timer vars
   private float nextDistanceCheckTime = -1f, checkDelay = .1f; //1 sec delay

    
    void Start()
    {   
        Transform playerTransform = GameObject.Find(playerName).transform;
        playerPosition  = playerTransform.position;
        

        if (playerName == null)
        {
            Debug.LogError($"Object {playerName} not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(updatePos)
        {
            //small optimization so not calculating distance checks every frame
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
            //deal large amount of damage to trigger 
            GetComponent<Shootable>().Attack();
        }
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

    public void stopMoving()
    {
        updatePos = false;
    }
}


