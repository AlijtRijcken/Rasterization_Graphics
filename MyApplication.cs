using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Input;

namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;                                                              // background surface for printing etc.
		Mesh Tpot, Tfloor, Tpot2, Tpot3, Tfloor1, Tfloor2, Tfloor3;                         // a mesh to draw using OpenGL
		Stopwatch timer;                                                                    // timer for measuring frame duration
		Texture wood, wood1, metal, rust, blue, yellow;                                      // texture to use for rendering
        SceneGraph sceneGraph;                                                              // SceneGraph used to render the scene 
        const float PI = 3.1415926535f;
       
        // initialize
        public void Init()
		{
            sceneGraph = new SceneGraph();

            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            wood1 = new Texture("../../assets/wood1.jpg");
            metal = new Texture("../../assets/metal.jpg");
            rust = new Texture("../../assets/rust.jpg");
            blue = new Texture("../../assets/blue.jpg");
            yellow = new Texture("../../assets/yellow.jpg");

            // load meshes
            Tpot = new Mesh( "../../assets/teapot.obj", new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0), new List<Mesh>(), wood);
            Tpot2 = new Mesh( "../../assets/teapot.obj", new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(5f, 0, 0), new List<Mesh>(), blue);
            Tpot3 = new Mesh( "../../assets/teapot.obj", new Vector3(0, 0, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-5f, 6f, 0), new List<Mesh>(), wood1);
			Tfloor = new Mesh( "../../assets/floor.obj", new Vector3(0, 0, 0), new Vector3(4.0f, 4.0f, 4.0f), new Vector3(0, 0, 0), new List<Mesh>(), rust);
			Tfloor1 = new Mesh( "../../assets/floor.obj", new Vector3(1, 0, 0), new Vector3(4.0f, 4.0f, 4.0f), new Vector3(0, 0, -25f), new List<Mesh>(), rust);
			Tfloor2 = new Mesh( "../../assets/floor.obj", new Vector3(0, 1, 0), new Vector3(4.0f, 4.0f, 4.0f), new Vector3(25f, 0, 0), new List<Mesh>(), metal);
            Tfloor3 = new Mesh("../../assets/floor.obj", new Vector3(1, 1, 0), new Vector3(4.0f, 4.0f, 4.0f), new Vector3(0, -1f, 0), new List<Mesh>(), yellow);

            //Add created meshes to the hierarchy tree
            sceneGraph.addPrimaryChild(Tpot);
            sceneGraph.addPrimaryChild(Tfloor);
            sceneGraph.addPrimaryChild(Tpot2);


            //Tpot.addChild(Tpot2);
            Tpot.addChild(Tpot3);
            Tfloor.addChild(Tfloor1);
            Tfloor.addChild(Tfloor2);
            Tfloor.addChild(Tfloor3);


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

            Tpot2.angle.X += 0.001f * frameDuration;
            if (Tpot2.angle.X > 2 * PI) Tpot2.angle.X -= 2 * PI;

            Tpot3.angle.Y += 0.001f * frameDuration;
            if (Tpot3.angle.Y > 2 * PI) Tpot3.angle.Y -= 2 * PI;

            Tfloor.angle.Y += 0.001f * frameDuration;
            if (Tfloor.angle.Y > 2 * PI) Tfloor.angle.Y -= 2 * PI;

            Tfloor3.angle.X += 0.001f * frameDuration;
            if (Tfloor3.angle.X > 2 * PI) Tfloor3.angle.X -= 2 * PI;


            // render scene
            sceneGraph.Render();
		}
	}
}