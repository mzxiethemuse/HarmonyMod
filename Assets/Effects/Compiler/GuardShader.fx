sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uShaderSpecificData;
float4 uSourceRect;
float2 uZoom;



// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

float4 main(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float multA = color.w * abs(pow(sin((coords.y + uTime / 2) * 8), 88));
	float timeMult = 1;
	if(uOpacity < 20)
	{
	    timeMult = abs(sin(uTime + uOpacity * 0.33));
	}
    float4 yellow = float4(200, 200, 0, 255) * max(multA * 0.25, 0.5 * (color.w / 255)) * 0.3 * timeMult;
	return color + yellow;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 main();
	}
}