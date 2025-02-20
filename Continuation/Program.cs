var tcs = new TaskCompletionSource(/*TaskCreationOptions.RunContinuationsAsynchronously*/);

ThreadPool.QueueUserWorkItem(_ =>
{
    Thread.Sleep(200);
    Console.WriteLine($"T0:{Environment.CurrentManagedThreadId}");
    tcs.SetResult();
});

Console.WriteLine($"M0:{Environment.CurrentManagedThreadId}");

var sync = new object();

Monitor.Enter(sync);

//tcs.Task
//    .ContinueWith(_ =>
//        Console.WriteLine($"C1:{Environment.CurrentManagedThreadId}")
//        ,TaskContinuationOptions.ExecuteSynchronously
//        );

tcs.Task.Wait();
 


Console.WriteLine($"M1:{Environment.CurrentManagedThreadId}");

Monitor.Exit(sync);

Thread.Sleep(1000);