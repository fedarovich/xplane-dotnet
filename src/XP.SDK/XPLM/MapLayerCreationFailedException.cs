using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace XP.SDK.XPLM
{
    [Serializable]
    public class MapLayerCreationFailedException : Exception
    {
        public MapLayerCreationFailedException() : base("Failed to create a map layer.")
        {
        }

        public MapLayerCreationFailedException(string message) : base(message)
        {
        }

        public MapLayerCreationFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MapLayerCreationFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
