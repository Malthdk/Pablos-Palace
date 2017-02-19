Shader "Custom/Metaballs_Paint" {
	Properties {
		_MainTex ("Texture", 2D) = "white" { }
	}
	/// <summary>
	/// Paint metaball shader.
	/// 
	/// Separates each particle by color and overrides it with the one specified.
	/// Notice the texture that passes through this shader only looks at particles, and has a black background.
	/// The core element for the color merging is the floor function, try tweaking it to achive the desired result.
	///
	/// Made by: Malte Holledig
	/// Thanks to: Rodrigo Fernandez Diaz
	/// </summary>
	Subshader {
		Tags {"Queue" = "Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
	
		Pass {
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		
		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};
		
		float4 _MainTex_ST;
		
		v2f vert (appdata_base v) {
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			return o;
		}

		fixed4 frag (v2f i) : COLOR {
			fixed4 texcol = tex2D (_MainTex, i.uv);
			fixed4 finalColor = texcol;
			if(texcol.r>0.37f && texcol.g>0.50f && texcol.b>0.5f){ // This is for WATER!
				finalColor=half4(0.7,0.85,1,0.6);	   
				return finalColor; 
	    	}
	    	else if(texcol.r<0.3 && texcol.g<0.35 && texcol.b<0.5){ // This is for BLACK
				finalColor = floor(finalColor/0.50)*1; 	
				return finalColor;			    
	    	}
	    	else if(texcol.r>0.35f && texcol.g<0.35f && texcol.b<0.35f){ // This is for RED!
				finalColor = half4(0.85,0,0.22,1);
				return finalColor;
	    	}
	    	else if(texcol.r<0.35f && texcol.g<0.35f && texcol.b>0.60f){ // This is for BLUE
				finalColor=half4(0.17,0.17,0.85,1);		
				return finalColor;	    
	    	}
	    	else if(texcol.r>0.30f && texcol.g>0.30f && texcol.b<0.35f){ // This is for YELLOW
				finalColor=half4(1,0.82,0.22,1);		 
				return finalColor;   
	    	}
	     	else if(texcol.r<0.9f && texcol.g>0.30f && texcol.b<0.35f){ // This is for GREEN
				finalColor=half4(0.185,0.75,0.175,1);	 
				return finalColor;
	    	}
	    	else if(texcol.r>0.45f && texcol.g<0.70f && texcol.b>0.4f){ // This is for PURPLE
				finalColor=half4(0.55,0.21,0.75,1);	  
				return finalColor;  
	    	}
	    	else if(texcol.r<0.6f && texcol.g>0.21f && texcol.b>0.64f){ // This is for ORANGE
				finalColor=half4(1,0.42,0.0,1);	    
				return finalColor;
	    	}
	    	return finalColor;
		}
		
		ENDCG
		
			
		}
	}
}	
				