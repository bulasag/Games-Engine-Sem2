using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.Scenarios
{

    class StartingLineScenario : Scenario
    {
        public override string Description()
        {
            return "Starting Line Scenario";
        }

        public override void Start()
        {
			GroundEnabled(true);
			
			int groundlevel = 100;
			Vector3 temp = GameObject.FindGameObjectWithTag ("ground").transform.position;
			temp.y = groundlevel - 4;
			GameObject.FindGameObjectWithTag ("ground").transform.position = temp;

            Params.Load("default.txt");
			Params.camMode = (int) Params.camModes.following;
			
			// Create a leader for the fleet and configure which steering behaviours it should have
            leader = CreateBoid(new Vector3(20, groundlevel, -20), leaderPrefab);
			leader.tag = "leader";
            leader.GetComponent<SteeringBehaviours>().ArriveEnabled = true;
            leader.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
            leader.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
            leader.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(0, groundlevel + 1, 5000);
						
			// Create obstacle course to traverse
			int obstacleheight = 50;
			int slalomlayers = 3;
			int showeramount = 6;
			
            CreateObstacle(new Vector3(5, groundlevel, 30), obstacleheight, 7);
            CreateObstacle(new Vector3(-10, groundlevel, 80), obstacleheight, 17);
            CreateObstacle(new Vector3(10, groundlevel, 120), obstacleheight, 10);
            CreateObstacle(new Vector3(50, groundlevel, 150), obstacleheight, 12);
            CreateObstacle(new Vector3(-20, groundlevel, 200), obstacleheight, 20);
            CreateObstacle(new Vector3(-65, groundlevel, 300), obstacleheight, 10);
            CreateObstacle(new Vector3(20, groundlevel, 250), obstacleheight, 10);
            CreateObstacle(new Vector3(5, groundlevel, 350), obstacleheight, 45);

            // Creates a fleet and configures which steering behaviours each follower will have
            int fleetSize = 3;
            float xOff = 12;
            float zOff = -12;
            for (int i = 2; i < fleetSize; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    float z = (i - 1) * zOff;
                    Vector3 offset = new Vector3((xOff * (-i / 2.0f)) + (j * xOff), 0, z);
					
					GameObject fleet = CreateBoid(leader.transform.position + offset, boidPrefab);

                    fleet.GetComponent<SteeringBehaviours>().leader = leader;
                    fleet.GetComponent<SteeringBehaviours>().offset = offset;
                    fleet.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
                    fleet.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(0, groundlevel + 1, 5000);
                    fleet.GetComponent<SteeringBehaviours>().OffsetPursuitEnabled = true;
                    fleet.GetComponent<SteeringBehaviours>().SeparationEnabled = true;
                    fleet.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
                }
            }
            Vector3 camOffset = new Vector3(0, 5, fleetSize * zOff);
            CreateCamFollower(leader, camOffset);
        }
    }
}