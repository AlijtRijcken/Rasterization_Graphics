#version 330
 
// shader input
in vec2 uv;					// interpolated texture coordinates
in vec3 normal;					// interpolated normal
uniform sampler2D pixels;			// texture sampler
in vec3 FragPos; 				//gl_Postion set to Frag Position from VertexShader

//uniform vec3 lightPos; 			//uniform position -> add in Template
//uniform vec3 lightColor; 
//uniform vec3 lightPos;

uniform vec3 viewPos;				//takes camera position to get to world space  

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
	vec3 ambient = vec3(0.1);		//HARDCODED -> NEEDS TO BECOME UNIFORM
	vec3 lightColor = vec3(1);  		//HARDCODED -> NEEDS TO BECOME UNIFORM 
    	vec3 lightPos = vec3(0, 6, 0);		//HARDCODED -> NEEDS TO BECOME UNIFORM

	//DIFFUSE LIGHT

	vec3 norm = normalize(normal);

	vec3 lightDir = normalize(lightPos - FragPos);	//direction between the light and the fragment position 

	float diff = max(dot(norm, lightDir), 0.0);	//calculate the diffuse impact
	vec3 diffuse = diff * lightColor;

	//SPECULAR LIGHT
	float specularStrength = 5;
	
	vec3 viewDir = normalize(viewPos - FragPos);	//view direction vector
	vec3 reflectDir = reflect(-lightDir, norm); 	//reflect vector along the normal axis

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * lightColor;		//calculating specular component

	
	//RESULTING LIGHT 

	//adding ambient+diffuse component to get fragments output color
	// texture(pixels, uv).xyz = texture color, Color of object

	vec3 result = ( ambient + diffuse + specular) * texture(pixels, uv).xyz;
	outputColor = vec4(result, 1.0);
	 
}