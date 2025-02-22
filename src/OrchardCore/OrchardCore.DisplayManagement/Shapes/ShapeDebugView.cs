using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OrchardCore.DisplayManagement.Shapes
{
    public class ShapeDebugView
    {
        private readonly Shape _shape;

        public ShapeDebugView(Shape shape)
        {
            _shape = shape;
        }

        public ShapeMetadata Metadata { get { return _shape.Metadata; } }

        public string Id { get { return _shape.Id; } }
        public IList<string> Classes { get { return _shape.Classes; } }
        public IDictionary<string, string> Attributes { get { return _shape.Attributes; } }
        public IEnumerable<dynamic> Items { get { return _shape.Items; } }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePairs[] Properties
        {
            get
            {
                return _shape.Properties
                    .Cast<KeyValuePair<string, object>>()
                    .Select(entry => new KeyValuePairs(entry.Key, entry.Value))
                    .ToArray();
            }
        }

        [DebuggerDisplay(" { _shapeType == null ? _value : \"Shape: \" + _shapeType}", Name = "{_key,nq}")]
        public class KeyValuePairs
        {
            public KeyValuePairs(string key, object value)
            {

                if (_value != null && _value is IShape)
                    _shapeType = (_value as IShape).Metadata.Type;

                _value = value;
                _key = key;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string _key;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private object _shapeType;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private object _value;
        }
    }
}
