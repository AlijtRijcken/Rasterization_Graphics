Guido Bekkers & Alijt Rijcken (6232590 & 6132981)

We implemented the basic requirements:
- Added a model matrix to the Mesh class
- Changed it Contsructor to compute the ModelMatrix
- Created a SceneGraph Class which stores the Camera matrix, the vieuw matrix, and the world matrix
- In the SceneGraph the Lights are initialized,
- It holds the List of PrimaryChildren to create the hierarchy,
- It handels the Keyboard state. 
- The SceneGraph.Render() renders all the meshes in the PrimaryChildren List
- The Mesh.Render() work recursifly to renders it's own children.
- Added the Phong Shading model to the Fragment Shader using uniform variables. 

- We made a float array contain all the data of the lights (position, color, ambient). The exchange of this array to the fragment shader
works fine, but when using the data from more lights in de fragment shader the screen turns black. We tried multiple ways of writing the data
Also, multiple TA helped us with the problem, but nobody could explain this weird phenomenon. 