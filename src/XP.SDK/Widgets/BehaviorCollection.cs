#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XP.SDK.Widgets
{
    /// <summary>
    /// Contains the list of <see cref="Behavior"/>s attached to the widget.
    /// </summary>
    public class BehaviorCollection : IEnumerable<Behavior>
    {
        private readonly Widget _widget;
        private readonly List<Behavior> _behaviors = new List<Behavior>(4);

        internal BehaviorCollection(Widget widget)
        {
            _widget = widget;
        }

        /// <summary>
        /// Adds a behavior.
        /// </summary>
        /// <param name="behavior">The behavior to add.</param>
        /// <returns>This <see cref="BehaviorCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="behavior"/> is <see langword="null"/>.</exception>
        public BehaviorCollection Add(Behavior behavior)
        {
            if (behavior == null) 
                throw new ArgumentNullException(nameof(behavior));

            _behaviors.Add(behavior);
            _widget.AddHook(behavior.WidgetFuncCallback);
            return this;
        }

        /// <summary>
        /// Adds a behavior.
        /// </summary>
        /// <typeparam name="T">The type of the behavior to add.</typeparam>
        /// <returns>This <see cref="BehaviorCollection"/>.</returns>
        public BehaviorCollection Add<T>() where T : Behavior, new()
        {
            return Add(new T());
        }

        /// <summary>
        /// Returns a behavior of type <typeparamref name="T"/> if it exists or creates and adds a new instance otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the behavior to get or add.</typeparam>
        public T GetOrAdd<T>() where T : Behavior, new()
        {
            var behavior = _behaviors.OfType<T>().FirstOrDefault();
            if (behavior == null)
            {
                behavior = new T();
                Add(behavior);
            }

            return behavior;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Behavior> GetEnumerator() => _behaviors.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
