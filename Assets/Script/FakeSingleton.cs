using UnityEngine;
using System.Collections;

public class FakeSingleton : MonoBehaviour {

	public void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
