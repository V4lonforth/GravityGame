float4x4 worldMatrix;

float width;
float height;

float currentTime;
float duration;

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

float2 rotate(float2 position, float angle)
{
	return float2	(position.x * cos(angle) - position.y * sin(angle),
					position.y * cos(angle) + position.x * sin(angle));
}

float4 transformPosition(float2 position)
{
	float4 outPosition = mul(float4(position, 1, 1), worldMatrix);
	outPosition.x = outPosition.x / width - 1.0;
	outPosition.y = -outPosition.y / height + 1.0;
	return outPosition;
}

VertexToPixel vsMain(VertexInput input)
{
	VertexToPixel output = (VertexToPixel)0;

    float2 position = input.position + input.speed * (currentTime - input.time);
    float angle = input.rotationSpeed * (currentTime - input.time);
    position = position + rotate(input.localPosition, angle);

	output.position = transformPosition(position);
	output.color = float4(input.color.rgb, 1 - (currentTime - input.time) / duration);
	return output;
}

PixelToFrame psMain(VertexToPixel input)
{
	PixelToFrame output = (PixelToFrame)0;
	output.color = input.color;
	return output;
}

technique PortalParticles
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 vsMain();
		PixelShader = compile ps_3_0 psMain();
	}
}