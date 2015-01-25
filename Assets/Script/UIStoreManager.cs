using UnityEngine;
using System.Collections;

public class UIStoreManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		this.gameObject.SetActive(false);
		Director.Instance.uiMgr.storeMgr = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
