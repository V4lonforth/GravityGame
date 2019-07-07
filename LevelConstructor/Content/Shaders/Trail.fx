float4x4 worldMatrix;

float width;
float height;

float duration;
float currentTime;
float4 color; 

struct VertexInput
{
	float2 startPosition : POSITION0;
	float2 endPosition : POSITION1;
	
	float endTime : PSIZE0;
};

float4 transformPosition(float2 position)
{
	float4 outPosition = mul(float4(position, 1, 1), worldMatrix);
	outPosition.x = outPosition.x / width - 1.0;
	outPosition.y = -outPosition.y / height + 1.0;
	return outPosition;
}

float4 vsMain(VertexInput input) : POSITION
{
    if (currentTime <= input.endTime - 0.02)
    {
	    float pos = (currentTime - input.endTime + duration) / duration;
		pos = sqrt(pos);
        float2 position = input.startPosition + (input.endPosition - input.startPosition) * pos;

		return transformPosition(position);
    }
	return transformPosition(input.endPosition);
}

float4 psMain(float4 position : POSITION) : COLOR0
{
	return color;
}

technique Trail
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 vsMain();
		PixelShader = compile ps_3_0 psMain();
	}
}