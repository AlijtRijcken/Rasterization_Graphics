using OpenTK;
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
        List<Mesh> primaryChildren;
        Matrix4 Tcamera, Tview, cameraMatrix;
        public static Shader shader;                          // shader to use for rendering
        const float PI = 3.1415926535f;

        //Needs to store a hierarchy of all the meshes that are in the scene. 
        public SceneGraph()
        {
            primaryChildren = new List<Mesh>();
            float angle90degrees = PI / 2;
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            Tcamera = Matrix4.CreateTranslation(new Vector3(0, -14.5f, 0)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), angle90degrees);
            Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
            cameraMatrix = Tcamera * Tview;
        }

        public void Render()
        {
            foreach (Mesh mesh in primaryChildren)
            {
                mesh.Render(shader, cameraMatrix, mesh.texture);               
            }
        }

        public void addPrimaryChild(Mesh mesh)
        {
            primaryChildren.Add(mesh);
        }

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
                cameraMatrix *= Matrix4.CreateTranslation(0, 0, 0.05f);
            }
            if (state[OpenTK.Input.Key.S])
            {
                cameraMatrix *= Matrix4.CreateTranslation(0, 0, -0.05f);
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
    }
}
