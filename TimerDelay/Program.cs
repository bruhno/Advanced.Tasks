using System.Diagnostics;

var sw = Stopwatch.StartNew();

var task = MyTimer.Delay(1500);

Console.WriteLine($"{sw.ElapsedMilliseconds}: Delay started");

task.Wait();

Console.WriteLine($"{sw.ElapsedMilliseconds}: Delay finished");

public class MyTimer
{
    public static Task Delay(int milliseconds)
    {
        var tcs = new TaskCompletionSource();

        var timer = new MyTimer(milliseconds, tcs);

        return tcs.Task;
    }

    private MyTimer(int milliseconds, TaskCompletionSource tcs)
    {
        _milliseconds = milliseconds;
        _tcs = tcs;

        _timer = new Timer(
            Callback,
            null,
            _milliseconds,
            Timeout.Infinite);
    }

    private void Callback(object? _)
    {
        _timer.Dispose();
        _tcs.SetResult();
    }

    private int _milliseconds;
    private readonly TaskCompletionSource _tcs;
    private readonly Timer _timer;
}