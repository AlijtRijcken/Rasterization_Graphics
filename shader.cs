﻿using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Template
{
	public class Shader
	{
		// data members
		public int programID, vsID, fsID;
		public int attribute_vpos;
		public int attribute_vnrm;
		public int attribute_vuvs;
		public int uniform_mview, uniform_viewpos;
        public int uniform_tworld;
        public int uniform_input;                   //float array filled with the light data
        public int uniform_lightsamount;            //total lights in the existing light list (SceneGraph)


        // constructor
        public Shader( String vertexShader, String fragmentShader )
		{
			// compile shaders
			programID = GL.CreateProgram();
			Load( vertexShader, ShaderType.VertexShader, programID, out vsID );
			Load( fragmentShader, ShaderType.FragmentShader, programID, out fsID );
			GL.LinkProgram( programID );
			Console.WriteLine( GL.GetProgramInfoLog( programID ) );

			// get locations of shader parameters
			attribute_vpos = GL.GetAttribLocation( programID, "vPosition" );
			attribute_vnrm = GL.GetAttribLocation( programID, "vNormal" );
			attribute_vuvs = GL.GetAttribLocation( programID, "vUV" );
			uniform_mview = GL.GetUniformLocation( programID, "transform" );
            uniform_tworld = GL.GetUniformLocation(programID, "toworld");
			uniform_viewpos = GL.GetUniformLocation( programID, "viewPos" );
            uniform_input = GL.GetUniformLocation(programID, "lights");
            uniform_lightsamount = GL.GetUniformLocation(programID, "lightsamount");
        }

		// loading shaders
		void Load( String filename, ShaderType type, int program, out int ID )
		{
			// source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
			ID = GL.CreateShader( type );
			using( StreamReader sr = new StreamReader( filename ) ) GL.ShaderSource( ID, sr.ReadToEnd() );
			GL.CompileShader( ID );
			GL.AttachShader( program, ID );
			Console.WriteLine( GL.GetShaderInfoLog( ID ) );
		}
	}
}
