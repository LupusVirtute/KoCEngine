#version 450 core
in vec2 TexCoord0;
out vec4 FragColor;

uniform sampler2D gSampler;
uniform vec3 textColor;

void main(void)
{
	FragColor = vec4(textColor,1.0) * texture2D(gSampler,TexCoord0);
}
