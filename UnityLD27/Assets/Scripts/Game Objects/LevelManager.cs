using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelManager: FContainer
{
	private Level1 _level1;
	private List<Door> _levelDoors = new List<Door>();
	
	public LevelManager(){
		_level1 = new Level1("ship1");
		_level1.SetPosition(0.0f, 0.0f);
		_level1.SetAnchor(0.0f, 0.0f);
		
		//_levelContainer.AddChild(_level1);
		AddChild(_level1);	
				
		addDoors();	
		scrambleDoors();
	}
	
	private void addDoors(){
		// spawning room doors
		Door horzSpawnDoorTop = new Door(false, 6, 15);
		_levelDoors.Add(horzSpawnDoorTop);
		AddChild(horzSpawnDoorTop);
				
		Door horzSpawnDoorBot = new Door(false, 3, 15);		
		_levelDoors.Add(horzSpawnDoorBot);
		AddChild(horzSpawnDoorBot);
		
		// escape pod room doors
		Door horzEscapeDoorTop = new Door(true, 6, 4);		
		_levelDoors.Add(horzEscapeDoorTop);		
		AddChild(horzEscapeDoorTop);
		
		Door horzEscapeDoorBot = new Door(true, 2, 4);		
		_levelDoors.Add(horzEscapeDoorBot);		
		AddChild(horzEscapeDoorBot);
		
		Door vertEscapeDoorTop = new Door(false, 8, 6);		
		_levelDoors.Add(vertEscapeDoorTop);		
		AddChild(vertEscapeDoorTop);
		
		Door vertEscapeDoorBot = new Door(false, 1, 6);		
		_levelDoors.Add(vertEscapeDoorBot);		
		AddChild(vertEscapeDoorBot);
		 
		// middle rooms
		Door horzMidDoorTop = new Door(true, 6, 9);		
		_levelDoors.Add(horzMidDoorTop);		
		AddChild(horzMidDoorTop);
		
		Door horzMidDoorBot = new Door(true, 2, 9);		
		_levelDoors.Add(horzMidDoorBot);		
		AddChild(horzMidDoorBot);
		
		Door vertMidDoorTop = new Door(false, 8, 11);		
		_levelDoors.Add(vertMidDoorTop);		
		AddChild(vertMidDoorTop);
		
		Door vertMidDoorBot = new Door(false, 1, 11);		
		_levelDoors.Add(vertMidDoorBot);		
		AddChild(vertMidDoorBot);
		
		Door vertMidDoorMidLeft = new Door(false, 4, 6);		
		_levelDoors.Add(vertMidDoorMidLeft);		
		AddChild(vertMidDoorMidLeft);
		
		Door vertMidDoorMidRight = new Door(false, 5, 11);		
		_levelDoors.Add(vertMidDoorMidRight);		
		AddChild(vertMidDoorMidRight);
		
		// last rooms between Mid and Spawn
		Door horzDoorTop = new Door(true, 6, 13);		
		_levelDoors.Add(horzDoorTop);		
		AddChild(horzDoorTop);
		
		Door horzDoorBot = new Door(true, 2, 13);		
		_levelDoors.Add(horzDoorBot);		
		AddChild(horzDoorBot);
	}
	
	public Vector2 randomSpawnPosition(){
		return _level1.randomSpawnPosition();
	}	
	
	public bool checkCrewHeadingOk(Crew crewMember, Vector2 newPosition){
		// check if the crew member is passing through a door
		if (crewMemberIsPassingThruDoor(crewMember)){
			// passing through door, so let them keep moving
			return true;
		} else {
			// now check heading
			return _level1.checkCrewHeadingOk(crewMember, newPosition);	
		}
	}
	
	public bool crewMemberIsPassingThruDoor(Crew crewMember){
		// check if the crew member is intersecting any doors, this would only be possible if we allowed them to start passing through one
		float crewXMin = crewMember.x - (crewMember.width / 2);
		float crewXMax = crewMember.x + (crewMember.width / 2);
		float crewYMin = crewMember.y - (crewMember.height / 2);
		float crewYMax = crewMember.y + (crewMember.height / 2);
		
		for (int i = 0; i < _levelDoors.Count; i++){
		 	Door door = _levelDoors[i];
			if (door.doorIsHorizontal){
				// horizontal door, so check if crew's y boundaries intersect door's y
				if (crewYMin <= door.y && crewYMax >= door.y){
					return true;
				}	
			} else {
				// vertical door, so check if crew's x boundaries intersect door's x
				if (crewXMin <= door.x && crewXMax >= door.x){
					return true;	
				}
			}			
		}
		
		return false;
	}
	
	public bool willDirectCrewToDoor(Crew crewMember){
		bool willDirectToDoor = false;
		
		// check for a door immediately next to crew member
		// we'll do a tile based check if the crew member is completely in a tile
		
		// get crew member's bounding values
		float crewXMin = crewMember.x - (crewMember.width / 2);
		float crewXMax = crewMember.x + (crewMember.width / 2);
		float crewYMin = crewMember.y - (crewMember.height / 2);
		float crewYMax = crewMember.y + (crewMember.height / 2);
		
		// get tile's x bounding values
		int colNumber = (int)crewMember.x / 64;
		int xTileMin = colNumber * 64;
		int xTileMax = xTileMin + 64;
		
		// get tile's y bounding values
		int rowNumber = (int)crewMember.y / 64;
		int yTileMin = rowNumber * 64;
		int yTileMax = yTileMin + 64;
		
		// see if there is a door next to the crew member
		Door door = getDoor(rowNumber, colNumber);
			
		if (door != null){
			if (door.doorIsHorizontal){
				// door is above or below the crew member, make sure they are within the tile's x boundary in order to access	
				if (crewXMin >= xTileMin && crewXMax <= xTileMax){
					if (rowNumber == door.startRow){
						// crew member is below	
						if (crewMember.direction != VectorDirection.Up){
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Up);
						}
					} else {
						// crew member is above
						if (crewMember.direction != VectorDirection.Down){
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Down);
						}
					}
				}
			} else {
				// door is to the left or right of the crew member, make sure they are within the tile's y boundary in order to access
				if (crewYMin >= yTileMin && crewYMax <= yTileMax){
					// check for a door to the left or right that the crew member could access
					if (colNumber == door.startCol){
						// crew member is to the left	
						if (crewMember.direction != VectorDirection.Right){
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Right);
						}
					} else {
						// crew member is to the right
						if (crewMember.direction != VectorDirection.Left){
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Left);
						}
					}
				}
			}
		}	
		
		
		return willDirectToDoor;
	}
	
	private Door getDoor(int crewRowNumber, int crewColNumber){
		Door returnDoor = null;
		
		for (int i = 0; i < _levelDoors.Count; i++){
		 	Door door = _levelDoors[i];
			if (door.startRow == crewRowNumber && door.startCol == crewColNumber){ 
				// crew member is standing on a door start tile
				returnDoor = door;	
				break;
			} else if (door.doorIsHorizontal){
				// horizontal door, so check ending tile (in same col)
				if (door.startRow + 1 == crewRowNumber && door.startCol == crewColNumber){
					// crew member is standing on a door end tile
					returnDoor = door;	
					break;
				}
			} else {
				// vertical door, so check ending tile (in same row)	
				if (door.startRow == crewRowNumber && door.startCol + 1 == crewColNumber){
					// crew member is standing on a door end tile
					returnDoor = door;	
					break;
				}
			}			
		}
		
		return returnDoor;
	}
	
	public void scrambleDoors(){
		// open half the doors, close the other half
		int countToOpen = _levelDoors.Count / 2;
		
		// close all doors to start
		for (int i = 0; i<_levelDoors.Count; i++){
			//_levelDoors[i].doorIsOpen = false;	
			_levelDoors[i].SetDoorStatus(false);
		}
		
		int[] doorsToOpen = new int[countToOpen];
		
		// pick random doors to open
		for (int i = 0; i < countToOpen; i++){
			int doorNumber = RXRandom.Range(0, _levelDoors.Count);
			bool picked = true;
			while (picked){
				picked = false;
				for (int j = 0; j < i; j++){
					if (doorsToOpen[j] == doorNumber){
						picked = true;
						doorNumber = RXRandom.Range(0, _levelDoors.Count);
						break;
					}
				}
			}
			doorsToOpen[i] = doorNumber;
			// open the door
			//_levelDoors[doorNumber].doorIsOpen = true;
			_levelDoors[doorNumber].SetDoorStatus(true);
		}
		
	}
}

