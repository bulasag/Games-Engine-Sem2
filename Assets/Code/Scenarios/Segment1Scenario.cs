using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BGE.Scenarios
{
	public class Segment1Scenario : Scenario
	{
        private PathFinder pathFinder;
        
        Vector3 targetPos;
        Vector3 startPos;

        bool lastPressed = false;

		bool recalculate = true;

        public override string Description()
        {
            return "Segment 1, Slalom";
        }

        public override void Start()
        {
			GroundEnabled(true);

			int groundlevel = 10;
			int obstacleheight = 30;
			int obstacleoffset = 480;
			Vector3 temp = GameObject.FindGameObjectWithTag ("ground").transform.position;
			temp.y = groundlevel - 4;
			GameObject.FindGameObjectWithTag ("ground").transform.position = temp;
			
			Params.camMode = (int) Params.camModes.boid;

            Params.Load("default.txt");

            targetPos = new Vector3(0, groundlevel + 1, 4000);
			lookPos = targetPos;
            startPos = new Vector3(10, groundlevel + 1, -20);

            leader = CreateBoid(startPos, leaderPrefab);
			leader.tag = "leader";

			CreateObstacle(new Vector3(15, groundlevel, 10), 30, 10);
			CreateObstacle(new Vector3(5, groundlevel, 50), 20, 12);
			CreateObstacle(new Vector3(15, groundlevel, 70), 50, 5);
			CreateObstacle(new Vector3(5, groundlevel, 30), obstacleheight, 7);
			CreateObstacle(new Vector3(-10, groundlevel, 80), obstacleheight, 17);
			CreateObstacle(new Vector3(10, groundlevel, 120), obstacleheight, 10);
			CreateObstacle(new Vector3(50, groundlevel, 150), obstacleheight, 12);
			CreateObstacle(new Vector3(-20, groundlevel, 200), obstacleheight, 20);
			CreateObstacle(new Vector3(-65, groundlevel, 300), obstacleheight, 30);
			CreateObstacle(new Vector3(20, groundlevel, 250), obstacleheight, 10);
			CreateObstacle(new Vector3(5, groundlevel, 350), obstacleheight, 45);
			CreateObstacle(new Vector3(90, groundlevel, 400), obstacleheight, 15);
			CreateObstacle(new Vector3(80, groundlevel, 420), obstacleheight, 15);
			CreateObstacle(new Vector3(-70, groundlevel, 430), obstacleheight, 40);
			CreateObstacle(new Vector3(70, groundlevel, 460), obstacleheight, 30);
			
			//Segm1 Inverse
			CreateObstacle(new Vector3(-15, groundlevel, obstacleoffset + 10), 30, 10);
			CreateObstacle(new Vector3(-5, groundlevel, obstacleoffset + 50), 20, 12);
			CreateObstacle(new Vector3(-15, groundlevel, obstacleoffset + 70), 50, 5);
			CreateObstacle(new Vector3(-5, groundlevel, obstacleoffset + 30), obstacleheight, 7);
			CreateObstacle(new Vector3(10, groundlevel, obstacleoffset + 80), obstacleheight, 17);
			CreateObstacle(new Vector3(-10, groundlevel, obstacleoffset + 120), obstacleheight, 10);
			CreateObstacle(new Vector3(-50, groundlevel, obstacleoffset + 150), obstacleheight, 12);
			CreateObstacle(new Vector3(20, groundlevel, obstacleoffset + 200), obstacleheight, 20);
			CreateObstacle(new Vector3(-30, groundlevel, obstacleoffset + 250), obstacleheight, 30); // Death Trap
			CreateObstacle(new Vector3(65, groundlevel, obstacleoffset + 300), obstacleheight, 30);
			CreateObstacle(new Vector3(-5, groundlevel, obstacleoffset + 350), obstacleheight, 45);
			CreateObstacle(new Vector3(-90, groundlevel, obstacleoffset + 400), obstacleheight, 15);
			CreateObstacle(new Vector3(-80, groundlevel, obstacleoffset + 420), obstacleheight, 15);
			CreateObstacle(new Vector3(70, groundlevel, obstacleoffset + 430), obstacleheight, 40);
			CreateObstacle(new Vector3(-70, groundlevel, obstacleoffset + 460), obstacleheight, 30);
			
            pathFinder = new PathFinder();
			pathFinder.Smooth = true;

            Path path = pathFinder.FindPath(startPos, targetPos);
            path.Looped = false;
            path.draw = false;
            leader.GetComponent<SteeringBehaviours>().path = path;
            leader.GetComponent<SteeringBehaviours>().FollowPathEnabled = true;
			leader.GetComponent<SteeringBehaviours>().PlaneAvoidanceEnabled = true;

            CreateCamFollower(leader, new Vector3(0, 5, -10));
        }

        public override void Update()
        {

            SteeringManager.PrintMessage("Press P to toggle smooth paths");
            SteeringManager.PrintMessage("Press O to toggle 3D paths");

            if (Input.GetKeyDown(KeyCode.P) && ! lastPressed)
            {
                pathFinder.Smooth = !pathFinder.Smooth;
                recalculate = true;
            }

            if (Input.GetKeyDown(KeyCode.O) && !lastPressed)
            {
                pathFinder.isThreeD = !pathFinder.isThreeD;
                recalculate = true;
            }

            GameObject camera = (GameObject) GameObject.FindGameObjectWithTag("MainCamera");

            if (recalculate)
            {
                Path path = pathFinder.FindPath(startPos, targetPos);
                if (path.Waypoints.Count == 0)
                {
                    leader.GetComponent<SteeringBehaviours>().TurnOffAll();
                }
                else
                {
                    leader.GetComponent<SteeringBehaviours>().FollowPathEnabled = true;
                }
                leader.GetComponent<SteeringBehaviours>().path = path;
                leader.GetComponent<SteeringBehaviours>().path.draw = false;
                leader.GetComponent<SteeringBehaviours>().path.Next = 0;
            }
			recalculate = false;

            if (Input.anyKeyDown)
            {
                lastPressed = true;
            }
            else
            {
                lastPressed = false;
            }

            if (pathFinder.message != "")
            {
                SteeringManager.PrintMessage(pathFinder.message);
            }

            base.Update();
        }
	}
}
