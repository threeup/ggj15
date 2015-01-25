using UnityEngine;
using System.Collections;

public class LevelSelectPath : MonoBehaviour {

	public GameObject[] wayPoints;
	public int[] levelNumbers;
	public float moveSpeed;
	public GameObject character;

	public int currentWaypoint;
	public int targetWaypoint;
	public bool go;

	// Update is called once per frame
	void Update () {
		if(go==true)
		{
			if(targetWaypoint>currentWaypoint)
			{
				if(character.transform.localPosition.x<wayPoints[currentWaypoint+1].transform.localPosition.x)
				{
					Vector3 temp = character.transform.localPosition;
					temp.x+=(moveSpeed*Time.deltaTime);
					if(temp.x>=wayPoints[currentWaypoint+1].transform.localPosition.x)
					{
						temp.x=wayPoints[currentWaypoint+1].transform.localPosition.x;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.y<wayPoints[currentWaypoint+1].transform.localPosition.y)
				{
					Vector3 temp = character.transform.localPosition;
					temp.y+=(moveSpeed*Time.deltaTime);
					if(temp.y>=wayPoints[currentWaypoint+1].transform.localPosition.y)
					{
						temp.y=wayPoints[currentWaypoint+1].transform.localPosition.y;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.x>wayPoints[currentWaypoint+1].transform.localPosition.x)
				{
					Vector3 temp = character.transform.localPosition;
					temp.x-=(moveSpeed*Time.deltaTime);
					if(temp.x<=wayPoints[currentWaypoint+1].transform.localPosition.x)
					{
						temp.x=wayPoints[currentWaypoint+1].transform.localPosition.x;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.y>wayPoints[currentWaypoint+1].transform.localPosition.y)
				{
					Vector3 temp = character.transform.localPosition;
					temp.y-=(moveSpeed*Time.deltaTime);
					if(temp.y<=wayPoints[currentWaypoint+1].transform.localPosition.y)
					{
						temp.y=wayPoints[currentWaypoint+1].transform.localPosition.y;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.x == wayPoints[currentWaypoint+1].transform.localPosition.x && character.transform.localPosition.y == wayPoints[currentWaypoint+1].transform.localPosition.y )
				{
					//Reached the next waypoint.
					currentWaypoint+=1;
				}
			}else if(targetWaypoint<currentWaypoint){
				if(character.transform.localPosition.x<wayPoints[currentWaypoint-1].transform.localPosition.x)
				{
					Vector3 temp = character.transform.localPosition;
					temp.x+=(moveSpeed*Time.deltaTime);
					if(temp.x>=wayPoints[currentWaypoint-1].transform.localPosition.x)
					{
						temp.x=wayPoints[currentWaypoint-1].transform.localPosition.x;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.y<wayPoints[currentWaypoint-1].transform.localPosition.y)
				{
					Vector3 temp = character.transform.localPosition;
					temp.y+=(moveSpeed*Time.deltaTime);
					if(temp.y>=wayPoints[currentWaypoint-1].transform.localPosition.y)
					{
						temp.y=wayPoints[currentWaypoint-1].transform.localPosition.y;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.x>wayPoints[currentWaypoint-1].transform.localPosition.x)
				{
					Vector3 temp = character.transform.localPosition;
					temp.x-=(moveSpeed*Time.deltaTime);
					if(temp.x<=wayPoints[currentWaypoint-1].transform.localPosition.x)
					{
						temp.x=wayPoints[currentWaypoint-1].transform.localPosition.x;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.y>wayPoints[currentWaypoint-1].transform.localPosition.y)
				{
					Vector3 temp = character.transform.localPosition;
					temp.y-=(moveSpeed*Time.deltaTime);
					if(temp.y<=wayPoints[currentWaypoint-1].transform.localPosition.y)
					{
						temp.y=wayPoints[currentWaypoint-1].transform.localPosition.y;
						character.transform.localPosition=temp;
					}else{
						character.transform.localPosition=temp;
					}
				}
				if(character.transform.localPosition.x == wayPoints[currentWaypoint-1].transform.localPosition.x && character.transform.localPosition.y == wayPoints[currentWaypoint-1].transform.localPosition.y )
				{
					//Reached the next waypoint.
					currentWaypoint-=1;
				}
			}else{
				go=false;
				currentWaypoint = targetWaypoint;
			}
		}
	}

	public void GoTargetWaypoint(int wayPointNum){
		targetWaypoint= wayPointNum;
		if(targetWaypoint!=currentWaypoint)
		{
			go=true;
		}
	}

	public void StartLevel()
	{
		if(go==false)
		{
			Application.LoadLevel(levelNumbers[currentWaypoint]);
		}
	}
}
