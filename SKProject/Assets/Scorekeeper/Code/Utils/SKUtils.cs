//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class SKExtensions
{
	public static TweenConfig destroyWhenComplete(this TweenConfig config)
	{
		config.onComplete((tween) => {((tween as Tween).target as SKDestroyable).Destroy();});	
		return config;
	}
}

public interface SKDestroyable
{
	void Destroy();
}