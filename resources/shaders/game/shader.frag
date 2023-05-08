#version 460 core

in vec2 TexCoord;

out vec4 FragColor;

uniform vec4 color;
uniform float ForceLight = 1.0;
uniform float alpha = 1.0;

void main()
{
    vec3 base = color.rgb * ForceLight;
    FragColor = vec4(base, alpha);
}