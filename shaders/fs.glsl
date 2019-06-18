#version 330
 
// shader input
in vec2 uv;					// interpolated texture coordinates
in vec3 normal;					// interpolated normal
uniform sampler2D pixels;			// texture sampler
in vec3 FragPos; 				//gl_Postion set to Frag Position from VertexShader

//uniform vec3 lightPos; 			//uniform position -> add in Template
//uniform vec3 lightColor; 
//uniform vec3 lightPos;  

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	vec3 ambient = vec3(0.1);		//HARDCODED -> NEEDS TO BECOME UNIFORM
	vec3 lightColor = vec3(1);  		//HARDCODED -> NEEDS TO BECOME UNIFORM 
    	vec3 lightPos = vec3(0, 6, 0);		//HARDCODED -> NEEDS TO BECOME UNIFORM

	vec3 norm = normalize(normal);

	//direction between the light and the fragment position 
	vec3 lightDir = normalize(lightPos - FragPos);

	//calculate the diffuse impact
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	//adding ambient+diffuse component to get fragments output color
	vec3 result = (ambient + diffuse) * texture(pixels, uv).xyz;
	outputColor = vec4(result, 1.0);
	 
}