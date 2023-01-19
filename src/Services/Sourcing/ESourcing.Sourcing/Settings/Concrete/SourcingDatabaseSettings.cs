using System;
using ESourcing.Sourcing.Settings.Abstract;

namespace ESourcing.Sourcing.Settings.Concrete
{
    public class SourcingDatabaseSettings : ISourcingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

