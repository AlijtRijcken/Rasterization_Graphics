﻿#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position

// shader output
out vec3 normal;			// transformed vertex normal
out vec2 uv;
out vec3 FragPos; 				
uniform mat4 transform;
uniform mat4 toworld;
 
// vertex shader
void main()
{
	// transform vertex using supplied matrix
	gl_Position = transform * vec4(vPosition, 1.0);

	//passing gl_Position to Fragment Shader	
	FragPos = (toworld * vec4(vPosition, 1.0)).xyz; 

	// forward normal and uv coordinate; will be interpolated over triangle
	normal = (toworld * vec4(vNormal, 1.0)).xyz;
	uv = vUV;
}