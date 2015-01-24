#define NUMLIGHTS 2

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 DiffuseColor = float3(1, 1, 1);
float3 AmbientLightColor = float3(.2, .2, .2);
float3 LightPosition[NUMLIGHTS];
float3 LightDirection[NUMLIGHTS];
float3 LightColor = float3(1, 1, 1);
float SpecularPower = 32;
float3 SpecularColor = float3(1, 1, 1);
bool TextureEnabled = false;
bool IsOtherTextureEnabled = false;
float3 CameraPosition;
float ConeAngle = 1.2;
float LightFalloff = 20;

float3 PointLightPosition = float3(0, 15, 195);
float PointLightAttenuation = 8;
float PointLightFalloff = 10;
float PointLightSpecularPower = 200;
float3 PointLightSpecularColor = float3(0, 0, 1);
float3 PointLightColor = float3(0, 0, 1);

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
	float3 diffuseColor = DiffuseColor;
	if (TextureEnabled)
	{
		diffuseColor *= tex2D(TextureSampler, input.UV);
		if (IsOtherTextureEnabled)
			diffuseColor += tex2D(OtherTextureSampler, input.UV);
	}
	float3 totalLight = AmbientLightColor;

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

	return float4(diffuseColor * totalLight, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
