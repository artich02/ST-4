﻿using Stateless;

namespace BugPro;

public class Bug
{
    public enum State { Open, Assigned, InProgress, Defered, Resolved, Closed, Reopened }
    private enum Trigger { Assign, StartProgress, Defer, Resolve, Close, Reopen, Reassign }
    private StateMachine<State, Trigger> sm;

    public Bug(State state)
    {
        sm = new StateMachine<State, Trigger>(state);

        sm.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);

        sm.Configure(State.Assigned)
            .Permit(Trigger.StartProgress, State.InProgress)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Close, State.Closed)
            .Ignore(Trigger.Assign);

        sm.Configure(State.InProgress)
            .Permit(Trigger.Resolve, State.Resolved)
            .Permit(Trigger.Defer, State.Defered)
            .Permit(Trigger.Reassign, State.Assigned);

        sm.Configure(State.Defered)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);

        sm.Configure(State.Resolved)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        sm.Configure(State.Reopened)
            .Permit(Trigger.Assign, State.Assigned)
            .Permit(Trigger.Close, State.Closed);
    }

    public void Assign() { sm.Fire(Trigger.Assign); Console.WriteLine("Assign"); }
    public void StartProgress() { sm.Fire(Trigger.StartProgress); Console.WriteLine("StartProgress"); }
    public void Defer() { sm.Fire(Trigger.Defer); Console.WriteLine("Defer"); }
    public void Resolve() { sm.Fire(Trigger.Resolve); Console.WriteLine("Resolve"); }
    public void Close() { sm.Fire(Trigger.Close); Console.WriteLine("Close"); }
    public void Reopen() { sm.Fire(Trigger.Reopen); Console.WriteLine("Reopen"); }
    public void Reassign() { sm.Fire(Trigger.Reassign); Console.WriteLine("Reassign"); }

    public State GetState() { return sm.State; }
}

public class Program
{
    public static void Main(string[] args)
    {
        var bug = new Bug(Bug.State.Open);
        bug.Assign();
        bug.StartProgress();
        bug.Resolve();
        bug.Close();
        bug.Reopen();
        bug.Assign();
        bug.Defer();
        Console.WriteLine($"Final state: {bug.GetState()}");
    }
}