using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
        public GameObject enemy;    //the enemy object that will be spawned
        public Transform plane;     //the plane that the enemies will be spawned from
        // public float spawnDuration = 5f;    //how long the enemies will keep spawning (for testing purposes)
        public float spawnInterval = 0.5f;  //how often enemies spawn
        public bool gameover = false;

        private float spawnTimer = 0f;  //tracks spawn interval
        private float spawnDurationTimer = 0f;  //tracks spawn duration


        void Start()
        {
            this.enabled = false;
        }


        // Update is called once per frame
        void Update() {
            spawnDurationTimer += Time.deltaTime;   //counts time btwn spawns

            //stops spawning when spawnDurationTimer is greater than or equal to spawnDuration
            // if (spawnDurationTimer >= spawnDuration) {
            //     return;
            // }

            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval) {
                //spawn enemy at random point on plane
                Vector3 randomPosition = getRandomPointOnPlane();
                Instantiate(enemy, randomPosition, Quaternion.identity);

                //reset timer
                spawnTimer = 0f;
            }
        }

        // this method just finds a random point on the spawn plane
        private Vector3 getRandomPointOnPlane() {
            //accesses renderer component of plane
            Renderer planeRender = plane.GetComponent<Renderer>();
            Bounds bounds = planeRender.bounds;

            //randomly get x and z coordinates
            float xPos = Random.Range(bounds.min.x, bounds.max.x);
            float zPos = Random.Range(bounds.min.z, bounds.max.z);

            //y coordinate is just on top of the plane
            float yPos = plane.position.y + 0.25f;

            return new Vector3(xPos, yPos, zPos);
        }
}
