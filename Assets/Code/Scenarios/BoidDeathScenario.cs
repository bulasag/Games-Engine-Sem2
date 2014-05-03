using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.Scenarios
{
    class BoidDeathScenario : Scenario
    {

        public override string Description()
        {
            return "Segment 2, Boid Death";
        }

        public override void Start()
        {
			GroundEnabled(true);

			int groundlevel = 50;
			int obstacleheight = 30;
			int obstacleoffset = 480;
			Vector3 temp = GameObject.FindGameObjectWithTag ("ground").transform.position;
			temp.y = groundlevel - 4;
			GameObject.FindGameObjectWithTag ("ground").transform.position = temp;
			
			Params.camMode = (int) Params.camModes.boid;

            Params.Load("default.txt");

			// Create a leader for the fleet and configure which steering behaviours it should have
            leader = CreateBoid(new Vector3(-10, groundlevel + 1, 550), boidPrefab);
            leader.GetComponent<SteeringBehaviours>().ArriveEnabled = true;
            leader.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
            leader.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(-30, groundlevel + 1, 750);

            // Create obstacle course to traverse
			CreateObstacle(new Vector3(-30, groundlevel, obstacleoffset + 250), obstacleheight, 30); // Death Trap
			CreateObstacle(new Vector3(65, groundlevel, obstacleoffset + 300), obstacleheight, 30);
			CreateObstacle(new Vector3(-5, groundlevel, obstacleoffset + 350), obstacleheight, 45);
			CreateObstacle(new Vector3(-90, groundlevel, obstacleoffset + 400), obstacleheight, 15);
			CreateObstacle(new Vector3(-80, groundlevel, obstacleoffset + 420), obstacleheight, 15);
			CreateObstacle(new Vector3(70, groundlevel, obstacleoffset + 430), obstacleheight, 40);
			CreateObstacle(new Vector3(-70, groundlevel, obstacleoffset + 460), obstacleheight, 30);
		}

    }
}