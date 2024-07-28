using System;
using System.Threading.Tasks;
using ReactiveUI;

namespace SpaceView.ViewModels;

public class NotifyTaskCompletion<TResult> : ReactiveObject
{
    public NotifyTaskCompletion(Task<TResult> task)
    {
        Task = task;
        if (!task.IsCompleted)
        {
            var _ = WatchTaskAsync(task);
        }
    }

    public Task<TResult> Task { get; private set; }
    public TResult? Result => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default;
    public TaskStatus Status => Task.Status;
    public bool IsCompleted => Task.IsCompleted;
    public bool IsNotCompleted => !Task.IsCompleted;
    public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
    public bool IsCanceled => Task.IsCanceled;
    public bool IsFaulted => Task.IsFaulted;
    public AggregateException? Exception => Task.Exception;
    public Exception? InnerException => (Exception == null) ? null : Exception.InnerException;
    public string? ErrorMessage => (InnerException == null) ? null : InnerException.Message;

    public async Task WatchTaskAsync(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception e)
        {
        }

        this.RaisePropertyChanged(nameof(Status));
        this.RaisePropertyChanged(nameof(IsCompleted));
        this.RaisePropertyChanged(nameof(IsNotCompleted));

        if (task.IsCanceled)
        {
            this.RaisePropertyChanged(nameof(IsCanceled));
        }
        else if (task.IsFaulted)
        {
            this.RaisePropertyChanged(nameof(IsFaulted));
            this.RaisePropertyChanged(nameof(Exception));
            this.RaisePropertyChanged(nameof(InnerException));
            this.RaisePropertyChanged(nameof(ErrorMessage));
        }
        else
        {
            this.RaisePropertyChanged(nameof(IsSuccessfullyCompleted));
            this.RaisePropertyChanged(nameof(Result));
        }
    }
}