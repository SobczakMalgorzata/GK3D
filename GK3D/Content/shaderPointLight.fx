//Constants
float4x4 xWorld, xView, xProjection, WorldIT;
float4 xCameraPosition;
//PointLight1
float4 Light1Position, Light2Position;
float4 Light1DiffuseColor, Light2DiffuseColor;
float4 Light1SpecularColor, Light2SpecularColor;
float Light1Range, Light2Range;
float4 AmbientColor, DiffuseColor;
float4 SpecularColor;

texture xTexture;

sampler2D textureSampler = sampler_state {
	Texture = (xTexture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderIn {
	float4 Position : SV_POSITION0;
	float4 Normal : NORMAL0;
	//float4 Color : COLOR0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOut {
	float4 Position : SV_POSITION0;
	float4 vPosition :TEXCOORD0;
	float4 Color : COLOR0;
	float4 vNormal : TEXCOORD1;
	float2 TextureCoordinate : TEXCOORD2;
};

struct PixelShaderOut {
	float4 Color : COLOR0;
};

VertexShaderOut VSmain(VertexShaderIn i) {
	VertexShaderOut o = (VertexShaderOut)0;

	o.vPosition = mul(i.Position, xWorld);
	o.Position = mul(o.vPosition, xView);
	o.Position = mul(o.Position, xProjection);

	o.vNormal = normalize(mul(i.Normal, xWorld));

	o.Color = float4(1,0,1,1);

	o.TextureCoordinate = i.TextureCoordinate;
	
	return o;
}

float4 CalculateColor(float4 light, float4 normal, float4 view, float3 diffuseColor, float3 specularColor, int specularPower)
{
	float3 diffuse = dot(normal, light) * diffuseColor;

	float4 reflection = reflect(-light, normal);
	float3 specular = pow(dot(view, reflection), abs(specularPower)) * specularColor;
	
	return float4(saturate(diffuse) + saturate(specular), 1);
}

PixelShaderOut PSmain(VertexShaderOut i) {
	PixelShaderOut o = (PixelShaderOut)0;
	float4 normal = normalize(i.vNormal);
	float4 l1 = normalize(Light1Position - i.vPosition);
	float4 l2 = normalize(Light2Position - i.vPosition);
	float4 v = normalize(xCameraPosition - i.vPosition);
	float4 r1 = reflect(i.vNormal, l1);
	float4 textureColor = tex2D(textureSampler, i.TextureCoordinate);
	float4 c1 = CalculateColor(l1, normal, v, Light1DiffuseColor, Light1SpecularColor.xyz, (int)(Light1SpecularColor.w));
	float4 c2 = CalculateColor(l2, normal, v, Light2DiffuseColor, Light2SpecularColor.xyz, (int)(Light2SpecularColor.w));


	o.Color = saturate(textureColor * (i.Color)) + AmbientColor + c1+c2;
	return o;
}

technique PointLight
{
	pass Pass0
	{
		VertexShader = compile vs_4_0 VSmain();
		PixelShader = compile ps_4_0 PSmain();
	}
}
