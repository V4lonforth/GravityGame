sampler TextureSampler : register(s0);

texture portalMap;
sampler PortalMapSampler
{
    Texture = <portalMap>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexToPixel
{
    float2 texCoord : TEXCOORD;
};

float4 psMain(VertexToPixel input) : COLOR0
{
	return tex2D(PortalMapSampler, input.texCoord).r * tex2D(TextureSampler, input.texCoord);
}

technique PortalMap
{
	pass Pass0
	{
		PixelShader = compile ps_3_0 psMain();
	}
}