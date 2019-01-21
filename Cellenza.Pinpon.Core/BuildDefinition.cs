using System;
using System.Collections.Generic;
using System.Text;

namespace Cellenza.Pinpon.Core
{
    internal struct BuildDefinition
    {
        public Guid Collection { get; }
        public Guid Project { get; }
        public int Id { get; }

        public BuildDefinition(Guid collection, Guid project, int id)
        {
            Collection = collection;
            Project = project;
            Id = id;
        }
    }
}
