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

    public Task Task { get; private set; } = null!;

    public void Run()
    {
        if (Task is not null)
        {

            throw new InvalidOperationException("Task has been alredy run");
        }

        var tcs = new TaskCompletionSource();

        Task = tcs.Task;

        var task0 = new Task(A);

        var task1 = task0;

        for (var i = 1; i < Max; i++)
        {
            task1 = task1.ContinueWith(_ => A());
        }

        task1.ContinueWith(_ => tcs.SetResult());

        task0.Start();
    }
}