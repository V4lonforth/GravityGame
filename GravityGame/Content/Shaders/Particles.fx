float4x4 worldMatrix;

float width;
float height;

float currentTime;
float duration;

static const float PI = 3.14159265f;

struct VertexInput
{
	float2 position : POSITION0;
	float2 localPosition : POSITION1;
    float2 speed : POSITION2;
	float rotationSpeed : PSIZE0;
	float time : PSIZE1;
	float4 color : COLOR0;
};

struct VertexToPixel
{
	float4 position : POSITION;
	float4 color : COLOR0;
};

struct PixelToFrame
{
	float4 color : COLOR0;
};

VertexToPixel vsMain(VertexInput input)
{
	VertexToPixel output = (VertexToPixel)0;

    float2 position = input.position + input.speed * (currentTime - input.time);
    float angle = input.rotationSpeed * (currentTime - input.time);
    position = position + float2(input.localPosition.x * cos(angle) - input.localPosition.y * sin(angle),
								 input.localPosition.y * cos(angle) + input.localPosition.x * sin(angle));
    output.position = mul(float4(position, 1, 1), worldMatrix);
	output.position.x = output.position.x / width - 1.0;
	output.position.y = -output.position.y / height + 1.0;
	output.color = float4(input.color.rgb, 1 - (currentTime - input.time) / duration);
	return output;
}

PixelToFrame psMain(VertexToPixel input)
{
	PixelToFrame output = (PixelToFrame)0;
	output.color = input.color;
	return output;
}

technique Particles
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 vsMain();
		PixelShader = compile ps_3_0 psMain();
	}
}