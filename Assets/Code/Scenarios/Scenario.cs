using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace BGE.Scenarios
{
    public abstract class Scenario
    {
		public float scenarioTimer = 0.0f;
		public float scenarioDuration = 0.0f;
		public Vector3 lookPos;

        System.Random random = new System.Random(DateTime.Now.Millisecond);

        public GameObject leaderPrefab = SteeringManager.Instance.leaderPrefab;
        public GameObject boidPrefab = SteeringManager.Instance.boidPrefab;

        public abstract string Description();
        public abstract void Start();

        public GameObject leader;
		public GameObject secondcar;

        public virtual void Update()
        {

        }

        public void DestroyObjectsWithTag(string tag)
        {
            GameObject[] o = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < o.Length; i++)
            {
                GameObject.Destroy(o[i]);
            }
        }

        public GameObject CreateObstacle(Vector3 position, float radius)
        {
            GameObject o;

            o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            o.tag = "obstacle";
			o.renderer.material.color = Color.grey;
			o.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            o.transform.position = position;
            return o;
        }
		
		public GameObject CreateObstacle(Vector3 position, int height, float radius)
        {
            GameObject o;

            o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            o.tag = "obstacle";
            o.renderer.material.color = Color.grey;
			o.transform.localScale = new Vector3(radius * 2, height, radius * 2);
            o.transform.position = position;
            return o;
        }

        public GameObject CreateBoid(Vector3 position, GameObject prefab)
        {
            GameObject boid;

            boid = (GameObject)GameObject.Instantiate(prefab);
            boid.tag = "boid";
            boid.AddComponent<SteeringBehaviours>();
            boid.transform.position = position;

            return boid;
        }

        public GameObject CreateCamFollower(GameObject leader, Vector3 offset)
        {
            GameObject camFollower = new GameObject();
            camFollower.tag = "camFollower";
            camFollower.AddComponent<SteeringBehaviours>();
            camFollower.GetComponent<SteeringBehaviours>().leader = leader;
            camFollower.GetComponent<SteeringBehaviours>().offset = offset;
            camFollower.transform.position = leader.transform.TransformPoint(offset);
            camFollower.GetComponent<SteeringBehaviours>().OffsetPursuitEnabled = true;
            //camFighter.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
            camFollower.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
            SteeringManager.Instance.camFighter = camFollower;
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = camFollower.transform.position;

            return camFollower;
        }

        public virtual void TearDown()
        {
            DestroyObjectsWithTag("obstacle");
			DestroyObjectsWithTag("leader");
            DestroyObjectsWithTag("boid");
            DestroyObjectsWithTag("camFollower");
        }

        public void GroundEnabled(bool enabled)
        {
            GameObject.FindGameObjectWithTag("ground").renderer.enabled = enabled;
        }
    }
}