#define NUMLIGHTS 2

float4x4 World;
float4x4 View;
float4x4 Projection;
float4 DiffuseColor = float4(1, 1, 1, 1);
float4 AmbientLightColor = float4(.2, .2, .2, 1);
float3 LightPosition[NUMLIGHTS];
float3 LightDirection[NUMLIGHTS];
float3 LightColor = float3(1, 1, 1);
float SpecularPower = 32;
float4 SpecularColor = float4(1, 1, 1, 1);
bool TextureEnabled = false;
bool IsOtherTextureEnabled = false;
float3 CameraPosition;
float ConeAngle = 1.2;
float LightFalloff = 20;

float3 PointLightPosition = float3(0, 15, 195);
float PointLightAttenuation = 8;
float PointLightFalloff = 10;
float PointLightSpecularPower = 200;
float4  PointLightSpecularColor = float4(0, 0, 1, 1);
float4 PointLightColor = float4(0, 0, 1, 1);

// Fog
float FogStart = 5;
float FogEnd = 400;
float FogEnabled = 1;
float FogPower = 0.8;

texture xTexture;
sampler TextureSampler = sampler_state {
	texture = <xTexture>;
	magfilter = LINEAR;
};

texture otherTexture;
sampler OtherTextureSampler = sampler_state {
	texture = <otherTexture>;
	magfilter = LINEAR;
};
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 WorldPosition : TEXCOORD2;
	float3 ViewDirection : TEXCOORD3;
};

float ComputeFogFactor(float d)
{
	return clamp((d - FogStart) / (FogEnd - FogStart), 0, 1) * FogEnabled * FogPower;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4x4 viewProjection = mul(View, Projection);
	output.Position = mul(worldPosition, viewProjection);
	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);
	output.ViewDirection = worldPosition - CameraPosition;
	output.WorldPosition = worldPosition;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 diffuseColor = DiffuseColor;
	if (TextureEnabled)
	{
		diffuseColor *= tex2D(TextureSampler, input.UV);
		if (IsOtherTextureEnabled)
			diffuseColor += tex2D(OtherTextureSampler, input.UV);
	}
	float4 totalLight = float4(0, 0, 0, 1);
	totalLight += AmbientLightColor;

	// Point light
	float3 lightDir = normalize(PointLightPosition - input.WorldPosition);
	float diffuse = dot(normalize(input.Normal), lightDir);
	float d = distance(PointLightPosition, input.WorldPosition);
	float att = 1 - pow(clamp(d / PointLightAttenuation, 0, 1), PointLightFalloff);
	float3 normal = normalize(input.Normal);
	float3 refl = reflect(lightDir, normal);
	float3 view = normalize(input.ViewDirection);
	totalLight += pow(saturate(dot(refl, view)), PointLightSpecularPower) * PointLightSpecularColor * att;
	totalLight += diffuse * att * PointLightColor;

	// Perform lighting calculations per spot light
	for (int i = 0; i < NUMLIGHTS; i++)
	{
		float3 lightDir = normalize(LightPosition[i] - input.WorldPosition);
		float diffuse = saturate(dot(normalize(input.Normal), lightDir));
		float d = dot(-lightDir, normalize(LightDirection[i]));
		float a = cos(ConeAngle);
		float att = 0;

		float3 normal = normalize(input.Normal);
		float3 refl = reflect(lightDir, normal);
		float3 view = normalize(input.ViewDirection);

		if (a < d) {
			att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);
			totalLight += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;
		}

		totalLight += diffuse * att * LightColor[i];
	}

	float4 finalColor = diffuseColor * totalLight;
	float dist = distance(input.WorldPosition + input.ViewDirection, input.WorldPosition);
	float fogFactor = ComputeFogFactor(dist);
	finalColor.rgb = lerp(finalColor.rgb, float4(1, 1, 1, 1), fogFactor);

	return finalColor;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}