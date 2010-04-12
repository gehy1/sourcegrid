
using System;
using FluentNHibernate.Mapping;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class TrackMap : ClassMap<Track>
	{
		public TrackMap()
		{
			this.Table("Track");
			Id(x => x.TrackId).GeneratedBy.Native("TRACK_GENERATOR");
			Map(x => x.Name);
			Map(x => x.Composer);
		}
	}
	
	public class Track
	{
		public virtual int TrackId {get;set;}
		public virtual string Name {get;set;}
		public virtual string Composer {get;set;}
	}
}
