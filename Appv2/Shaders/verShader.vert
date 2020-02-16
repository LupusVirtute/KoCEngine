#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 TexCoord;

out vec2 TexCoord0;

layout (location = 20) uniform  mat4 projection;
layout (location = 21) uniform  mat4 camera;
layout (location = 22) uniform  mat4 modelView;

void main(void)
{
    gl_Position = projection*camera* modelView * vec4(position,1.0f);
    TexCoord0 = TexCoord;
}