using UnityEngine;
using System.Collections;

public class CameraFx : MonoBehaviour {

	public Material mat;

	void OnRenderImage (RenderTexture source, RenderTexture destination){
	 
		if( Application.HasProLicense() )
		{
			//mat is the material containing your shader
			Graphics.Blit(source,destination,mat);
		}
	}

	public void SetPixelState(float remaining)
	{
		int scale = (int)Mathf.Round(remaining / 0.33f);
		mat.SetFloat( "_Scale", scale );
	}
	
}
