#version 330

struct Light {
	vec4 position;
	vec4 color; // w = ambient
};

// shader input
in vec2 uv;					// interpolated texture coordinates
in vec3 normal;					// interpolated normal
uniform sampler2D pixels;			// texture sampler
in vec3 FragPos; 				//gl_Postion set to Frag Position from VertexShader

//uniform vec3 lightPos; 				//uniform position of a light
//uniform vec3 lightColor; 			//uniform color of a light

uniform float[] lights;
uniform int lightsamount;

uniform vec3 viewPos;				//takes camera position to get to world space  

// shader output
out vec4 outputColor;

vec3 CalcLight(Light light, vec3 norm, vec3 fragPos, vec3 viewDir)
{
	//AMBIENT LIGHT
	//vec3 ambient = light.ambient * light.color; 
	vec3 ambient = light.color.w * light.color.xyz;
	
	//DIFFUSE LIGHT

	vec3 lightDir = normalize(light.position.xyz - fragPos);	//direction between the light and the fragment position 
	
	float diff = max(dot(norm, lightDir), 0.0);	//calculate the diffuse impact
	
	vec3 diffuse = diff * light.color.xyz;
	
	//SPECULAR LIGHT
	float specularStrength = 10;
	
	vec3 reflectDir = reflect(-lightDir, norm); 	//reflect vector along the normal axis

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * light.color.xyz;		//calculating specular component
	
	float distance = length(light.position.xyz - fragPos);
	float attenuation = 1.0 / (distance * distance);

	diffuse *= attenuation;
	specular *= attenuation;
	return (ambient + diffuse + specular);
}
	

// fragment shader
void main()
{	
	vec3 norm = normalize(normal);	
	vec3 viewDir = normalize(viewPos - FragPos);	//view direction vector

	vec3 result = vec3(0.0);

	Light l;
	l.position.x = lights[0];
	l.position.y = lights[1];
	l.position.z = lights[2];
	l.color.x = lights[4];
	l.color.y = lights[5];
	l.color.z = lights[6];
	l.color.w = lights[7];
	
	result += CalcLight(l, norm, FragPos, viewDir);

	result *= texture(pixels, uv).xyz;
	
	//outputColor = vec4(l.position.xyz, 1.0);
	outputColor = vec4(result, 1.0);	 
}

