using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    // VARS

    bool isWallR;
    bool isWallL;
    RaycastHit hitR;
    RaycastHit hitL;
	bool canAttach = true;
	bool hasAttached = false;
	public float wallRunAngle;

	
	UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController cc;
    Rigidbody rb;
	public float fallOffForce;
	[Tooltip("Setting to 1 will disable the boost from jumping off")]
	public float jumpOffMultiplier;
	public float jumpOffBoost;

	void Awake( ){
        cc = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
		rb = GetComponent<Rigidbody>();
    }

	void Start( ) {
		
	}
	
	void FixedUpdate( ) {

		if( cc.Grounded ) {

			hasAttached = false;
			canAttach = true;

		} else {
			if( Input.GetKeyDown( KeyCode.Space ) ) {
				AfterRun( fallOffForce * jumpOffMultiplier );
			} else if ( Input.GetKeyUp( KeyCode.Space ) ) {
				canAttach = true;
				hasAttached = false;
			} else {
				if( TryAttach() ) {

				} else {
					if(hasAttached) {
						AfterRun();
					}
				}
			}
		}
	}

	private bool TryAttach( ) {
		if( canAttach && Input.GetKey( KeyCode.D ) && Physics.Raycast( transform.position,  transform.right, out hitR, 1 ) ) { //user wants to attach on right side (ie. start wallrunning)
			if( hitR.transform.tag == "Wall" ) { //check if the rayed thing is supposed to be wallrunable
				isWallR = true;
				isWallL = false;
				rb.useGravity = false; //make player fly off into space
				StablilizeUpwardSpeed(); //make player not fly off into space
				hasAttached = true;
				return true;
			}
			return false;
		} else 
		if( canAttach && Input.GetKey( KeyCode.A ) && Physics.Raycast( transform.position, -transform.right, out hitL, 1 ) ) { //user wants to attach on left side  (ie. start wallrunning)
			if( hitL.transform.tag == "Wall" ) { //check if the rayed thing is supposed to be wallrunable
				isWallL = true;
				isWallR = false;
				rb.useGravity = false; //make player fly off into space
				StablilizeUpwardSpeed(); //make player not fly off into space
				hasAttached = true;
				return true;
			}
			return false;
		} else {
			return false;
		}
	}

	private void StablilizeUpwardSpeed( ) {
		rb.velocity = new Vector3( rb.velocity.x, rb.velocity.y * ( 1.1f * Time.fixedDeltaTime ), rb.velocity.z ); //make y velocity divide by 1.1 every second
	}

	void AfterRun( float force){

		canAttach = false;

		if( isWallL ) {
			rb.AddForce( (hitL.normal + ( Input.GetKey( KeyCode.W ) ? this.transform.forward : Vector3.zero ) ).normalized * force, ForceMode.VelocityChange );
		}

		if( isWallR ) {
			rb.AddForce( ( hitR.normal + ( Input.GetKey( KeyCode.W ) ? this.transform.forward : Vector3.zero ) ).normalized * force, ForceMode.VelocityChange );
		}

		if (force != fallOffForce ) {
			rb.AddForce( this.transform.up * jumpOffBoost, ForceMode.VelocityChange );
		}

		isWallL = false;
		isWallR = false;
		rb.useGravity = true;
	}

	void AfterRun( ) {
		AfterRun( fallOffForce );
	}


}
