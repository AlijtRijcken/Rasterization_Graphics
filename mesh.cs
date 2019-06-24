using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace Template
{
	// mesh and loader based on work by JTalton; http://www.opentk.com/node/642

	public class Mesh
	{
		// data members
		public ObjVertex[] vertices;            // vertex positions, model space
		public ObjTriangle[] triangles;         // triangles (3 vertex indices)
		public ObjQuad[] quads;                 // quads (4 vertex indices)
        public Vector3 angle, scale, position;  //model matrix variables
        public List<Mesh> children;             //List to store hierarchy of meshes
        public Texture texture;
		int vertexBufferId;                     // vertex buffer
		int triangleBufferId;                   // triangle buffer
		int quadBufferId;                       // quad buffer

		// constructor
		public Mesh( string fileName, Vector3 angle, Vector3 scale, Vector3 position, List<Mesh> children, Texture texture )
		{
			MeshLoader loader = new MeshLoader();
			loader.Load( this, fileName );

            this.angle = angle;
            this.scale = scale;
            this.position = position;
            this.children = children;
            this.texture = texture;
		}

        public void addChild(Mesh mesh)
        {
            children.Add(mesh);
        }
    
        //Local transform matrix
        public Matrix4 ModelMatrix
        {
            get
            {
                return Matrix4.CreateRotationX(angle.X) * Matrix4.CreateRotationY(angle.Y) * Matrix4.CreateRotationZ(angle.Z) * 
                       Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(position);
            }
        }

        public void Rotation(float rotationSpeed)
        {

        }

		// initialization; called during first render
		public void Prepare( Shader shader )
		{
			if( vertexBufferId == 0 )
			{
				// generate interleaved vertex data (uv/normal/position (total 8 floats) per vertex)
				GL.GenBuffers( 1, out vertexBufferId );
				GL.BindBuffer( BufferTarget.ArrayBuffer, vertexBufferId );
				GL.BufferData( BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Marshal.SizeOf( typeof( ObjVertex ) )), vertices, BufferUsageHint.StaticDraw );

				// generate triangle index array
				GL.GenBuffers( 1, out triangleBufferId );
				GL.BindBuffer( BufferTarget.ElementArrayBuffer, triangleBufferId );
				GL.BufferData( BufferTarget.ElementArrayBuffer, (IntPtr)(triangles.Length * Marshal.SizeOf( typeof( ObjTriangle ) )), triangles, BufferUsageHint.StaticDraw );

				// generate quad index array
				GL.GenBuffers( 1, out quadBufferId );
				GL.BindBuffer( BufferTarget.ElementArrayBuffer, quadBufferId );
				GL.BufferData( BufferTarget.ElementArrayBuffer, (IntPtr)(quads.Length * Marshal.SizeOf( typeof( ObjQuad ) )), quads, BufferUsageHint.StaticDraw );
			}
		}

		// render the mesh using the supplied shader and matrix
		public void Render( Shader shader, Matrix4 transform, Texture texture )
		{
			// on first run, prepare buffers
			Prepare( shader );

			// safety dance
			GL.PushClientAttrib( ClientAttribMask.ClientVertexArrayBit );

			// enable texture
			int texLoc = GL.GetUniformLocation( shader.programID, "pixels" );
			GL.Uniform1( texLoc, 0 );
			GL.ActiveTexture( TextureUnit.Texture0 );
			GL.BindTexture( TextureTarget.Texture2D, texture.id );

			// enable shader
			GL.UseProgram( shader.programID );

            // pass transform to vertex shader
            Matrix4 newMatrix = ModelMatrix * transform;
			GL.UniformMatrix4( shader.uniform_mview, false, ref newMatrix);
           // GL.Uniform3(shader.uniform_viewpos, 0, 10, 0);          /////////////////////////////////////HARDCODE CAMERA POSITION
           // GL.Uniform3(shader.uniform_lightPos, 0, 6, 0); //////////////////////////////////HARDCODED LIGHTPOSITION
           // GL.Uniform3(shader.uniform_lightColor, 1, 1, 1); //////////////////////////////////HARDCODED LIGHTCOLOR

            // enable position, normal and uv attributes
            GL.EnableVertexAttribArray( shader.attribute_vpos );
			GL.EnableVertexAttribArray( shader.attribute_vnrm );
			GL.EnableVertexAttribArray( shader.attribute_vuvs );

			// bind interleaved vertex data
			GL.EnableClientState( ArrayCap.VertexArray );
			GL.BindBuffer( BufferTarget.ArrayBuffer, vertexBufferId );
			GL.InterleavedArrays( InterleavedArrayFormat.T2fN3fV3f, Marshal.SizeOf( typeof( ObjVertex ) ), IntPtr.Zero );

			// link vertex attributes to shader parameters 
			GL.VertexAttribPointer( shader.attribute_vuvs, 2, VertexAttribPointerType.Float, false, 32, 0 );
			GL.VertexAttribPointer( shader.attribute_vnrm, 3, VertexAttribPointerType.Float, true, 32, 2 * 4 );
			GL.VertexAttribPointer( shader.attribute_vpos, 3, VertexAttribPointerType.Float, false, 32, 5 * 4 );

			// bind triangle index data and render
			GL.BindBuffer( BufferTarget.ElementArrayBuffer, triangleBufferId );
			GL.DrawArrays( PrimitiveType.Triangles, 0, triangles.Length * 3 );

			// bind quad index data and render
			if( quads.Length > 0 )
			{
				GL.BindBuffer( BufferTarget.ElementArrayBuffer, quadBufferId );
				GL.DrawArrays( PrimitiveType.Quads, 0, quads.Length * 4 );
			}

			// restore previous OpenGL state
			GL.UseProgram( 0 );
			GL.PopClientAttrib();

            foreach (Mesh child in children)
            {
                child.Render(SceneGraph.shader, newMatrix, child.texture);
            }
		}

		// layout of a single vertex
		[StructLayout( LayoutKind.Sequential )]
		public struct ObjVertex
		{
			public Vector2 TexCoord;
			public Vector3 Normal;
			public Vector3 Vertex;
		}

		// layout of a single triangle
		[StructLayout( LayoutKind.Sequential )]
		public struct ObjTriangle
		{
			public int Index0, Index1, Index2;
		}

		// layout of a single quad
		[StructLayout( LayoutKind.Sequential )]
		public struct ObjQuad
		{
			public int Index0, Index1, Index2, Index3;
		}
	}
}