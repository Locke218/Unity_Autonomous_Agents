//AUTHOR:Zane Draper (Teacher Assistance)
//
//PURPOSE: Assess forces
//
//******************************************

using UnityEngine;
using System.Collections;

//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

abstract public class Vehicle : MonoBehaviour {

    //VARIABLES******************

    protected Vector3 acceleration;
    protected Vector3 velocity;
	protected Vector3 desired;

	protected bool isInGrass = false;
	
	protected int pokeballs = 6;

    protected Vector3 predictedLocation;

    public int followDistance = 0;

    public float maxSpeed;
    public float maxForce;
    public float mass;
    public float radius;
    public float tooClose;
    public bool forceYes = true;

    public int idNum = 0;
	private Vector3 wanderVector = Vector3.zero;
	private int wanderCount = 0;

    public Vector3 Velocity {
        get { return velocity; }
    }

    CharacterController charControl;

    protected GameManager gm;
    public float safeDistance;
    public float avoidWeight;
    public float sepWeight;
    public float seekWeight;
    public float alignWeight;
	public float cohesionWeight;
	public float inBoundWeight;
	public float fleeWeight;


    //***************************


    //Runs at creation
    virtual public void Start() {


        //Sets vectors
        acceleration = Vector3.zero;
        velocity = transform.forward;
        desired = Vector3.zero;

        //Needed Variables
        charControl = GetComponent<CharacterController>();
        gm = GameObject.Find("GameManagerGO").GetComponent<GameManager>();

    }


    // Update is called once per frame
    protected void Update() {
        //Adds all steering forces
        CalcSteeringForces();

        //Increases or decreases the velocity
        velocity += acceleration * Time.deltaTime;

        //flattens the y direction
        velocity.y = 0;

        //Can't exceed max velocity
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //Controls character movement
        charControl.Move(Velocity * Time.deltaTime);

        //resets acceleration
        acceleration = Vector3.zero;

        //Keeps character facing right direction
        transform.forward = velocity.normalized;

        if (Input.GetKeyDown("space")) {
            if (forceYes == true) forceYes = false;
            else forceYes = true;
        }


    }

    //Needs to be used in chioldren
    abstract protected void CalcSteeringForces();

    //Sets the acceleration
    protected void ApplyForce(Vector3 steeringForce) {
        acceleration += steeringForce / mass;
    }


    //Finds the force towards a desired position
    protected Vector3 Seek(Vector3 targetPosition) {
        desired = targetPosition - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steeringForce = desired - velocity;

        return steeringForce;
    }

    //Negates Seek, so the force is away from desired position
    protected Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 fleeV = Seek(targetPosition);
        fleeV.x *= -1;
        fleeV.z *= -1;

