using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelManager: FContainer
{
	private Level1 _level1;
	private List<Door> _levelDoors = new List<Door>();
	public List<MoveTile> moveTiles = new List<MoveTile>();
	
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
		Door horzSpawnDoorTop = new Door(false, 5, 15);
		_levelDoors.Add(horzSpawnDoorTop);
		AddChild(horzSpawnDoorTop);
				
		Door horzSpawnDoorBot = new Door(false, 4, 15);		
		_levelDoors.Add(horzSpawnDoorBot);
		AddChild(horzSpawnDoorBot);
		
		// escape pod room doors
		Door horzEscapeDoorTop = new Door(true, 6, 4);		
		_levelDoors.Add(horzEscapeDoorTop);		
		AddChild(horzEscapeDoorTop);
		
		Door horzEscapeDoorBot = new Door(true, 2, 5);		
		_levelDoors.Add(horzEscapeDoorBot);		
		AddChild(horzEscapeDoorBot);
		
		Door vertEscapeDoorTop = new Door(false, 8, 6);		
		_levelDoors.Add(vertEscapeDoorTop);		
		AddChild(vertEscapeDoorTop);
		
//		Door vertEscapeDoorBot = new Door(false, 1, 6);		
//		_levelDoors.Add(vertEscapeDoorBot);		
//		AddChild(vertEscapeDoorBot);
		 
		// middle rooms
		Door horzMidDoorTop = new Door(true, 6, 9);		
		_levelDoors.Add(horzMidDoorTop);		
		AddChild(horzMidDoorTop);
		
		Door horzMidDoorBot = new Door(true, 2, 8);		
		_levelDoors.Add(horzMidDoorBot);		
		AddChild(horzMidDoorBot);
		
		Door vertMidDoorTop = new Door(false, 7, 11);		
		_levelDoors.Add(vertMidDoorTop);		
		AddChild(vertMidDoorTop);
		
		Door vertMidDoorBot = new Door(false, 1, 11);		
		_levelDoors.Add(vertMidDoorBot);		
		AddChild(vertMidDoorBot);
		
		Door vertMidDoorMidLeft = new Door(false, 4, 6);		
		_levelDoors.Add(vertMidDoorMidLeft);		
		AddChild(vertMidDoorMidLeft);
		
//		Door vertMidDoorMidRight = new Door(false, 5, 11);		
//		_levelDoors.Add(vertMidDoorMidRight);		
//		AddChild(vertMidDoorMidRight);
		
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
	
	// this method is fairly similar to willDirectCrewToDoor(Crew crewMember)
	// there may be a way to combine similar functionality that has been copy/pasted.
	// but thats a challenge for another time.
	public bool crewMemberIsPassingThruDoor(Crew crewMember){
		// check if the crew member is on its way through a daor
		// we'll do this by checking if the crew member is on a door's start/end tile and headed towards its end/start tile
		// of if the crew member is actually intersecting the door
		
		// check for intersection status first, since thats easier
		float crewXMin = crewMember.x - (crewMember.width / 2);
		float crewXMax = crewMember.x + (crewMember.width / 2);
		float crewYMin = crewMember.y - (crewMember.height / 2);
		float crewYMax = crewMember.y + (crewMember.height / 2);
		
		for (int i = 0; i < _levelDoors.Count; i++){
		 	Door door = _levelDoors[i];
			if (door.doorIsHorizontal){
				// horizontal door, so check if crew's y boundaries intersect door's y
				if (crewYMin <= door.y && crewYMax >= door.y){
					// make sure crew member's x boundaries are within the door's x boundaries
					if (crewXMin >= door.x - door.width / 2 && crewXMax <= door.x + door.width / 2){
						return true;
					}
				}	
			} else {
				// vertical door, so check if crew's x boundaries intersect door's x
				if (crewXMin <= door.x && crewXMax >= door.x){
					// make sure crew member's y boundaries are within the door's y boundaries
					if (crewYMin >= door.y - door.height / 2 && crewYMax <= door.y + door.height / 2){
						return true;
					}	
				}
			}			
		}
		
		// if we get here, we didn't find any interesecting doors. So now we check if 		
		// the crew member is on its way towards a door.
		
		// get crew members column number and the tile's x bounding values
		int colNumber = (int)crewMember.x / 64;
		int xTileMin = colNumber * 64;
		int xTileMax = xTileMin + 64;
		
		// get crew members row number tile's y bounding values
		int rowNumber = (int)crewMember.y / 64;
		int yTileMin = rowNumber * 64;
		int yTileMax = yTileMin + 64;
		
		// see if there is a door next to the crew member
		Door checkDoor = getOpenDoor(rowNumber, colNumber);
		
		if (checkDoor != null){
			if (checkDoor.doorIsHorizontal){
				// door is above or below the crew member, make sure they are within the tile's x boundary in order to access	
				if (crewXMin >= xTileMin && crewXMax <= xTileMax){
					if (rowNumber == checkDoor.startRow){
						// crew member is below	
						if (crewMember.direction == VectorDirection.Up){
							// crew member is heading through the door
							return true;
						}
					} else {
						// crew member is above
						if (crewMember.direction == VectorDirection.Down){
							// crew member is heading through the door
							return true;
						}
					}
				}
			} else {
				// door is to the left or right of the crew member, make sure they are within the tile's y boundary in order to access
				if (crewYMin >= yTileMin && crewYMax <= yTileMax){
					// check for a door to the left or right that the crew member could access
					if (colNumber == checkDoor.startCol){
						// crew member is to the left	
						if (crewMember.direction == VectorDirection.Right){
							// crew member is heading through the door
							return true;
						}
					} else {
						// crew member is to the right
						if (crewMember.direction == VectorDirection.Left){
							// crew member is heading through the door
							return true;
						}
					}
				}
			}
		}	
		
		// if get here, crew member is not heading through a door
		return false;
	}
	
	// this method is fairly similar to rewMemberIsPassingThruDoor(Crew crewMember)
	// there may be a way to combine similar functionality that has been copy/pasted.
	// but thats a challenge for another time.
	public bool willDirectCrewToDoor(Crew crewMember){
		bool willDirectToDoor = false;
		
		// check for a door immediately next to crew member
		
		// we'll do a tile based check if the crew member is completely in a tile
		
		// get crew member's bounding values
		float crewXMin = crewMember.x - (crewMember.width / 2);
		float crewXMax = crewMember.x + (crewMember.width / 2);
		float crewYMin = crewMember.y - (crewMember.height / 2);
		float crewYMax = crewMember.y + (crewMember.height / 2);
		
		// get crew members column number tile's x bounding values
		int colNumber = (int)crewMember.x / 64;
		int xTileMin = colNumber * 64;
		int xTileMax = xTileMin + 64;
		
		// get crew members row number tile's y bounding values
		int rowNumber = (int)crewMember.y / 64;
		int yTileMin = rowNumber * 64;
		int yTileMax = yTileMin + 64;
		
		// see if there is a door next to the crew member
		Door checkDoor = getOpenDoor(rowNumber, colNumber);
			
		if (checkDoor != null && crewMember.lastMoveTileUsed == null){ // if lastMoveTileUsed is not null, then crew member is still obeying a move tile
			if (checkDoor.doorIsHorizontal){
				// door is above or below the crew member, make sure they are within the tile's x boundary in order to access	
				if (crewXMin >= xTileMin && crewXMax <= xTileMax){
					if (rowNumber == checkDoor.startRow){
						// crew member is below	
						if (crewMember.direction != VectorDirection.Up && crewMember.direction != VectorDirection.Down){
							// update direction so long as crew member is not moving directly away from the door, in which case they probably just passed through it
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Up);
						}
					} else {
						// crew member is above
						if (crewMember.direction != VectorDirection.Down && crewMember.direction != VectorDirection.Up){
							// update direction so long as crew member is not moving directly away from the door, in which case they probably just passed through it
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Down);
						}
					}
				}
			} else {
				// door is to the left or right of the crew member, make sure they are within the tile's y boundary in order to access
				if (crewYMin >= yTileMin && crewYMax <= yTileMax){
					// check for a door to the left or right that the crew member could access
					if (colNumber == checkDoor.startCol){
						// crew member is to the left	
						if (crewMember.direction != VectorDirection.Right && crewMember.direction != VectorDirection.Left){
							// update direction so long as crew member is not moving directly away from the door, in which case they probably just passed through it
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Right);
						}
					} else {
						// crew member is to the right
						if (crewMember.direction != VectorDirection.Left && crewMember.direction != VectorDirection.Right){
							// update direction so long as crew member is not moving directly away from the door, in which case they probably just passed through it
							willDirectToDoor = true;
							crewMember.ChangeDirection(VectorDirection.Left);
						}
					}
				}
			}
		}	
		
		
		return willDirectToDoor;
	}
	
	private Door getOpenDoor(int crewRowNumber, int crewColNumber){
		Door returnDoor = null;
		
		for (int i = 0; i < _levelDoors.Count; i++){
		 	Door door = _levelDoors[i];
			if (door.doorIsOpen){
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
	
	public void handleUserTouch(FTouch touch){
		//Debug.Log ("User Global Touch x:" + touch.position.x + " y:" + touch.position.y);
		Vector2 touchPos = _level1.GlobalToLocal(touch.position);
		//Debug.Log ("User Local Touch x:" + touchPos.x + " y:" + touchPos.y);
		
		int colNumber = (int)touchPos.x / 64;
		int rowNumber = (int)touchPos.y / 64;
		
		Door doorTouched = null;
		
		for (int i = 0; i < _levelDoors.Count; i++){
		 	Door door = _levelDoors[i];
			if (door.startRow == rowNumber && door.startCol == colNumber){
				doorTouched = door;	
				break;
			} else if (door.doorIsHorizontal){
				// horizontal door, so check ending tile (in same col)
				if (door.startRow + 1 == rowNumber && door.startCol == colNumber){
					doorTouched = door;	
					break;
				}
			} else {
				// vertical door, so check ending tile (in same row)	
				if (door.startRow == rowNumber && door.startCol + 1 == colNumber){
					doorTouched = door;	
					break;
				}
			}
		}
		
		if (doorTouched != null){
			doorTouched.SetDoorStatus(!doorTouched.doorIsOpen);	
		}
	}
	
	#region MoveTiles
	
	public void moveTileAdded(MoveTile newMoveTile){
		// first check if this tile exists
		for (int i = moveTiles.Count-1; i >= 0; i--) 
		{
			MoveTile testMoveTile = moveTiles[i];
		
			if (testMoveTile.x == newMoveTile.x && testMoveTile.y == newMoveTile.y){
				moveTiles.Remove(testMoveTile);
				testMoveTile.shouldDestroy = true;
				break; // break out since we can only have 1 moveTile on a location
			}
		}
		
		moveTiles.Add(newMoveTile);
	}
	
	private MoveTile getMoveTile(int row, int col){
		MoveTile foundTile = null;
		
		for (int i = moveTiles.Count-1; i >= 0; i--) 
		{
			MoveTile testMoveTile = moveTiles[i];
		
			if (testMoveTile.shouldDestroy){
				// ignore this move tile, its being destroyed. We can remove it form our list.
				moveTiles.Remove (testMoveTile);
			} else {
				int moveTileRow = (int)testMoveTile.y / 64;
				int moveTileCol = (int)testMoveTile.x / 64;
				
				if (moveTileRow == row && moveTileCol == col){
					foundTile = testMoveTile;
					break;
				}
			}
		}
		
		return foundTile;
	}
	
	public bool willObeyFloorTile(Crew crewMember, Vector2 newPosition){
		bool willObey = false;
		// check for a floor tile under crew member
		
		// we'll do a tile based check if the crew member is completely in a tile
		
		// get crew member's bounding values
		float crewXMin = newPosition.x - (crewMember.width / 2);
		float crewXMax = newPosition.x + (crewMember.width / 2);
		float crewYMin = newPosition.y - (crewMember.height / 2);
		float crewYMax = newPosition.y + (crewMember.height / 2);
		
		// get crew members column number tile's x bounding values
		int colNumber = (int)newPosition.x / 64;
		int xTileMin = colNumber * 64;
		int xTileMax = xTileMin + 64;
		
		// get crew members row number tile's y bounding values
		int rowNumber = (int)newPosition.y / 64;
		int yTileMin = rowNumber * 64;
		int yTileMax = yTileMin + 64;
		
		MoveTile moveTile = getMoveTile(rowNumber, colNumber);
		
		if (moveTile != null && moveTile != crewMember.lastMoveTileUsed){
			// crew member is on a move tile and is not currently following it			
			if (crewMember.direction != moveTile.direction){
				// we only care if the tile will change the crew members direction
				
				if (crewMember.direction == VectorDirection.Up || crewMember.direction == VectorDirection.Down){
				 	// crew member is moving up or down, wait until crew member is contained between tile's min/max y before obeying it 	
					if (crewYMin >= yTileMin && crewYMax <= yTileMax){
						crewMember.ChangeDirection(moveTile.direction);	
						crewMember.lastMoveTileUsed = moveTile;
						willObey = true;
					}
				} else {
					// crew member is moving left or right, wait until crew member is contained between tile's min/max x before obeying it 	
					if (crewXMin >= xTileMin && crewXMax <= xTileMax){
						crewMember.ChangeDirection(moveTile.direction);	
						crewMember.lastMoveTileUsed = moveTile;
						willObey = true;
					}
				}
			}
		} else if (moveTile == null){
			// crew member is not on a move tile
			crewMember.lastMoveTileUsed = null;
		}
		
		return willObey;	
	}
	
	#endregion
}

