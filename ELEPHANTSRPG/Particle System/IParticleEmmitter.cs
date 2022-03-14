using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ELEPHANTSRPG.Particle_System
{
    public interface IParticleEmmitter
    {
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
    }
}
