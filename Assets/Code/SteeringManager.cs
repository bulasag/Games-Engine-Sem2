using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BGE.Scenarios;
using BGE.States;

namespace BGE
{
    public class SteeringManager : MonoBehaviour
    {
        List<Scenario> scenarios = new List<Scenario>();
		float titleDuration = 15.0f;
		float eventTimer = 0.0f;
		bool scene1 = false; //Leader Slalom
		bool scene2 = false; //Boid Death Scenario
		bool scene3 = false; //Ride Along Scenario
		bool scene4 = false; //Boid Slalom
		bool scene5 = false; //Leader Slalom Redux
		bool scene6 = false; //Ride Along Slalom

        public Scenario currentScenario;
        StringBuilder message = new StringBuilder();
        
        public GameObject camFighter;

        public GameObject boidPrefab;
        public GameObject leaderPrefab;
		public GameObject racer3Prefab;
        public Space space;
        static SteeringManager instance;
        // Use this for initialization
		GUIStyle style = new GUIStyle();
		GUIStyle introstyle = new GUIStyle();
		GUIStyle endingstyle = new GUIStyle();

        float[] timeModifiers = {0.0f, 5.0f, 10.0f};
        int timeModIndex = 0;
        
        GameObject monoCamera;
        GameObject activeCamera;
        GameObject riftCamera;
        
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
			Application.targetFrameRate = 60;
			QualitySettings.vSyncCount = 0;

			GameObject.FindGameObjectWithTag ("finishline").renderer.enabled = false;
			GameObject.FindGameObjectWithTag ("finishtexture").guiTexture.enabled = false;
			GameObject.FindGameObjectWithTag ("explosionframe").guiTexture.enabled = false;

            instance = this;
            Screen.showCursor = false;

            style.fontSize = 18;
            style.normal.textColor = Color.white;

            space = new Space();
            
			scenarios.Add(new StartingLineScenario());
			scenarios.Add (new Segment1Scenario ());
			scenarios.Add (new BoidDeathScenario ());
			scenarios.Add (new Segment3Scenario ());
			scenarios.Add (new BoidAvoidanceScenario ());
			scenarios.Add (new RideAlongSlalomScenario ());
			scenarios.Add (new SeekScenario());
			scenarios.Add(new PathFindingScenario());
			scenarios.Add(new StateMachineScenario());
			scenarios.Add (new ObstacleAvoidanceScenario ());
            currentScenario = scenarios[0];
            currentScenario.Start();

            monoCamera = GameObject.FindGameObjectWithTag("MainCamera");
            riftCamera = GameObject.FindGameObjectWithTag("ovrcamera");

            activeCamera = monoCamera;
        }

        public static SteeringManager Instance
        {
            get
            {
                return instance;
            }
        }

