#nullable enable

using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace ProgressDemo;

public class IntToProgressConverter : BaseConverter<int, float >
{
	public override float DefaultConvertReturnValue { get; set; } = 0;

	public override int DefaultConvertBackReturnValue { get; set; } = 0;

	public override float ConvertFrom(int value, CultureInfo? culture = null) => (float)(value/100.0f);

	public override int ConvertBackTo(float value, CultureInfo? culture = null) => (int)(value * 100.0f);
}
