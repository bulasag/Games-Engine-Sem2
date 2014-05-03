using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.Scenarios
{
	
	class RideAlongSlalomScenario : Scenario
	{
		public override string Description()
		{
			return "Finale, Ride Along Finish";
		}
		
		public override void Start()
		{
			GameObject.FindGameObjectWithTag ("finishline").renderer.enabled = true;
			GroundEnabled(true);
			
			// Create obstacle course to traverse
			int obstacleheight = 50;
			int groundlevel = 100;
			Vector3 temp = GameObject.FindGameObjectWithTag ("ground").transform.position;
			temp.y = groundlevel - 4;
			GameObject.FindGameObjectWithTag ("ground").transform.position = temp;
			
			Params.Load("default.txt");
			Params.camMode = (int) Params.camModes.sidebyside;
			
			// Create a leader for the fleet and configure which steering behaviours it should have
			leader = CreateBoid(new Vector3(40, groundlevel, -50), leaderPrefab);
			leader.tag = "leader";
			leader.GetComponent<SteeringBehaviours>().ArriveEnabled = true;
			leader.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
			leader.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
			leader.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(40, groundlevel + 1, 5000);
			
			secondcar = CreateBoid(new Vector3(-40, groundlevel, -50), boidPrefab);
			secondcar.tag = "boid";
			secondcar.GetComponent<SteeringBehaviours>().ArriveEnabled = true;
			secondcar.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
			secondcar.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
			secondcar.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(-40, groundlevel + 1, 5000);
			
			int slalomlength = 20;

			CreateObstacle (new Vector3(0, groundlevel, 0), obstacleheight, 30);

			for(int i = 2; i < slalomlength; i++)
			{
				int tempdist = i * 50;
				CreateObstacle(new Vector3(0, groundlevel, tempdist), obstacleheight, 5);
			}
		}
	}
}