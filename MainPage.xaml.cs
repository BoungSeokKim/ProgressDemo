using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProgressDemo;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	int count = 0;

	public int Count { 
		get { return count; }
		set 
		{ 
			count = value;
			OnPropertyChanged();
		}
	}

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		Count++;

		if (Count == 1)
			CounterBtn.Text = $"Clicked {Count} time";
		else
			CounterBtn.Text = $"Clicked {Count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}

