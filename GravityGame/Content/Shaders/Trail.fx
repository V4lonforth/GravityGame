float4x4 worldMatrix;

float width;
float height;

float duration;
float currentTime;
float4 color; 

static const float PI = 3.14159265f;

struct VertexInput
{
	float2 startPosition : POSITION0;
	float2 endPosition : POSITION1;
	
	float endTime : PSIZE0;
};

struct VertexToPixel
{
	float4 position : POSITION;
};

struct PixelToFrame
{
	float4 color : COLOR0;
};

VertexToPixel vsMain(VertexInput input)
{
	VertexToPixel output = (VertexToPixel)0;
    if (currentTime <= input.endTime - 0.02)
    {
	    float pos = (currentTime - input.endTime + duration) / duration;
		//pos = sin(pos * PI / 2);
		pos = sqrt(pos);
        float2 position = input.startPosition + (input.endPosition - input.startPosition) * pos;

	    output.position = mul(float4(position.x, position.y, 1, 1), worldMatrix);
	    output.position.x = output.position.x / width - 1.0;
	    output.position.y = -output.position.y / height + 1.0;
    }
	return output;
}

PixelToFrame psMain(VertexToPixel input)
{
	PixelToFrame output = (PixelToFrame)0;

	output.color = color;

	return output;
}

technique Trail
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 vsMain();
		PixelShader = compile ps_3_0 psMain();
	}
}