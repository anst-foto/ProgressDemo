using System.Windows;

namespace ProgressDemo;

public partial class MainWindow : Window
{
    private CancellationTokenSource _cts;
    private int _progressState;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void ButtonStart_OnClick(object sender, RoutedEventArgs e)
    {
        ButtonStart.IsEnabled = false;
        ButtonStop.IsEnabled = true;
        ButtonPause.IsEnabled = true;
        
        var start = 0;
        var end = 50;
        
        Progress.Minimum = start;
        Progress.Maximum = end;

        if (_progressState != 0)
        {
            start = _progressState;
        }
        
        var progress = new Progress<int>();
        progress.ProgressChanged += (_, i) =>
        {
            Progress.Value = i;
            _progressState = i;
        };

        _cts = new CancellationTokenSource();
        try
        {
            await ProgressTestAsync(start, end, progress, _cts.Token);
        }
        catch (OperationCanceledException)
        {
            MessageBox.Show(_progressState == 0 ? $"Operation canceled." : "Operation pause.");
        }
        
        ButtonStart.IsEnabled = true;
        ButtonStop.IsEnabled = false;
        ButtonPause.IsEnabled = false;
    }
    
    private void ButtonPause_OnClick(object sender, RoutedEventArgs e)
    {
        ButtonStart.IsEnabled = true;
        ButtonPause.IsEnabled = false;
        
        _cts.Cancel();
    }

    private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
    {
        ButtonStart.IsEnabled = true;
        ButtonStop.IsEnabled = false;
        ButtonPause.IsEnabled = false;
        
        _progressState = 0;
        Progress.Value = 0;
        
        _cts.Cancel();
    }
    
    private async Task ProgressTestAsync(int start, int stop, IProgress<int>? progress = null, CancellationToken cancellationToken = default)
    {
        /*try
        {
            for (int i = start; i <= stop; i++)
            {
                await Task.Delay(100, cancellationToken);
                progress?.Report(i);
            }
        }
        catch (OperationCanceledException)
        {
            MessageBox.Show("Canceled");
            cancellationToken.ThrowIfCancellationRequested();
        }*/

        for (int i = start; i <= stop; i++)
        {
            /*if (cancellationToken.IsCancellationRequested)
            {
                //return;
                throw new OperationCanceledException();
            }*/
            await Task.Delay(100, cancellationToken);
            progress?.Report(i);
        }
    }
}