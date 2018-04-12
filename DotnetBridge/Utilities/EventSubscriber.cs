﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Westwind.WebConnection
{
    /// <summary>
    /// FoxPro interop access to .NET events. Handles all events of a source object for subsequent retrieval by a FoxPro client.
    /// </summary>
    public sealed class EventSubscriber : IDisposable
    {
        private readonly object _source;
        private readonly List<Delegate> _eventHandlers = new List<Delegate>();
        private readonly ConcurrentQueue<InteropEvent> _interopEvents = new ConcurrentQueue<InteropEvent>();
        private TaskCompletionSource<InteropEvent> _completion = new TaskCompletionSource<InteropEvent>();

        public EventSubscriber(object source)
        {
            // Indicates that initially the client is not waiting.
            _completion.SetResult(null);

            // For each event, adds a handler that calls QueueInteropEvent.
            this._source = source;
            foreach (var ev in source.GetType().GetEvents()) {
                var eventParams = ev.EventHandlerType.GetMethod("Invoke").GetParameters().Select(p => Expression.Parameter(p.ParameterType)).ToArray();
                var eventHandlerLambda = Expression.Lambda(ev.EventHandlerType,
                    Expression.Call(
                        instance: Expression.Constant(this),
                        method: typeof(EventSubscriber).GetMethod(nameof(QueueInteropEvent), BindingFlags.NonPublic | BindingFlags.Instance),
                        arg0: Expression.Constant(ev.Name),
                        arg1: Expression.NewArrayInit(typeof(object), eventParams.Select(p => Expression.Convert(p, typeof(object))))),
                    eventParams);
                var eventHandler = eventHandlerLambda.Compile();
                ev.AddEventHandler(source, eventHandler);
                _eventHandlers.Add(eventHandler);
            }
        }

        public void Dispose()
        {
            var events = _source.GetType().GetEvents();
            for (int e = 0; e < events.Length; ++e)
                events[e].RemoveEventHandler(_source, _eventHandlers[e]);
        }

        void QueueInteropEvent(string name, object[] parameters)
        {
            var interopEvent = new InteropEvent { Name = name, Parameters = parameters };
            if (!_completion.TrySetResult(interopEvent))
                _interopEvents.Enqueue(interopEvent);
        }

        /// <summary>
        /// Waits until an event is raised, or returns immediately if a queued event is available.
        /// </summary>
        /// <exception cref="TaskCanceledException"><see cref="CancelWait"/> was called while waiting.</exception>
        public InteropEvent WaitForEvent()
        {
            if (_interopEvents.TryDequeue(out var interopEvent)) return interopEvent;
            _completion = new TaskCompletionSource<InteropEvent>();
            return _completion.Task.Result;
        }

        public void CancelWait()
        {
            _completion.TrySetCanceled();
        }
    }

    public class InteropEvent
    {
        public string Name { get; internal set; }
        public object[] Parameters { get; internal set; }
    }
}
