using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Input;

namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;                  // background surface for printing etc.
		Mesh Tpot, Tfloor;                // a mesh to draw using OpenGL
		Stopwatch timer;                        // timer for measuring frame duration
		Texture wood;                           // texture to use for rendering
        SceneGraph sceneGraph;                  // SceneGraph used to render the scene 

        const float PI = 3.1415926535f;
        // initialize
        public void Init()
		{
            sceneGraph = new SceneGraph();

            // load a texture
            wood = new Texture("../../assets/wood.jpg");

            // load meshes
            Tpot = new Mesh( "../../assets/teapot.obj", new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0), new List<Mesh>(), wood);
			Tfloor = new Mesh( "../../assets/floor.obj", new Vector3(0, 0, 0), new Vector3(4.0f, 4.0f, 4.0f), new Vector3(0, 0, 0), new List<Mesh>(), wood);

            //Add created meshes to the hierarchy tree
            sceneGraph.addPrimaryChild(Tpot);
            sceneGraph.addPrimaryChild(Tfloor);

			// initialize stopwatch
			timer = new Stopwatch();
			timer.Reset();
			timer.Start();
		}

		// tick for background surface
		public void Tick()
		{
			screen.Clear( 0 );

            KeyboardState state = OpenTK.Input.Keyboard.GetState();
            sceneGraph.HandleInput(state);
		}

		// tick for OpenGL rendering code
		public void RenderGL()
		{
			// measure frame duration
			float frameDuration = timer.ElapsedMilliseconds;
			timer.Reset();
			timer.Start();

			// update rotation
			Tpot.angle.Y += 0.001f * frameDuration;
			if( Tpot.angle.Y > 2 * PI ) Tpot.angle.Y -= 2 * PI;

   //         Tfloor.angle.Y += 0.001f * frameDuration;
   //         if (Tfloor.angle.Y > 2 * PI) Tfloor.angle.Y -= 2 * PI;

            // render scene
            sceneGraph.Render();
		}
	}
}