sampler TextureSampler : register(s0);

float width;
float height;

struct VertexToPixel
{
    float2 texCoord : TEXCOORD;
};

float4 psMain(VertexToPixel input) : COLOR0
{
    float4 Color;
    Color =  6 * tex2D(TextureSampler, input.texCoord.xy);
    Color += tex2D(TextureSampler, input.texCoord.xy + float2(1.0 / width, 0.0));
    Color += tex2D(TextureSampler, input.texCoord.xy - float2(1.0 / width, 0.0));
    Color += tex2D(TextureSampler, input.texCoord.xy + float2(0.0, 1.0 / height));
    Color += tex2D(TextureSampler, input.texCoord.xy - float2(0.0, 1.0 / height));
    Color = Color / 10;

    return Color;
}

technique Simplest
{
	pass Pass0
	{
		PixelShader = compile ps_3_0 psMain();
	}
}