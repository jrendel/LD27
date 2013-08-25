using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Level1 : FSprite
{	
	
	public Level1(string name) : base(name)
	{
		
	}
	
	public Vector2 randomSpawnPosition(){
		// random col between 16 and 19
		// random row between 2 and 8
		
		int horzMin = (16 * 64) + 32;
		int horzMax = (19 * 64) - 32;
		int vertMin = (2 * 64) + 32;
		int vertMax = (8 * 64) - 32;
		float x = RXRandom.Range(horzMin, horzMax);
		float y = RXRandom.Range(vertMin, vertMax);
		
		// lets make sure the crew don't spawn on the wall between the spawning zones
		if (y >= (4 * 64) + 32 && y <= (5 * 64)){
			y = 4 * 64;	
		}
		
		if (y >= (5 * 64) && y <= (5 * 64) + 32){
			y = (5 * 64) + 34;	
		}
		
		return new Vector2(x, y);
	}
	
	// check if the crew member is going to run into a wall
	public bool checkCrewHeadingOk(Crew crewMember, Vector2 newPosition){
		bool headingOk = true;
		
		if (crewMember.direction == VectorDirection.Up){
			// check top walls and horizontal walls	
			int maxY; // max height the crew member can travel in this spot
			if (crewMember.x >= 16*64){
				maxY = (8 * 64) - 32;				
			} else {
				maxY = 9 * 64;
			}	
			if (newPosition.y >= maxY || isCrewGoingToIntersectWall(crewMember, newPosition)){
				headingOk = false;
				
				if (RXRandom.Range(0,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Right);
				} else {
					crewMember.ChangeDirection(VectorDirection.Left);	
				}
			}
		} else if (crewMember.direction == VectorDirection.Down){
			// check bottom walls and horizontal walls	
			int minY; // min y the crew member can travel in this spot
			if (crewMember.x >= 16*64){
				minY = (2 * 64) + 30;				
			} else {
				minY = 64 ;
			}	
			if (newPosition.y <= minY || isCrewGoingToIntersectWall(crewMember, newPosition)){
				headingOk = false;	
				
				if (RXRandom.Range(0,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Right);
				} else {
					crewMember.ChangeDirection(VectorDirection.Left);	
				}
			}
		} else if (crewMember.direction == VectorDirection.Left){
			// check bottom walls and horizontal walls	
			int minX; // min x the crew member can travel in this spot
			minX = (3 * 64) + 32;				
				
			if (newPosition.x <= minX || isCrewGoingToIntersectWall(crewMember, newPosition)){
				headingOk = false;	
				
				if (RXRandom.Range(0,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Up);
				} else {
					crewMember.ChangeDirection(VectorDirection.Down);	
				}
			} 
		} else if (crewMember.direction == VectorDirection.Right){
			// check bottom walls and horizontal walls	
			int maxX; // max x the crew member can travel in this spot
			if (crewMember.y >= (2*64) + 32 && crewMember.y <= (8*64) - 32){
				maxX = (19 * 64) - 32;				
			} else {
				maxX = 16 * 64 - 32;
			}	
			if (newPosition.x >= maxX  || isCrewGoingToIntersectWall(crewMember, newPosition)){
				headingOk = false;	
				
				if (RXRandom.Range(0,2) == 1){
					crewMember.ChangeDirection(VectorDirection.Up);
				} else {
					crewMember.ChangeDirection(VectorDirection.Down);	
				}
			}
		}
		
		return headingOk;
	}
	
	private bool isCrewGoingToIntersectWall(Crew crewMember,  Vector2 newPosition){
		bool willIntersectWall = false;
		
		float crewXMin = newPosition.x - (crewMember.width / 2);
		float crewXMax = newPosition.x + (crewMember.width / 2);
		float crewYMin = newPosition.y - (crewMember.height / 2);
		float crewYMax = newPosition.y + (crewMember.height / 2);
		
		if (crewXMax > 16 * 64){ // check spawning location walls
			// vertical
			int wallX = 16 * 64;
			
			if (crewXMin < wallX && crewXMax > wallX){
				willIntersectWall = true;
			}
			
			// horizontal
			int wallY = 5 * 64;
			
			if (crewYMin < wallY && crewYMax > wallY){
				willIntersectWall = true;
			}
		} else { // check all other walls
			// check verticals 	
			int[] vertWallPositions = new int[3];
			vertWallPositions[0] = 7 * 64;
			vertWallPositions[1] = 12 * 64;
			vertWallPositions[2] = 16 * 64;
			
			for (int i = 0; i < 3; i++){
				if (crewXMin < vertWallPositions[i] && crewXMax > vertWallPositions[i]){
					willIntersectWall = true;
					break;
				}	
			}
			
			// check horizontals
			int[] horzWallPositions = new int[2];
			horzWallPositions[0] = 3 * 64;
			horzWallPositions[1] = 7 * 64;
			
			for (int i = 0; i < 2; i++){
				if (crewYMin < horzWallPositions[i] && crewYMax > horzWallPositions[i]){
					willIntersectWall = true;
					break;
				}
			}
		}
		
		return willIntersectWall;
	}
	
}