        return (fleeV);
    }

	protected Vector3 Flee()
	{
		Vector3 fleeV = Vector3.zero;
		
		float dist = Vector3.Distance(gm.trainer.transform.position, transform.position);
		if (dist < 30) {
			fleeV = Seek (gm.trainer.transform.position);
			fleeV.x *= -1;
			fleeV.z *= -1;
		}
		
		return (fleeV);
	}

    //Avoid things in path
    protected Vector3 AvoidObstacle(GameObject ob, float safe) {

        desired = Vector3.zero;

        //Radius of obstacles
        float obRad = ob.GetComponent<ObstacleScript>().Radius;

        //Distance from character center to obstacle center
        Vector3 vecToCenter = ob.transform.position - transform.position;
        vecToCenter.y = 0;

        //Back out if too far away
        if (vecToCenter.magnitude > safe) {
            return Vector3.zero;
        }

        //Back out if behind
        if (Vector3.Dot(vecToCenter, transform.forward) < 0) {
            return Vector3.zero;
        }

        //Back out if too far to sides
        if (Mathf.Abs(Vector3.Dot(vecToCenter, transform.right)) > obRad + radius) {
            return Vector3.zero;
        }

        //If to right
        if (Vector3.Dot(vecToCenter, transform.right) < 0) {
            desired = transform.right * maxSpeed;

            Debug.DrawLine(transform.position, ob.transform.position, Color.red);
        }

        //if to left
        else {
            desired = transform.right * -maxSpeed;

            Debug.DrawLine(transform.position, ob.transform.position, Color.green);
        }

        //Returns the desired movement
        return desired;
    }

	//For leader who is following the path
    protected Vector3 PathFollow(Path p, Vector3 nP)
    {
        Vector3 followV = Vector3.zero;
        float distance = Vector3.Distance(predictedLocation, nP);

        if (distance > gm.pathRadius) {
            Vector3 B = p.B.normalized;
            B *= 25;
            nP += B;
            followV = Seek(nP);

            //Debug.DrawLine(predictedLocation, nP, Color.green);
        }

        return followV;
    }

	//Finds closest line
    public Vector3 FindNormalPoint(Vector3 point, Path p)
    {
        Vector3 A = point - p.startP;

        //Debug.DrawLine(point, p.startP, Color.blue);

        Vector3 C = p.endP - p.startP;
        C.Normalize();

        C *= (Vector3.Dot(A, C));
        //Debug.DrawLine(p.endP, C, Color.red);

        Vector3 normalPoint = p.startP + C;

        //Debug.DrawLine(p.startP, normalPoint, Color.cyan);

        return normalPoint;
    }

	//seeks closest path
    public Vector3 CalculatePath()
    {
        Vector3 pathF = Vector3.zero;

        predictedLocation = transform.position + (transform.forward * 2);

        int j = 0;
        float shortestDist = 100;
        Vector3 normalPoint = Vector3.zero;

        //Find closest path
        for (int i = 0; i < gm.Paths.Count; i++) {
            Path p = ((GameObject)gm.Paths[i]).GetComponent<Path>();

            Vector3 nP = FindNormalPoint(predictedLocation, p);

            //float distA = Vector3.Distance(p.startP, nP);
            //float distB = Vector3.Distance(p.endP, nP);
            //float distC = Vector3.Distance(p.startP, p.endP);

            if ((p.startP.x < nP.x && nP.x < p.endP.x) || (p.endP.x < nP.x && nP.x < p.startP.x))
            { }
            else {
                float distA = Vector3.Distance(predictedLocation, p.startP);
                float distB = Vector3.Distance(predictedLocation, p.endP);

                if (distA < distB)
                {
                    nP = p.startP;
                }
                else
                {
                    nP = p.endP;
                }
            }

            float distance = Vector3.Distance(predictedLocation, nP);

            Debug.DrawLine(predictedLocation, nP, Color.blue);

            if (distance < shortestDist) {
                shortestDist = distance;
                j = i;
                normalPoint = nP;
            }

        }

        //Once found, see if we need to move towards it
        pathF = PathFollow(((GameObject)gm.Paths[j]).GetComponent<Path>(), normalPoint);


        return pathF;
    }

    //Keeps flock from moving into each other (Personal Bubble)
    public Vector3 Separation(float separationDistance) {

        //Initializing
        Vector3 totalVelocity = new Vector3(0, 0, 0);
        float dist;

        if (idNum == 3) {
            //Cycle through Dudes
            for (int i = 0; i < gm.Cubones.Count; i++) {
                //Find distance away
                dist = Vector3.Distance(((GameObject)gm.Cubones[i]).transform.position, transform.position);
                dist = Mathf.Abs(dist);

                //If distance is close, but not identicle to current position
                if ((dist < separationDistance) && (dist != 0)) {
                    //Keeps velocity proportional to distance
                    Vector3 tempVelocity = (Flee(((GameObject)gm.Cubones[i]).transform.position)).normalized;
                    tempVelocity.y = 0;
                    tempVelocity *= (1 / dist);
                    totalVelocity += tempVelocity;
                }
            }
        }

        else if (idNum == 1) {
            //Cycle through Dudes
            for (int i = 0; i < gm.Charmanders.Count; i++) {
                //Find distance away
                dist = Vector3.Distance(((GameObject)gm.Charmanders[i]).transform.position, transform.position);
                dist = Mathf.Abs(dist);

                //If distance is close, but not identicle to current position
                if ((dist < separationDistance) && (dist != 0)) {
                    //Keeps velocity proportional to distance
                    Vector3 tempVelocity = (Flee(((GameObject)gm.Charmanders[i]).transform.position)).normalized;
                    tempVelocity.y = 0;
                    tempVelocity *= (1 / dist);
                    totalVelocity += tempVelocity;
                }
            }
        }

        //return desired velocity
        return ((totalVelocity.normalized * maxSpeed) - velocity);
    }

    //Alligns flocker with flocks average direction
    public Vector3 Alignment() {

        //Initializing
        Vector3 totalForward = new Vector3(0, 0, 0);

        if (idNum == 3) {
            //Combines all forward vectors
            for (int i = 0; i < gm.Cubones.Count; i++) {
                totalForward += ((GameObject)gm.Cubones[i]).transform.forward;
            }
        }
        else if (idNum == 1) {
            //Combines all forward vectors
            for (int i = 0; i < gm.Charmanders.Count; i++) {
                totalForward += ((GameObject)gm.Charmanders[i]).transform.forward;
            }
        }

        //Normalize for average forward vector
        totalForward.Normalize();
        totalForward *= maxSpeed;

        //return desired velocity
        return (totalForward - velocity);
    }

    //Keeps the flock close together
    public Vector3 Cohesion(Vector3 cohesionVector) {

        //always moving towards center of group
        Vector3 towardsCenter = Seek(cohesionVector);

        return towardsCenter;
    }

    //Keeps flock from wandering out of bounds
	public Vector3 StayInBounds() {
		if (transform.position.x > 130 || transform.position.x < -130) {
			return Seek (Vector3.zero);
		}
		else if (transform.position.z > 130 || transform.position.z < -130) {
			return Seek (Vector3.zero);
		}

        return new Vector3();
    }

	//if pokemon, seek out the eggs placed on map
    public Vector3 SeekEggs() {
        int j = -1;
        float dist = 0;
        float currentDist = 20;
        for (int i = 0; i < gm.Eggs.Count; i++) {
            if ((dist = Vector3.Distance(((GameObject)gm.Eggs[i]).transform.position, transform.position)) < currentDist) {
                currentDist = dist;
                j = i;
            }
        }

        if (currentDist < 2) {
            gm.HatchEgg(j, idNum);
        }

        if (j == -1)
            return Vector3.zero;
        else {
            return Seek(((GameObject)gm.Eggs[j]).transform.position);
        }


    }

	//trainer is drawn to the nearest pokemon within their area of notice
    public Vector3 SeekPokemon() {

		int pokeType = 0;
		int num = -1;
		float currentDist = 30;
		float dist = 0;

		//cycles through pokemon seeing if anyone is close
		for (int i = 0; i < gm.Cubones.Count; i++) {
			if(((dist = Vector3.Distance(((GameObject)gm.Cubones[i]).transform.position, transform.position)) < currentDist) && ((GameObject)gm.Cubones[i]).GetComponent<Seeker>().isInGrass == false ){
				pokeType = 3;
				num = i;
				currentDist = dist;
			}
		}
			
		for (int i = 0; i < gm.Charmanders.Count; i++) {
			if( ( (dist = Vector3.Distance( ( (GameObject)gm.Charmanders[i]).transform.position, transform.position) ) < currentDist) && ( (GameObject)gm.Charmanders[i]).GetComponent<Seeker>().isInGrass == false){
				pokeType = 1;
				num = i;
				currentDist = dist;
			}
		}

		Vector3 returnForce = Vector3.zero;

		//if pokemon are close, seek them
		if(num != -1){
			if(currentDist < 4){
				if(pokeType == 3){
					returnForce = Seek (((GameObject)gm.Cubones[num]).transform.position);
					Destroy((GameObject)gm.Cubones[num]);
					gm.Cubones.RemoveAt(num);
					pokeballs++;
					return returnForce;
				}
				else if(pokeType == 1){
					returnForce = Seek (((GameObject)gm.Charmanders[num]).transform.position);
					Destroy((GameObject)gm.Charmanders[num]);
					gm.Charmanders.RemoveAt(num);
					pokeballs++;
					return returnForce;
				}
			}
			//Seek the right pokemon from the right list
			else{
				if(pokeType == 3)
					return Seek (((GameObject)gm.Cubones[num]).transform.position);
				else if(pokeType == 1)
					return Seek (((GameObject)gm.Charmanders[num]).transform.position);
			}
		}

        return Vector3.zero;
    }

	//moves to pokecenter when they are full of pokeballs
    public Vector3 SeekPokeCenter()
    {
        float dist = Vector3.Distance(gm.pokeCenterPrefab.transform.position, transform.position);

        if(dist < 5)
        {
            pokeballs = 0;
        }

        return Seek(gm.pokeCenterPrefab.transform.position);
    }

	//when no pokemon are near, wander
    public Vector3 Wander(){
		if (wanderCount > 100) {
			Vector3 f = transform.forward.normalized;
			Vector3 r = transform.right.normalized;
			float randNum = Random.Range(-20.0f, 20.0f);

			wanderVector = ((f * 5) + (r * randNum));
			wanderCount = 0;
		}

		wanderCount++;

		return Seek(wanderVector) + Seek(gm.charmeleon.transform.position)/2;
	}
}
