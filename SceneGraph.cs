using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    class SceneGraph
    {
        List<Mesh> primaryChildren;                                     //Stores nodes in the first layer of the hierarchy  

        public Matrix4 Tcamera, Tview, cameraMatrix, Tworld;       //Transform matrixes

        public static Shader shader;                                    //Shader to use for rendering
        const float PI = 3.1415926535f;
        Light light1, light2;
        List<Light> lights;
        float[] lightData; 

        //Needs to store a hierarchy of all the meshes that are in the scene. 
        public SceneGraph()
        {
            primaryChildren = new List<Mesh>();
            lights = new List<Light>();
            float angle90degrees = PI / 2;

            //
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");

            //Initializing lights
            light1 = new Light(new Vector3(0f, 1f, 0.5f), new Vector3(5, 5, 5), 0.1f);
            lights.Add(light1);
            light2 = new Light(new Vector3(5f, -50f, 5f), new Vector3(0.99f, 0.99f, 0.99f), 0.1f);
            lights.Add(light2);

            //Initialize the transformation matrixes
            Tcamera = Matrix4.CreateTranslation(new Vector3(0, -14.5f, 0)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), angle90degrees);
            Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
            Tworld = Matrix4.Identity;

            //Persective Matrix
            cameraMatrix = Tcamera * Tview;
        }

        //Render the first layer of the hierarchy
        public void Render()
        {

            // enable shader
            GL.UseProgram(shader.programID);

            //Passing Uniform variables to the Shader
            GL.UniformMatrix4(shader.uniform_viewpos, false, ref cameraMatrix);
            float[] input = listToFloat(lights);
            int length = lights.Count;
            GL.Uniform1(shader.uniform_input, input.Length, input);
            GL.Uniform1(shader.uniform_lightsamount, length);
            //GL.Uniform3(shader.uniform_lightPos, light1.position);
            //GL.Uniform3(shader.uniform_lightColor, light1.color);
            //GL.Uniform1(shader.uniform_ambientStrength, light1.ambient);

            GL.UseProgram(0);

            foreach (Mesh mesh in primaryChildren)
            {
                mesh.Render(shader, Tworld, cameraMatrix, mesh.texture);               
            }
        }
        
        //Add meshes to the first layer of the hierarchy
        public void addPrimaryChild(Mesh mesh)
        {
            primaryChildren.Add(mesh);
        }

        //Handle Key input to move the camera around the scene
        public void HandleInput(KeyboardState state)
        {
            if (state[OpenTK.Input.Key.Up])
            {
                cameraMatrix *= Matrix4.CreateTranslation(0, -0.05f, 0);
            }
            if (state[OpenTK.Input.Key.Down])
            {
                cameraMatrix *= Matrix4.CreateTranslation(0, 0.05f, 0);
            }
            if (state[OpenTK.Input.Key.Left])
            {
                cameraMatrix *= Matrix4.CreateTranslation(0.05f, 0, 0);
            }
            if (state[OpenTK.Input.Key.Right])
            {
                cameraMatrix *= Matrix4.CreateTranslation(-0.05f, 0, 0);
            }
            if (state[OpenTK.Input.Key.W])
            {
                cameraMatrix = Matrix4.CreateRotationX(0.01f) * cameraMatrix;
            }
            if (state[OpenTK.Input.Key.S])
            {
                cameraMatrix = Matrix4.CreateRotationX(-0.01f) * cameraMatrix;
            }
            if (state[OpenTK.Input.Key.A])
            {
                cameraMatrix = Matrix4.CreateRotationY(0.01f) * cameraMatrix;
            }
            if (state[OpenTK.Input.Key.D])
            {
                cameraMatrix = Matrix4.CreateRotationY(-0.01f) * cameraMatrix;
            }
        }

        float[] listToFloat(List<Light> lights)
        {
            float[] result = new float[lights.Count * 8];
            int i = 0;

            foreach(Light light in lights)
            {
                result[i] = light.position.X; i++;
                result[i] = light.position.Y; i++;
                result[i] = light.position.Z; i++;
                result[i] = 0.0f;  i++;
                result[i] = light.color.X; i++;
                result[i] = light.color.Y; i++;
                result[i] = light.color.Z; i++;
                result[i] = light.ambient; i++;
            }
            return result;
        }
    }
}
