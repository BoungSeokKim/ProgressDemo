using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ProgressDemo;


public class CircleProgress : SKCanvasView
{
	class Circle
	{
		public SKPoint Center { get; }
		public float Radius { get; }
		public Circle(SKPoint center, float radius)
		{
			Center = center;
			Radius = radius;
		}
		public Circle(SKRect rect)
		{
			Center = new SKPoint(rect.MidX, rect.MidY);
			// 반지름을 사각영역 보다 조금 작게 만들어 줌.
			Radius = Math.Min(rect.Height, rect.Width) / 2.5f; 
		}
		public SKRect Rect => new SKRect(
			Center.X - Radius,
			Center.Y - Radius,
			Center.X + Radius,
			Center.Y + Radius);
	}

	public CircleProgress() : base()
	{

	}

	public CircleProgress(float min, float max, float val) : this()
	{
		if (min >= max)
			throw new ArgumentOutOfRangeException(nameof(min));

		if (max > Minimum)
		{
			Maximum = max;
			Minimum = min;
		}
		else
		{
			Minimum = min;
			Maximum = max;
		}

		Value = float.Clamp(val, min, max);
	}

	public float Maximum
	{
		get => (float)GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}
	
	public float Minimum
	{
		get => (float)GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	public float Value
	{
		get => (float)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public int Thickness
	{
		get => (int)GetValue(ThicknessProperty);
		set => SetValue(ThicknessProperty, value);
	}

	public Color ProgressColor
	{
		get => (Color)GetValue(ProgressColorProperty);
		set => SetValue(ProgressColorProperty, value);
	}

	public Color BaseColor
	{
		get => (Color)GetValue(BaseColorProperty);
		set => SetValue(BaseColorProperty, value);
	}

	public event EventHandler<ValueChangedEventArgs> ValueChanged;

	public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(float), typeof(CircleProgress), 100.0f, propertyChanged: OnBindablePropertyChanged, coerceValue: (bindable, value) =>
	{
		var progress = (CircleProgress)bindable;
		progress.Value = Math.Clamp(progress.Value, progress.Minimum, (float)value);
		return value;
	});
	public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(float), typeof(CircleProgress), 0.0f, propertyChanged: OnBindablePropertyChanged, coerceValue: (bindable, value) =>
	{
		var progress = (CircleProgress)bindable;
		progress.Value = Math.Clamp(progress.Value, (float)value, progress.Maximum);
		return value;
	});
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(float), typeof(CircleProgress), 0.0f, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
	{
		var progress = (CircleProgress)bindable;
		if(oldValue != newValue)
		{
			progress.ValueChanged?.Invoke(progress, new ValueChangedEventArgs((float)oldValue, (float)newValue));
			progress.InvalidateSurface();
		}
	}, coerceValue: (bindable, value) =>
	{
		var progress = (CircleProgress)bindable;
		return Math.Clamp((float)value, progress.Minimum, progress.Maximum);
	});
	public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(nameof(Thickness), typeof(int), typeof(CircleProgress), 5, propertyChanged: OnBindablePropertyChanged);
	public static readonly BindableProperty ProgressColorProperty = BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircleProgress), Colors.BlueViolet, propertyChanged: OnBindablePropertyChanged);
	public static readonly BindableProperty BaseColorProperty = BindableProperty.Create(nameof(BaseColor), typeof(Color), typeof(CircleProgress), Colors.LightGray, propertyChanged: OnBindablePropertyChanged);

	private static void OnBindablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		((CircleProgress)bindable).InvalidateSurface();
	}

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		base.OnPaintSurface(e);

		SKImageInfo info = e.Info;
		SKSurface surface = e.Surface;
		SKCanvas canvas = surface.Canvas;

		Circle circle = new Circle(info.Rect);
		canvas.Clear();

		var min = Minimum;
		var max = Maximum;
		var value = Value;

		float angle = ((value - min) / (max - min)) * 360f;

		canvas.DrawCircle(circle.Center, circle.Radius, 
			new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				StrokeWidth = Thickness,
				Color = BaseColor.ToSKColor(),
				IsAntialias = true
			}
		);
		canvas.DrawArc(circle.Rect, 270, angle, false,
			new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				StrokeWidth = Thickness,
				StrokeCap = SKStrokeCap.Round,
				Color = ProgressColor.ToSKColor(),
				IsAntialias = true
			}
		);
	}
}
