namespace ProgressDemo;

public static class Registation
{
	public static MauiAppBuilder UsePhotoZoneControls(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handler => 
		{
			handler.AddHandler<CircleProgress, CircleProgressHnadler>();
		});

		return builder;
	}
}
