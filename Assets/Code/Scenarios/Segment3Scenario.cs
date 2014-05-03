using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.Scenarios
{
	
	class Segment3Scenario : Scenario
	{
		public override string Description()
		{
			return "Segment 3, Ride Along";
		}
		
		public override void Start()
		{
			GroundEnabled(true);

			// Create obstacle course to traverse
			int obstacleheight = 50;
			int groundlevel = 100;
			Vector3 temp = GameObject.FindGameObjectWithTag ("ground").transform.position;
			temp.y = groundlevel - 4;
			GameObject.FindGameObjectWithTag ("ground").transform.position = temp;

			Params.Load("default.txt");
			Params.camMode = (int) Params.camModes.leadersideview;

			// Create a leader for the fleet and configure which steering behaviours it should have
			leader = CreateBoid(new Vector3(10, groundlevel, 0), leaderPrefab);
			leader.tag = "leader";
			leader.GetComponent<SteeringBehaviours>().ArriveEnabled = true;
			leader.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
			leader.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
			leader.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(10, groundlevel + 1, 5000);

			secondcar = CreateBoid(new Vector3(-10, groundlevel, 0), boidPrefab);
			secondcar.tag = "boid";
			secondcar.GetComponent<SteeringBehaviours>().ArriveEnabled = true;
			secondcar.GetComponent<SteeringBehaviours>().ObstacleAvoidanceEnabled = true;
			secondcar.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;
			secondcar.GetComponent<SteeringBehaviours>().seekTargetPos = new Vector3(-10, groundlevel + 1, 5000);

			int slalomlength = 50;

			for(int i = 0; i < slalomlength; i++)
			{
				int tempdist = i * 50;
				CreateObstacle(new Vector3(0, groundlevel, tempdist), obstacleheight, 5);
			}
		}
	}
}