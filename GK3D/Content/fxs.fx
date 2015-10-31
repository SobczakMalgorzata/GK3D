float4x4 World, View, Proj, WorldIT;
float4 CameraPosition;
float4 Light1Position;
float4 Light1Color;
float4 Light1Range;
float4 ambient, diffuse;
int specular;

struct VertexShaderIn {
	float4 position : POSITION0;
	float4 normal : NORMAL0;
	float4 color : COLOR0;
};

struct VertexShaderOut {
	float4 position : POSITION0;
	float4 vPosition :TEXCOORD0;
	float4 color : COLOR0;
	float4 vNormal : TEXCOOR1;
};

struct PixelShaderOut {
	float4 color : COLOR0;
};

VertexShaderOut VSmain (VertexShaderIn i) {
	VertexShaderOut o = (VertexShaderOut)0;
	o.vPosition = mul(i.position, World);
	o.vNormal = normalize(mul(i.normal, View));
	o.position = mul(o.position, Proj);
	return o;
}

PixelShaderOut PSmain(VertexShaderOut i) {
	PixelShaderOut o;
	float4 l1 = normalize(Light1Position - i.vPosition);
	float4 v = normalize(CameraPosition- i.vPosition);
	float4 r1 = reflect(i.vNormal, l1);
	float4 c1 = dot(i.vNormal, l1) * diffuse + pow(dot(i.vNormal, r1), specular)*specular;

		
	o.color = ambient + c1;
	return o;
}

technique FirstTechnique
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VSmain();
		PixelShader = compile ps_4_0 PSmain();
	}
}
