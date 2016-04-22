//AUTHOR:Zane Draper (Teacher Assistance)
//
//PURPOSE: Control Game
//
//******************************************

using UnityEngine;
using System.Collections;

//add using System.Collections.Generic; to use the generic list format
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //VARIABLES*****************
	public GameObject pathPrefab;
	public GameObject charmeleonPrefab;
	public GameObject cubonePrefab;
	public GameObject marowakPrefab;
	public GameObject charmanderPrefab;
	public GameObject eggPrefab;
	public GameObject pokeCenterPrefab;
	public GameObject trainerPrefab;
	public GameObject cuboneCentroidPre;
	public GameObject charmanderCentroidPre;

	public Terrain terrain;
	private int camSet = 0;

	private ArrayList paths = new ArrayList();
	private ArrayList cubones = new ArrayList();
	private ArrayList charmanders = new ArrayList();
	private ArrayList eggs = new ArrayList();
	public float pathRadius = 1f;
	public Vector3 cuboneCentroid;
	public Vector3 charmanderCentroid;
	
	private GameObject marowak;
	public GameObject charmeleon;
	public GameObject trainer;
	public ArrayList Paths
    {
        get { return paths; }
	}
	public ArrayList Cubones
	{
		get { return cubones; }
	}
	public ArrayList Charmanders
	{
		get { return charmanders; }
	}
	public ArrayList Eggs
	{
		get { return eggs; }
	}

    public GameObject obstaclePrefab;
    public GameObject obstaclePrefab2;

	private ArrayList obstacles = new ArrayList();


	public ArrayList Obstacles
    {
        get { return obstacles; }
    }
	//*************************

    //Called on creation
	void Start () {

        //Temporary Variable
        Vector3 pos = new Vector3(0, 0, 0);
        Vector3 startP = new Vector3(0, .5f, 0);
        Vector3 endP = new Vector3(0, .5f, 0);

		//Creates path objects*********************
        startP = new Vector3(110, 1, -115);
        endP = new Vector3(-65, 1, -125);
        GameObject next0 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next0.GetComponent<Path>().startP = startP;
		next0.GetComponent<Path>().endP = endP;
		paths.Add (next0);

		startP = new Vector3(-65, 1, -125);
		endP = new Vector3(-60, 1, 10);
		GameObject next1 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next1.GetComponent<Path>().startP = startP;
		next1.GetComponent<Path>().endP = endP;
		paths.Add (next1);

		startP = new Vector3(-60, 1, 10);
		endP = new Vector3(35, 1, 15);
		GameObject next2 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next2.GetComponent<Path>().startP = startP;
		next2.GetComponent<Path>().endP = endP;
		paths.Add (next2);

		startP = new Vector3(35, 1, 15);
		endP = new Vector3(40, 1, 70);
		GameObject next3 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next3.GetComponent<Path>().startP = startP;
		next3.GetComponent<Path>().endP = endP;
		paths.Add (next3);

		startP = new Vector3(40, 1, 70);
		endP = new Vector3(-50, 1, 90);
		GameObject next4 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next4.GetComponent<Path>().startP = startP;
		next4.GetComponent<Path>().endP = endP;
		paths.Add (next4);

		startP = new Vector3(-50, 1, 90);
		endP = new Vector3(-45, 1, 115);
		GameObject next5 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next5.GetComponent<Path>().startP = startP;
		next5.GetComponent<Path>().endP = endP;
		paths.Add (next5);

		startP = new Vector3(-45, 1, 115);
		endP = new Vector3(105, 1, 110);
		GameObject next6 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next6.GetComponent<Path>().startP = startP;
		next6.GetComponent<Path>().endP = endP;
		paths.Add (next6);
		
		startP = new Vector3(105, 1, 110);
		endP = new Vector3(115, 1, -5);
		GameObject next7 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next7.GetComponent<Path>().startP = startP;
		next7.GetComponent<Path>().endP = endP;
		paths.Add (next7);
		
		startP = new Vector3(115, 1, -5);
		endP = new Vector3(110, 1, -115);
		GameObject next8 = (GameObject)Instantiate(pathPrefab, new Vector3(0, 1, 0), Quaternion.identity);
		next8.GetComponent<Path>().startP = startP;
		next8.GetComponent<Path>().endP = endP;
		paths.Add (next8);
		//***************************************************


        //Position and what theyre moving towards
		pos = new Vector3(Random.Range(-35, -30), 0, Random.Range(-35, -30));
		marowak = (GameObject)Instantiate(marowakPrefab, pos, Quaternion.identity);
		
		pos = new Vector3(Random.Range(35, 30),  0, Random.Range(35, 30));
		charmeleon = (GameObject)Instantiate(charmeleonPrefab, pos, Quaternion.identity);
		
		pos = new Vector3(Random.Range(140, 145),  0, Random.Range(140, 145));
		trainer = (GameObject)Instantiate(trainerPrefab, pos, Quaternion.identity);
		
		//Initializing the flockers
		for (int i = 0; i < 8; i++)
		{
			//Position and what theyre moving towards
			pos = new Vector3(Random.Range(-45, -35), -.2f, Random.Range(-45, -35));
			GameObject dud = (GameObject)Instantiate(cubonePrefab, pos, Quaternion.identity);
			dud.GetComponent<Seeker>().seekerTarget = marowak;
			Cubones.Add(dud);
		}

		//Initializing the flockers
		for (int i = 0; i < 8; i++)
		{
			//Position and what theyre moving towards
			pos = new Vector3(Random.Range(45, 35), 0, Random.Range(45, 35));
			GameObject dud = (GameObject)Instantiate(charmanderPrefab, pos, Quaternion.identity);
			dud.GetComponent<Seeker>().seekerTarget = charmeleon;
			Charmanders.Add(dud);
		}
		
		//Initializing the flockers
		for (int i = 0; i < 8; i++)
		{
			//Position and what theyre moving towards
			pos = new Vector3(Random.Range(-140, 140), 0, Random.Range(-140, 140));
			pos.y = terrain.SampleHeight(pos);
			GameObject eg = (GameObject)Instantiate(eggPrefab, pos, Quaternion.identity);
			Eggs.Add(eg);
		}

        //Setting target to prefab
		//target = (GameObject) Instantiate(targetPrefab, pos, Quaternion.identity);


		//Camera will follow the GameManagerGO Cube 
		Camera.main.GetComponent<SmoothFollow>().target = trainer.transform;

        //Initializing the two types of obstacles
        for (int i = 0; i < 30; i++)
        {
			pos = new Vector3(Random.Range(-140, 140), 0, Random.Range(-140, 140));
			Quaternion rot = Quaternion.Euler(0, Random.Range(0, 180), 0);
			pos.y = terrain.SampleHeight(pos);
            obstacles.Add((GameObject)Instantiate(obstaclePrefab, pos, rot));
        }

		//creates tree obstacles
        for (int i = 30; i < 45; i++)
        {
			pos = new Vector3(Random.Range(-140, 140), 0, Random.Range(-140, 140));
			Quaternion rot = Quaternion.Euler(0, Random.Range(0, 180), 0);
			pos.y = terrain.SampleHeight(pos);
			obstacles.Add((GameObject)Instantiate(obstaclePrefab2, pos, rot));
        }

	}
	
    //Runs every Frame
	void Update () {

		//switch between cam targets
		if (Input.GetKeyDown (KeyCode.Space)) {
			if(camSet == 0){
				camSet = 1;
				Camera.main.GetComponent<SmoothFollow>().target = charmeleon.transform;
			}
			else if(camSet == 1){
				camSet = 2;
				Camera.main.GetComponent<SmoothFollow>().target = marowak.transform;
			}
			else if(camSet == 2){
				camSet = 0;
				Camera.main.GetComponent<SmoothFollow>().target = trainer.transform;
			}
			else if(camSet == 2){
				camSet = 0;
				Camera.main.GetComponent<SmoothFollow>().target = trainer.transform;
			}

		}


        //Calculates the center of the flockers
        CalcCentroids();

		
		//Keeps the position and direction with the flock
		cuboneCentroidPre.transform.position = (cuboneCentroid + new Vector3(0, 1, 0));
		cuboneCentroidPre.transform.forward = CalcFlockDirection(cubones);

		//Keeps the position and direction with the flock
		charmanderCentroidPre.transform.position = (charmanderCentroid + new Vector3(0, 1, 0));
		charmanderCentroidPre.transform.forward = CalcFlockDirection(charmanders);
	}

    

    //Finds the Center of the flock
    private void CalcCentroids(){
		Vector3 center = new Vector3();
		
		//Adds the positions
		for (int i = 0; i < Cubones.Count; i++)
		{
			center += ((GameObject)Cubones[i]).transform.position;
		}
		
		//Finds the average location (center)
		cuboneCentroid = (center / Cubones.Count);
		
		center = Vector3.zero;
		
		//Adds the positions
		for (int i = 0; i < Charmanders.Count; i++)
		{
			center += ((GameObject)Charmanders[i]).transform.position;
		}
		
		//Finds the average location (center)
		charmanderCentroid = (center / Charmanders.Count);
    }


    //Finds the average forward direction of the flock
    private Vector3 CalcFlockDirection(ArrayList pokemon){
        Vector3 totalForward = new Vector3(0, 0, 0);

        //adds the forwards together
        for (int i = 0; i < pokemon.Count; i++)
        {
			totalForward += ((GameObject)pokemon[i]).transform.forward;
        }

        //Normalizes for the final direction
        totalForward.Normalize();

        //Returns new forward direction
        return totalForward;
    }

	//if pokemon approaches egg, hatch into that pokemon
	public void HatchEgg(int eggNum, int hatcherNum){
		Vector3 pos = new Vector3 (((GameObject)Eggs [eggNum]).transform.position.x, .4f, ((GameObject)Eggs [eggNum]).transform.position.z);

		//remove egg prefab
		Destroy ((GameObject)Eggs[eggNum]);
		Eggs.RemoveAt(eggNum);

		//place a new pokemon based on who 
		if (hatcherNum == 3) {
			pos = new Vector3 (pos.x, pos.y, pos.z);
			GameObject dud = (GameObject)Instantiate (cubonePrefab, pos, Quaternion.identity);
			dud.GetComponent<Seeker> ().seekerTarget = marowak;
			Cubones.Add (dud);
		}
		
		else if (hatcherNum == 1) {
			pos = new Vector3 (pos.x, pos.y, pos.z);
			GameObject dud = (GameObject)Instantiate (charmanderPrefab, pos, Quaternion.identity);
			dud.GetComponent<Seeker> ().seekerTarget = charmeleon;
			Charmanders.Add (dud);
		}

		//spawn new egg
		if (eggs.Count < 6) {
			pos = new Vector3 (Random.Range(-50, 50), .4f, Random.Range(-50, 50));
			eggs.Add ((GameObject)Instantiate (eggPrefab, pos, Quaternion.identity));
		}
	}

}
