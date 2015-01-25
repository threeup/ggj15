using UnityEngine;
using System.Collections;

public class SimpleBob : MonoBehaviour {

	public GameObject move;
	public float reverseHeightTop;
	public float reverseHeightBottom;
	public float speed;

	private bool up;
	private bool delay;
	private float time;

	// Use this for initialization
	void Start () {
		Vector3 temp = move.transform.localPosition;
		temp.y= reverseHeightBottom;
		up=true;
		move.transform.localPosition = temp;
	}
	
	// Update is called once per frame
	void Update () {
		if(up==true)
		{
			if(delay==false)
			{
				Vector3 temp = move.transform.localPosition;
				temp.y+=(speed*Time.deltaTime);
				if(temp.y>=reverseHeightTop)
				{
					up=false;
					move.transform.localPosition=temp;
					delay=true;
				}else{
					move.transform.localPosition=temp;
				}
			}else{
				time+=Time.deltaTime;
				if(time>=0.75f)
				{
					delay=false;
					time=0;
				}
			}
		}else{
			if(delay==false)
			{
				Vector3 temp = move.transform.localPosition;
				temp.y-=(speed*Time.deltaTime);
				if(temp.y<=reverseHeightBottom)
				{
					up=true;
					move.transform.localPosition=temp;
					delay=true;
				}else{
					move.transform.localPosition=temp;
				}
			}else{
				time+=Time.deltaTime;
				if(time>=0.75f)
				{
					delay=false;
					time=0;
				}
			}
		}
	}
}
