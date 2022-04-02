using System;
using System.Collections.Generic;
using System.Text;
using ELEPHANTSRPG.Maps;

namespace ELEPHANTSRPG.Objects
{
    public class ObjectCollisionHelper
    {
        private Tilemap _map;
        private WorldObject _object;

        public ObjectCollisionHelper(Tilemap map, WorldObject thing)
        {
            _map = map;
            _object = thing;
        }

        public bool CheckForMapCollision()
        {
            return _map.CollidesWith(_object);
        }

        public bool CheckForObjectToObjectCollisions(WorldObject thing)
        {
            return _object.Bounds.CollidesWith(thing.Bounds);
        }
    }
}
