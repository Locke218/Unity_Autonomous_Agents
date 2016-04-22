//AUTHOR:Zane Draper (Teacher Assistance)
//
//PURPOSE: To control seekers (flock)
//
//******************************************

using UnityEngine;
using System.Collections;

public class Seeker : Vehicle {

    //VARIABLES************
	public GameObject seekerTarget;

	private Vector3 ultimateForce;
    //*********************

	// Call Inherited Start and then do our own
	override public void Start () {
		base.Start();

        //resets total force
		ultimateForce = Vector3.zero;

	}

	//Sums up all forces moving the seeker
	protected override void CalcSteeringForces(){
		Vector3 eggForce = Vector3.zero;
		Vector3 fleeForce = Vector3.zero;

		//sees if something is chasing the pokemon
		if ((fleeForce = Flee ()) != Vector3.zero) {

			ultimateForce += fleeForce * fleeWeight;

		} 
		//if not then see other forces
		else {
			//ignore leader if there is an egg to pick up
			if ((eggForce = SeekEggs ()) != Vector3.zero) {

				ultimateForce += eggForce * (seekWeight * 1.25f);

			} else {
				//SEEK
				ultimateForce += Seek (seekerTarget.transform.position + seekerTarget.transform.forward * -followDistance) * seekWeight;
			
				if (idNum == 3) {
					//COHESION
					ultimateForce += Cohesion (gm.cuboneCentroid) * cohesionWeight;
				} else if (idNum == 1) {
					//COHESION
					ultimateForce += Cohesion (gm.charmanderCentroid) * cohesionWeight;
				}

				//ALIGNMENT
				ultimateForce += Alignment () * alignWeight;
			}
		}
		
		//SEPARATION
		ultimateForce += (Separation (tooClose) * sepWeight);

		
		//Obstacle Avoid
		for(int k = 0; k < gm.Obstacles.Count; k++){
			ultimateForce += AvoidObstacle((GameObject)gm.Obstacles[k], safeDistance) * avoidWeight;
		}
        
		//divert towards center if too far to the edge
		ultimateForce += (StayInBounds() * inBoundWeight);

        //Keep from exceeding max
		ultimateForce = Vector3.ClampMagnitude (ultimateForce, maxForce);

        //Applies the actual force
		ApplyForce (ultimateForce);
	}

	//checks for entrance into the grass
	void OnTriggerEnter (Collider other){

		isInGrass = true;
	}

	//checks for exit of grass
	void OnTriggerExit (Collider other){
		
		isInGrass = false;
	}
}
