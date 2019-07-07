float4x4 worldMatrix;

float width;
float height;
float acceleration;

float currentTime;
float2 parentPosition;

struct VertexInput
{
	float2 localVertexPosition : POSITION0;
	float centerDistance : PSIZE0;
	float centerAngle : PSIZE1;
	float centerRotationAcceleration : PSIZE2;
	float vertexRotationAcceleration : PSIZE3;
	float time : PSIZE4;
};

struct VertexToPixel
{
	float4 position : POSITION;
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

	float localTime = currentTime - input.time;
	if (localTime > 0)
	{
		float distance = acceleration * localTime * localTime / 2.0;
		if (distance < input.centerDistance)
		{
			float2 position = parentPosition + rotate(float2(1, 0), input.centerAngle + input.centerRotationAcceleration * localTime * localTime / 2.0) * (input.centerDistance - distance)
				+ rotate(input.localVertexPosition, input.vertexRotationAcceleration * localTime * localTime / 2.0);
			output.position = transformPosition(position);

			output.color = float4(0, 0, 0, distance / input.centerDistance);
		}
	}
	return output;
}

float4 psMain(VertexToPixel input) : COLOR0
{
	return input.color;
}

technique GravityParticles
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 vsMain();
		PixelShader = compile ps_3_0 psMain();
	}
}