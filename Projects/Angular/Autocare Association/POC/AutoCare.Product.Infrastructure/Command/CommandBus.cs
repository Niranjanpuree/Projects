using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.Command
{
    public class CommandBus : ICommandBus
    {
        private readonly IDictionary<Type, List<object>> _commandHandlers = null; 
        public CommandBus()
        {
            _commandHandlers = new Dictionary<Type, List<object>>();
        }
        public Task RegisterAsync<T, THandler>(T command, THandler commandHandler)
            where T : class, ICommand
            where THandler : class, ICommandHandler<T>
        {
            return Task.Run(() =>
            {
                List<object> handlers;
                if (!_commandHandlers.TryGetValue(typeof(T), out handlers))
                {
                    handlers = new List<object>();
                    _commandHandlers.Add(typeof(T), handlers);
                }

                handlers.Add(commandHandler);

            });
        }

        public async Task SendAsync<T>(T command)
            where T : class, ICommand
        {
            await Send(command);
        }

        private Task Send<T>(T command)
            where T : class, ICommand
        {
            return Task.Run(() =>
            {
                List<object> handlers;
                if (!_commandHandlers.TryGetValue(typeof(T), out handlers))
                {
                    return;
                }

                foreach (var handler in handlers)
                {
                    var commandHandler = handler as ICommandHandler<T>;
                    commandHandler?.Handle(command);
                }

            });
        }
    }
}
