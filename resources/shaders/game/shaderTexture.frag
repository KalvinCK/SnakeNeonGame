#version 460 core

in vec2 TexCoord;

out vec4 FragColor;

uniform float ForceLight = 1.0;

uniform sampler2D inputTex;

vec4 removeAlpha(vec4 base)
{
    if(base.a <= 0.1)
        discard;

    return base;

}
void main()
{
    vec4 base = texture(inputTex, TexCoord);
    base = removeAlpha(base);
    base = base * ForceLight;

    FragColor = base;
}