        void OnGUI()
        {
			if (titleDuration > 0)
			{
				titleDuration -= Time.smoothDeltaTime;

				introstyle.fontSize = (int)(Screen.height * 0.05f);
				introstyle.normal.textColor = Color.white;
				introstyle.alignment = TextAnchor.MiddleCenter;

				GUI.Label (new Rect (Screen.width * 0.80f, 10, Screen.width * 0.1f, Screen.width * 0.1f), "Games Engine 2 \n Inno Bulasag \n Wipeout Tribute", introstyle);
			
				introstyle.fontSize = (int)(Screen.height * 0.1f);
				introstyle.normal.textColor = Color.red;
				string countdown = ((int)(titleDuration) - 5).ToString();

				if (titleDuration > 6)
				{
					GUI.Label (new Rect (Screen.width * 0.45f, 10.0f, Screen.width * 0.1f, Screen.width * 0.1f), countdown, introstyle);
				}
				else if (titleDuration < 6)
				{
					GUI.Label (new Rect (Screen.width * 0.45f, 10.0f, Screen.width * 0.1f, Screen.width * 0.1f), "GO!", introstyle);
				}
			}

			if (Params.showMessages)
            {
                GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "" + message, style);
            }
            if (Event.current.type == EventType.Repaint)
            {
                message.Length = 0;
            }

            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.F1)
                {
                    Params.camMode = (Params.camMode + 1) % 5;
                }

                for (int i = 0; i < scenarios.Count; i++)
                {
                    if (Event.current.keyCode == KeyCode.Alpha0 + i)
                    {
                        currentScenario.TearDown();
                        currentScenario = scenarios[i];
                        currentScenario.Start();                        
                    }
                }

                if (Event.current.keyCode == KeyCode.F2)
                {
                    timeModIndex = (timeModIndex + 1) % timeModifiers.Length;
                }

                if (Event.current.keyCode == KeyCode.F4)
                {
                    Params.showMessages = !Params.showMessages;
                }

                if (Event.current.keyCode == KeyCode.F5)
                {
                    Params.drawVectors = !Params.drawVectors;
                }
                
                if (Event.current.keyCode == KeyCode.F6)
                {
                    Params.drawDebugLines = !Params.drawDebugLines;
                }

                if (Event.current.keyCode == KeyCode.F7)
                {
                    monoCamera.transform.up = Vector3.up;
                }

                if (Event.current.keyCode == KeyCode.F9)
                {
                    Params.enforceNonPenetrationConstraint = !Params.enforceNonPenetrationConstraint;
                }

                if (Event.current.keyCode == KeyCode.F11)
                {
                    Params.drawForces = !Params.drawForces;
                }                                

                if (Event.current.keyCode == KeyCode.Escape)
                {
                    Application.Quit();
                }                
            }
        }

        public static void PrintMessage(string message)
        {
            if (instance != null)
            {
                Instance.message.Append(message + "\n");
            }
        }

        public static void PrintFloat(string message, float f)
        {
            if (instance != null)
            {
                Instance.message.Append(message + ": " + f + "\n");
            }
        }

        public static void PrintVector(string message, Vector3 v)
        {
            if (instance != null)
            {
                Instance.message.Append(message + ": (" + v.x + ", " + v.y + ", " + v.z + ")\n");
            }
        }

        // Update is called once per frame
        void Update()
        {
			if (titleDuration < 6)
			{
				eventTimer += Time.smoothDeltaTime;
				timeModIndex = 1;
				//GameObject.FindGameObjectWithTag ("runsound").transform.position = GameObject.FindGameObjectWithTag ("MainCamera").transform.position;
			}

			if ((int)eventTimer == 5)
			{
				if(scene1 == false)
				{
					currentScenario.TearDown();
					currentScenario = scenarios[1];
					currentScenario.Start();
					scene1 = true;
				}
			}

			if ((int)eventTimer == 20)
			{
				if(scene2 == false)
				{
					currentScenario.TearDown();
					currentScenario = scenarios[2];
					currentScenario.Start();
					scene2 = true;
				}
			}
			
			if ((int)eventTimer == 22)
			{
				if(scene3 == false)
				{
					currentScenario.TearDown();
					currentScenario = scenarios[3];
					currentScenario.Start();
					scene3 = true;
					GameObject.FindGameObjectWithTag ("explosionframe").guiTexture.enabled = true;
				}
			}

			if ((int)eventTimer == 23)
			{
				GameObject.FindGameObjectWithTag ("explosionframe").guiTexture.enabled = false;
			}

			if ((int)eventTimer == 30)
			{
				if(scene3 == true)
				{
					Params.camMode = (int) Params.camModes.boidsideview;
				}
			}

			if ((int)eventTimer == 35)
			{
				if(scene4 == false)
				{
					currentScenario.TearDown();
					currentScenario = scenarios[4];
					currentScenario.Start();
					scene4 = true;
				}
			}

			if ((int)eventTimer == 48)
			{
				if(scene5 == false)
				{
					currentScenario.TearDown();
					currentScenario = scenarios[1];
					currentScenario.Start();
					scene5 = true;
				}
			}

			if ((int)eventTimer == 62)
			{
				if(scene6 == false)
				{
					currentScenario.TearDown();
					currentScenario = scenarios[5];
					currentScenario.Start();
					scene6 = true;
				}
			}

			if ((int)eventTimer == 65)
			{
				if(scene6 == true)
				{
					Params.camMode = (int) Params.camModes.boidsideview;
				}
			}

			if ((int)eventTimer == 70)
			{
				if(scene6 == true)
				{
					Params.camMode = (int) Params.camModes.leadersideview;
				}
			}

			if ((int)eventTimer == 75)
			{
				if(scene6 == true)
				{
					Params.camMode = (int) Params.camModes.sidebyside;
				}
			}

			if ((int)eventTimer == 80)
			{
				if(scene6 == true)
				{
					GameObject.FindGameObjectWithTag ("finishtexture").guiTexture.enabled = true;
				}
			}
			
			Params.timeModifier = timeModifiers[timeModIndex];
            
			if (Params.riftEnabled)
            {
                riftCamera.SetActive(true);
                activeCamera = riftCamera;
            }
            else
            {
                riftCamera.SetActive(false);
                activeCamera = monoCamera;
            }

			PrintMessage (Application.targetFrameRate.ToString () + " " + QualitySettings.vSyncCount.ToString ());
			PrintMessage("Timer: " + eventTimer);
			PrintMessage ("TimeModIndex: " + timeModIndex + " CamMode: " + Params.camMode);
            PrintMessage("Press F1 to toggle camera mode");
            PrintMessage("Press F2 to adjust speed");
            PrintMessage("Press F4 to toggle messages");
            PrintMessage("Press F5 to toggle vector drawing");
            PrintMessage("Press F6 to toggle debug drawing");
            PrintMessage("Press F7 to level camera");
            PrintMessage("Press F8 to toggle cell space partitioning");
            PrintMessage("Press F9 to toggle non-penetration constraint");
            PrintMessage("Press F10 to toggle Rift");
            PrintMessage("Press F11 to toggle force drawing");
            int fps = (int)(1.0f / Time.smoothDeltaTime);
            PrintFloat("FPS: ", fps);
            PrintMessage("Current scenario: " + currentScenario.Description());
            for (int i = 0; i < scenarios.Count; i++)
            {
                PrintMessage("Press " + i + " for " + scenarios[i].Description());
            }
           
            switch (Params.camMode)
            {
                case((int) Params.camModes.following):
                    currentScenario.leader.GetComponentInChildren<Renderer>().enabled = true;
                    monoCamera.transform.position = camFighter.transform.position;
                    monoCamera.transform.rotation = camFighter.transform.rotation;
                   break;
                
				case ((int)Params.camModes.boid):
                    currentScenario.leader.GetComponentInChildren<Renderer>().enabled = false;
                    //monoCamera.transform.position = currentScenario.leader.transform.position;
					monoCamera.transform.position = new Vector3(currentScenario.leader.transform.position.x,
				                                        currentScenario.leader.transform.position.y + 1.0f,
				                                        currentScenario.leader.transform.position.z - 2.0f);

                    monoCamera.transform.rotation = currentScenario.leader.transform.rotation;
               	break;
                
				case ((int)Params.camModes.fps):
                   currentScenario.leader.GetComponentInChildren<Renderer>().enabled = true;
                break;

				case ((int)Params.camModes.leadersideview):
					currentScenario.leader.GetComponentInChildren<Renderer>().enabled = false;
					//monoCamera.transform.position = currentScenario.leader.transform.position;
					monoCamera.transform.position = new Vector3(currentScenario.leader.transform.position.x + 10.0f,
					                                            currentScenario.leader.transform.position.y + 1.0f,
					                                            currentScenario.leader.transform.position.z - 2.0f);
					
					monoCamera.transform.LookAt(currentScenario.secondcar.transform.position);
				break;
				
				case ((int)Params.camModes.boidsideview):
					currentScenario.secondcar.GetComponentInChildren<Renderer>().enabled = false;
					//monoCamera.transform.position = currentScenario.secondcar.transform.position;
					monoCamera.transform.position = new Vector3(currentScenario.secondcar.transform.position.x - 15.0f,
					                                            currentScenario.secondcar.transform.position.y + 1.0f,
					                                            currentScenario.secondcar.transform.position.z - 2.0f);
					
					monoCamera.transform.LookAt(currentScenario.leader.transform.position);
				break;

				case ((int)Params.camModes.sidebyside):
					currentScenario.secondcar.GetComponentInChildren<Renderer>().enabled = false;
					monoCamera.transform.position = new Vector3(0,
					                                            currentScenario.leader.transform.position.y + 50.0f,
					                                            currentScenario.leader.transform.position.z - 50.0f);
					
					Vector3 temptarget;
					temptarget.x = 0;
					temptarget.y = currentScenario.leader.transform.position.y;
					temptarget.z = currentScenario.leader.transform.position.z + 100;
					monoCamera.transform.LookAt(temptarget);
				break;
			}
			
			if (Params.enforceNonPenetrationConstraint)
            {
                PrintMessage("Enforce non penetration constraint on");
            }
            else
            {
                PrintMessage("Enforce non penetration constraint off");
            }

            if (Params.drawDebugLines && Params.cellSpacePartitioning)
            {
                space.Draw();
            }
      
            currentScenario.Update();
		}
		
		void LateUpdate()
		{

        }
    }
}
