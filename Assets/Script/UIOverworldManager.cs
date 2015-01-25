using UnityEngine;
using System.Collections;

public class UIOverworldManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		this.gameObject.SetActive(false);
		Director.Instance.uiMgr.overworldMgr = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
