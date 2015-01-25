using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

	public GameObject moveMe;
	public float speed;
	public float startX;
	public float endX;

	void Start()
	{
		Vector3 temp = moveMe.transform.localPosition;
		temp.x=startX;
		moveMe.transform.localPosition = temp;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = moveMe.transform.localPosition;
		temp.x-=Time.deltaTime*speed;
		if(temp.x<=endX)
		{
			temp.x=startX;
			moveMe.transform.localPosition=temp;
		}else{
			moveMe.transform.localPosition=temp;
		}
	}
}
