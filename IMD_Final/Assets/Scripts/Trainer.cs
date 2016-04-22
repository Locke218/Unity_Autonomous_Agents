//AUTHOR:Zane Draper
//
//PURPOSE: To control the trainer
//
//******************************************

using UnityEngine;
using System.Collections;

public class Trainer: Vehicle{
	
	//VARIABLES************
	public GameObject seekerTarget;
	
	private Vector3 ultimateForce;
	//*********************
	
	// Call Inherited Start and then do our own
	override public void Start () {
		base.Start();
		
		//resets total force
		ultimateForce = Vector3.zero;
		pokeballs = 0;
		
	}

	//sum up forces
	protected override void CalcSteeringForces(){

		//if theyve collected 6 pokemon, return to pokecenter
		if (pokeballs == 6) {
			ultimateForce += SeekPokeCenter() * seekWeight;
		}
		//else search for pokemon
		else{
			Vector3 pokemonForce = Vector3.zero;
			if( (pokemonForce = SeekPokemon()) != Vector3.zero){
				ultimateForce += pokemonForce;
			}
			else
				ultimateForce += Wander() * seekWeight;

		}
		
		//Obstacle Avoid
		for(int k = 0; k < gm.Obstacles.Count; k++){
			ultimateForce += AvoidObstacle((GameObject)gm.Obstacles[k], safeDistance) * avoidWeight;
		}

		//seeks center
		ultimateForce += StayInBounds () * inBoundWeight;
		
		//Keep from exceeding max
		ultimateForce = Vector3.ClampMagnitude (ultimateForce, maxForce);
		
		//Applies the actual force
		ApplyForce (ultimateForce);
	}
}
