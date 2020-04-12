#version 450 core
in vec2 TexCoord0;
out vec4 FragColor;

uniform sampler2D gSampler;
uniform vec3 textColor;

void main(void)
{
	vec4 textureRGB =  texture(gSampler,TexCoord0);
	FragColor = textureRGB;
}
