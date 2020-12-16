#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// The command management APIs let plugins interact with the command-system in X-Plane, the abstraction behind keyboard presses and joystick buttons.
    /// This API lets you create new commands and modify the behavior (or get notification) of existing ones.
    /// </summary>
    public sealed class Command
    {
        private static readonly Dictionary<CommandRef, Command> _commandCache = new Dictionary<CommandRef, Command>();

        private RefStructEventHandler<Command, CommandBeforeExecuteEventArgs>? _beforeExecute;
        private InStructEventHandler<Command, CommandAfterExecuteEventArgs>? _afterExecute;

        private readonly CommandRef _commandRef;

        private Command(CommandRef commandRef)
        {
            _commandRef = commandRef;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Command? FromRef(CommandRef commandRef)
        {
            if (commandRef == default)
                return null;

            if (!_commandCache.TryGetValue(commandRef, out var command))
            {
                _commandCache[commandRef] = command = new Command(commandRef);
            }

            return command;
        }

        /// <summary>
        /// Gets the command reference.
        /// </summary>
        public CommandRef CommandRef => _commandRef;

        /// <summary>
        /// Looks up a command by name.
        /// </summary>
        public static Command? Find(in ReadOnlySpan<char> name)
        {
            var commandRef = UtilitiesAPI.FindCommand(name);
            return FromRef(commandRef);
        }

        /// <summary>
        /// Creates a new command for a given string.
        /// If the command already exists, the existing command reference is returned.
        /// The description may appear in user interface contexts, such as the joystick configuration screen.
        /// </summary>
        public static Command? Create(in ReadOnlySpan<char> name, in ReadOnlySpan<char> description)
        {
            var commandRef = UtilitiesAPI.CreateCommand(name, description);
            return FromRef(commandRef);
        }

        /// <summary>
        /// This executes a given command momentarily, that is, the command begins and ends immediately.
        /// This is the equivalent of calling <see cref="Begin"/> and <see cref="End"/> back ot back.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Once() => UtilitiesAPI.CommandOnce(_commandRef);

        /// <summary>
        /// <para>Starts the execution of a command.</para>
        /// <para>The command is "held down" until <see cref="End"/> is called.</para>
        /// <para>You must balance each <see cref="Begin"/> call with an <see cref="End"/> call.</para> 
        /// </summary>
        /// <seealso cref="BeginScope"/>
        /// <seealso cref="BeginScopeAllocationFree"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Begin() => UtilitiesAPI.CommandBegin(_commandRef);

        /// <summary>
        /// <para>Ends the execution of a given command that was started with <see cref="Begin"/>.</para>
        /// <para>You must not issue XPLMCommandEnd for a command you did not begin.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void End() => UtilitiesAPI.CommandEnd(_commandRef);

        /// <summary>
        /// <para>Starts the execution of a command.</para>
        /// <para>The command is "held down" until the <see cref="IDisposable.Dispose()"/> of the returned object is called.</para>
        /// </summary>
        /// <seealso cref="BeginScopeAllocationFree"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IDisposable BeginScope() => new CommandDisposableScope(_commandRef);

        /// <summary>
        /// <para>Starts the execution of a command.</para>
        /// <para>The command is "held down" until the <see cref="CommandExecutionScope.Dispose()"/> of the returned object is called.</para>
        /// </summary>
        /// <seealso cref="BeginScope"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CommandExecutionScope BeginScopeAllocationFree() => new CommandExecutionScope(_commandRef);

        /// <summary>
        /// Raised before X-Plane executes the command.
        /// </summary>
        /// <remarks>
        /// Set <see cref="CommandBeforeExecuteEventArgs.Handled"/> to <see langword="true"/>
        /// to prevent the command from being handled by X-Plane and other plugins.
        /// </remarks>
        public unsafe event RefStructEventHandler<Command, CommandBeforeExecuteEventArgs> BeforeExecute
        {
            add
            {
                if (value == null)
                    return;

                bool mustRegister = _beforeExecute == null;
                _beforeExecute += value;
                if (mustRegister)
                {
                    UtilitiesAPI.RegisterCommandHandler(_commandRef, &BeforeExecuteCallback, 1, null);
                }
            }
            remove
            {
                if (_beforeExecute != null)
                {
                    _beforeExecute -= value;
                    if (_beforeExecute == null)
                    {
                        UtilitiesAPI.UnregisterCommandHandler(_commandRef, &BeforeExecuteCallback, 1, null);
                    }
                }
            }
        }

        /// <summary>
        /// Raised after X-Plane has executed the command.
        /// </summary>
        public unsafe event InStructEventHandler<Command, CommandAfterExecuteEventArgs> AfterExecute
        {
            add
            {
                if (value == null)
                    return;

                bool mustRegister = _afterExecute == null;
                _afterExecute += value;
                if (mustRegister)
                {
                    UtilitiesAPI.RegisterCommandHandler(_commandRef, &AfterExecuteCallback, 0, null);
                }
            }
            remove
            {
                if (_afterExecute != null)
                {
                    _afterExecute -= value;
                    if (_afterExecute == null)
                    {
                        UtilitiesAPI.UnregisterCommandHandler(_commandRef, &AfterExecuteCallback, 0, null);
                    }
                }
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static unsafe int BeforeExecuteCallback(CommandRef incommand, CommandPhase inphase, void* inrefcon)
        {
            if (_commandCache.TryGetValue(incommand, out var command) && command != null)
            {
                var args = new CommandBeforeExecuteEventArgs(inphase);
                command._beforeExecute?.Invoke(command, ref args);
                return args.Handled ? 0 : 1;
            }

            return 1;
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static unsafe int AfterExecuteCallback(CommandRef incommand, CommandPhase inphase, void* inrefcon)
        {
            if (_commandCache.TryGetValue(incommand, out var command) && command != null)
            {
                var args = new CommandAfterExecuteEventArgs(inphase);
                command._afterExecute?.Invoke(command, in args);
            }

            return 1;
        }

        private class CommandDisposableScope : IDisposable
        {
            private CommandRef _commandRef;

            public CommandDisposableScope(CommandRef commandRef)
            {
                _commandRef = commandRef;
                UtilitiesAPI.CommandBegin(_commandRef);
            }

            public void Dispose()
            {
                if (_commandRef != default)
                {
                    UtilitiesAPI.CommandEnd(_commandRef);
                    _commandRef = default;
                }
            }
        }
    }
}
