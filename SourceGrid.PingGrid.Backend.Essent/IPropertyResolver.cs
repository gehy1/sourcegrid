
using System;

namespace SourceGrid.PingGrid.Backend.Essent
{
	/// <summary>
	/// An extension point, where users could define their own 
	/// property resolver
	/// </summary>
	public interface IPropertyResolver 
	{
		object ReadValue(object obj, string propertyPath);
	}
	

}
