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
// this is all FUCKED!!!! i hate shaders so fucking much
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	bool nextToSolidPixel = false;
    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            float4 color2 = tex2D(uImage0, coords + float2(x, y));
            if (color.w == 0) {
                nextToSolidPixel = true;
            }
        }
    }
    
    if (nextToSolidPixel) {
        color = float4(255, 0, 0, 255);
    }
    return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}