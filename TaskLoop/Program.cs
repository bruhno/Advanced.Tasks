var taskLoop = new TaskLoop
{
    A = () => Console.WriteLine($"After delay {Environment.CurrentManagedThreadId}"),
    Max = 5,
};

Console.WriteLine($"Hello world {Environment.CurrentManagedThreadId}");

taskLoop.Run();

taskLoop.Task.Wait();

class TaskLoop
{
    public required Action A { get; init; }

    public required int Max { get; init; }

    public Task Task => _tcs.Task;

    public void Run()
    {
        var task0 = new Task(A);

        ContinueWithDelay(task0, Max - 1);

        task0.Start();
    }

    private void ContinueWithDelay(Task task, int counter)
    {
        if (counter == 0)
        {
            task.ContinueWith(_ =>
            {
                _timers.ForEach(t => t.Dispose());
                _timers.Clear();
                _tcs.SetResult();
            });

            return;
        }

        task.ContinueWith(_ =>
        {
            _timers.Add(new Timer(_ =>
                {
                    var task1 = new Task(A);

                    ContinueWithDelay(task1, counter - 1);

                    task1.Start();
                },
                null,
                _DELAY,
                0
                ));
        });
    }

    private readonly List<Timer> _timers = new();
    private readonly TaskCompletionSource _tcs = new();
    private const int _DELAY = 2000;
}