#version 450 core
in vec2 TexCoord0;
out vec4 FragColor;

uniform sampler2D gSampler;
uniform vec3 textColor;

void main(void)
{
	FragColor =  texture2D(gSampler,TexCoord0) - vec4(1.0f,1.0f,1.0f,.5f);
}
