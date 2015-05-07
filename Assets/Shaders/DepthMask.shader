Shader "DepthMask/MaskerAlphaGradient" {

	Properties {

		_Color ("Color", Color) = (0.0,0.0,0.0,1.0)
		_P1Pos ("P1 Position", Vector) = (0.0, 0.0, 0.0, 0.0)
		_P2Pos ("P2 Position", Vector) = (0.0, 0.0, 0.0, 0.0)

	}	
	SubShader {
		// Render the mask after regular geometry, but before masked geometry and
		// transparent things.
		
		//Tags {"Queue" = "Geometry+10" }
		
		// Don't draw in the RGBA channels; just the depth buffer
		
		//ColorMask 0
		//ZWrite On
		
		// Do nothing specific in the pass:
		//Pass {
		//
		//	Tags {"LightMode" = "ForwardBase"}
		//
		//	//Change alpha value based on attenuation
		//	//user defined
		//	
		//
		//	//unity variables
		//}

		Pass {
			CGPROGRAM
			
			Tags {"LightMode" = "ForwardBase"}

			//pragmas
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _P1Pos;
			uniform float4 _P2Pos;

			//base input structs
			struct vertexInput{
				float4 vertex: POSITION;
				float4 normal: NORMAL;
				float4 color: COLOR;
			};

			struct vertexOutput{
				float4 pos: SV_POSITION;
				float3 normal: NORMAL;
				float4 col: COLOR;
				float4 toP1: To_P1_POS;
				float4 toP2: TO_P2_POS;
			};

			//vertex function
			vertexOutput vert(vertexInput vi){
				vertexOutput vo;
				vo.pos = mul(UNITY_MATRIX_MVP, vi.vertex);

				float3 normalDirection = normalize(mul(float4(vi.normal, 0.0), _World2Object).xyz);

				//normals and color simply passed along
				vo.normal = vi.normal;
				vo.color = vi.color;

				//calculate direction vector to both players
				vo.toP1 = _P1Pos - vi.vertex;
				vo.toP2 = _P2Pos - vi.vertex;

				return vo;
			}

			//fragment function
			float4 frag(vertexOutput vo) : COLOR
			{
				float alpha;
				
				vo.col = (0.0, 0.0, 0.0, alpha);
				return vo.col;
			}
			ENDCG
		}
	}

	//fallback
	Fallback "Diffuse"

}