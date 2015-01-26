using UnityEngine;
using System.Collections;

public class UIStoreManager : MonoBehaviour {

	public StoreManager jjStore;
	public Canvas canvas;
	// Use this for initialization
	void Awake () {
		if( Director.Instance != null)
		{
			Director.Instance.uiMgr.storeMgr = this;
			this.canvas.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool gameOver = Input.GetButtonDown("Cancel");
		if( gameOver )
		{
			Director.Instance.UnloadScene();
			return;
		}
	}

	public void BeginScreen()
	{
		jjStore.BeginScreen();
	}

	public void Refresh()
	{
		jjStore.Refresh();
	}
}
