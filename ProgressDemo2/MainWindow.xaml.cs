using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProgressDemo2;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private int _progressValue;
    public int ProgressValue
    {
        get => _progressValue;
        set => SetField(ref _progressValue, value);
    }
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void ButtonStart_OnClick(object sender, RoutedEventArgs e)
    {
        Progress.Minimum = 0;
        Progress.Maximum = 100;
        
        /*Observable.Interval(TimeSpan.FromMilliseconds(100))
            .Where(i => i % 10 == 0)
            .Subscribe(x => { ProgressValue = (int)x; });*/

        for (int i = 0; i <= 100; i++)
        {
            await Task.Delay(100);
            ProgressValue = i;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}