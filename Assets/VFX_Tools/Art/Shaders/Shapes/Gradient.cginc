



//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

float4 InverseLerp_float4(float4 A, float4 B, float4 T)
{
    return (T - A) / (B - A);
}

void SampleGradient_float(float4 PrimaryColor, float4 SecondaryColor, float4 BlendColor, float LocationA, float LocationB, float T, out float4 Color)
{
    float blendCenter = (LocationA + LocationB) / 2.0f;
	
	if(T < LocationA)
	{
		Color = PrimaryColor;
	}
	else if( T > LocationB)
	{
		Color = SecondaryColor;
	}
	else
	{
        Color = BlendColor;
		if(T < blendCenter)
        {
            Color = lerp(PrimaryColor, BlendColor, InverseLerp_float4(LocationA, blendCenter, T));
        }
		else
        {
            Color = lerp(BlendColor, SecondaryColor, InverseLerp_float4(blendCenter, LocationB, T));
        }
    }
}

void SampleGradient_half(float4 PrimaryColor, float4 SecondaryColor, float4 BlendColor, float LocationA, float LocationB, float T, out float4 Color)
{
    float blendCenter = (LocationA + LocationB) / 2.0f;
	
    if (T < LocationA)
    {
        Color = PrimaryColor;
    }
    else if (T > LocationB)
    {
        Color = SecondaryColor;
    }
    else
    {
        Color = BlendColor;
        if (T < blendCenter)
        {
            Color = lerp(PrimaryColor, BlendColor, InverseLerp_float4(LocationA, blendCenter, T));
        }
        else
        {
            Color = lerp(BlendColor, SecondaryColor, InverseLerp_float4(blendCenter, LocationB, T));
        }
    }
}

#endif //MYHLSLINCLUDE_INCLUDED