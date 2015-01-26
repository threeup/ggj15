Shader "Custom/Pixelate" {
	Properties {
	 _MainTex ("", 2D) = "white" {}
	 _Scale ("Scale", Range (0.01, 4)) = 1
	}
	 
	SubShader {
	 
	 ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
	 
	 Pass{
	  CGPROGRAM
	  #pragma vertex vert
	  #pragma fragment frag
	  #include "UnityCG.cginc" 
	  //we include "UnityCG.cginc" to use the appdata_img struct
	    
	  struct v2f {
	   float4 pos : POSITION;
	   half2 uv : TEXCOORD0;
	  };
	   
	  //Our Vertex Shader 
	  v2f vert (appdata_img v){
	   v2f o;
	   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	   o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
	   return o; 
	  }
	    
	  sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
	  fixed _Scale;
	    
		//Our Fragment Shader
	  fixed4 frag (v2f i) : COLOR{

		fixed2 pixuv = i.uv;
		fixed scaleX = 256/_Scale;
		fixed scaleY = 240/_Scale;
		pixuv.x = round(pixuv.x*scaleX)/scaleX;
		pixuv.y = round(pixuv.y*scaleY)/scaleY;
		fixed4 col = tex2D(_MainTex, pixuv);


	   return col;
	  }
	  ENDCG
	 }
	} 
	FallBack "Diffuse"
}