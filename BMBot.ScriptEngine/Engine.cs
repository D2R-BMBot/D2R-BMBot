using System.Threading.Channels;

namespace ScriptEngine;

public class Engine
{
    public PriorityQueue<ScriptCommand, ActionPriority> ActionQueue { get; } = new();

    public async Task RunActions()
    {
        var channel = Channel.CreateUnbounded<ScriptCommand>();
        
        var script1 = new Script();
        
        script1.Commands.Enqueue(new ScriptCommand(async () =>
                                               {
                                                   Console.WriteLine("Starting Script 1!");
                                                   await Task.CompletedTask;
                                               }, ActionPriority.LOW));

        
        for ( var i = 0; i < 100_000; ++i )
        {
            var counter = i;
            script1.Commands.Enqueue(new ScriptCommand(async () =>
                                                       {
                                                           Console.WriteLine($"Script 1: Action {counter}");
                                                           await Task.Delay(Random.Shared.Next(100, 2250));
                                                       }, ActionPriority.LOW));
        }
        
        var script2 = new Script();
        
        script2.Commands.Enqueue(new ScriptCommand(async () =>
                                                   {
                                                       Console.WriteLine("Starting Script 2!");
                                                       await Task.CompletedTask;
                                                   }, ActionPriority.IMMEDIATE));
        
        for ( var i = 0; i < 100_000; ++i )
        {
            var counter = i;
            script2.Commands.Enqueue(new ScriptCommand(async () =>
                                                       {
                                                           Console.WriteLine($"Script 2: Action {counter}");
                                                           await Task.Delay(Random.Shared.Next(100, 2250));
                                                       }, ActionPriority.IMMEDIATE));
        }

        var channelTask = WriteToQueue(channel.Reader);
        await RunScript(script1, channel.Writer);
        await RunScript(script2, channel.Writer);
        var processQueueTask = ProcessQueue();

        var result = await Task.WhenAny([channelTask, processQueueTask]);
    }

    private async Task WriteToQueue(ChannelReader<ScriptCommand> p_reader)
    {
        await foreach ( var command in p_reader.ReadAllAsync() )
        {
            ActionQueue.Enqueue(command, command.Priority);
        }
    }
    
    public async Task RunScript(Script p_script, ChannelWriter<ScriptCommand> p_channelWriter)
    {
        while ( p_script.Commands.Count != 0)
        {
            await p_channelWriter.WriteAsync(p_script.Commands.Dequeue());
        }
    }

    private async Task ProcessQueue()
    {
        while ( true )
        {
            while ( ActionQueue.Count == 0 )
            {
                await Task.Delay(10);
            }
            
            var action = ActionQueue.Dequeue();

            await action.Execute();
        }
    }
}