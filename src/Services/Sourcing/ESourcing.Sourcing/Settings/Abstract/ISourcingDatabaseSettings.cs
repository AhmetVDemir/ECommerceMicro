using System;
namespace ESourcing.Sourcing.Settings.Abstract
{
	public interface ISourcingDatabaseSettings
	{
		public string ConnectionString { get; set; }

		public string DatabaseName { get; set; }

	}
}

