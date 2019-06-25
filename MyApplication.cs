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
		Mesh Tpot, Tfloor, Tdog;                // a mesh to draw using OpenGL
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
            //Tdog = new Mesh( "../../assets/dog.obj", new Vector3(0, 0, 0), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0, -7f, 0) );

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

			// prepare matrix for vertex shader
			//float angle90degrees = PI / 2;
			//Matrix4 Tpot = Matrix4.CreateScale( 0.5f ) * Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
			//Matrix4 Tfloor = Matrix4.CreateScale( 4.0f ) * Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
			//Matrix4 Tcamera = Matrix4.CreateTranslation( new Vector3( 0, -14.5f, 0 ) ) * Matrix4.CreateFromAxisAngle( new Vector3( 1, 0, 0 ), angle90degrees );
			//Matrix4 Tview = Matrix4.CreatePerspectiveFieldOfView( 1.2f, 1.3f, .1f, 1000 );

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