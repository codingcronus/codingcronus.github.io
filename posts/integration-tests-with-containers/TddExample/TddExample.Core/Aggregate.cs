using System;

namespace TddExample.Core
{
    public abstract class Aggregate
    {
        public string Id { get; set; }

        public Aggregate(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
