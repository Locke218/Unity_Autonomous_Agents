//AUTHOR:Zane Draper
//
//PURPOSE: Directs a leader pokemon
//
//******************************************

using UnityEngine;
using System.Collections;

public class Leader: Vehicle {

    //VARIABLES************
    public GameObject seekerTarget;


    private Vector3 ultimateForce;
    //*********************

    // Call Inherited Start and then do our own
    override public void Start()
    {
        base.Start();

        //resets total force
        ultimateForce = Vector3.zero;

    }

    protected override void CalcSteeringForces()
	{
		Vector3 fleeForce = Vector3.zero;

		//flees from trainer
		if ((fleeForce = Flee ()) != Vector3.zero) {
			
			ultimateForce += fleeForce * fleeWeight;
			
		} 
		//else it goes about its normal business
		else {

			if (forceYes == true)
				ultimateForce += CalculatePath () * seekWeight;
		}

		//Obstacle Avoid
		for(int k = 0; k < gm.Obstacles.Count; k++){
			ultimateForce += AvoidObstacle((GameObject)gm.Obstacles[k], safeDistance) * avoidWeight;
		}

		//keeps the pokemon in bounds
		ultimateForce += StayInBounds () * inBoundWeight;

        //Keep from exceeding max
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        //Applies the actual force
        ApplyForce(ultimateForce);
    }
}
