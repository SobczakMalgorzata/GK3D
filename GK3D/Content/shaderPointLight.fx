float4x4 World, View, Projection, WorldIT;
float4 CameraPosition;
float4 Light1Position;
float4 Light1Color;
float4 Light1Range;
float4 AmbientColor, DiffuseColor;
int SpecularColor;
float Alpha;

struct VertexShaderIn {
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float4 Color : COLOR0;

};

struct VertexShaderOut {
	float4 Position : POSITION0;
	float4 vPosition :TEXCOORD0;
	float4 Color : COLOR0;
	float4 vNormal : TEXCOOR1;
};

struct PixelShaderOut {
	float4 Color : COLOR0;
};

VertexShaderOut VSmain(VertexShaderIn i) {
	VertexShaderOut o = (VertexShaderOut)0;
	o.vPosition = mul(i.Position, World);
	o.vNormal = normalize(mul(i.Normal, View));
	o.Position = mul(o.Position, Projection);
	return o;
}

PixelShaderOut PSmain(VertexShaderOut i) {
	PixelShaderOut o;
	float4 l1 = normalize(Light1Position - i.vPosition);
	float4 v = normalize(CameraPosition - i.vPosition);
	float4 r1 = reflect(i.vNormal, l1);
	float4 c1 = dot(i.vNormal, l1) * DiffuseColor + pow(dot(i.vNormal, r1), SpecularColor)*SpecularColor;


	o.Color = AmbientColor + c1;
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
