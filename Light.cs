using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    class Light
    {
        public Vector3 position;
        public Vector3 color;
        public float ambientStrength = 0.1f;                       //Ambient factor to pas to the Shader on rendering

        public Light(Vector3 position, Vector3 color, float ambientStrenght)
        {
            this.position = position;
            this.color = color;
            this.ambientStrength = ambientStrength; 
        }
    }
}

