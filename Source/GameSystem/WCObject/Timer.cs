using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Source.GameSystem.WCObject
{
#pragma warning disable CS0824 // Constructor is marked external
#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    /// <summary>
    /// Warcraft timer wrapper
    /// </summary>
    /// @CSharpLua.Ignore
    public sealed class Timer : IDisposable
    {
        /// <summary>
        /// Create a timer
        /// </summary>
        /// @CSharpLua.Template = "CreateTimer()"
        public Timer() { }

        /// <summary>
        /// Destroy this timer
        /// </summary>
        /// @CSharpLua.Template = "DestroyTimer({this})
        public void Dispose() { }

        /// <summary>
        /// Start timer that will call <paramref name="handlerMethod"/> once it expire
        /// </summary>
        /// <param name="timeout">Delay in seconds</param>
        /// <param name="periodic">Repeat after expire?</param>
        /// <param name="handlerMethod">The method at which will be called once timer expired</param>
        /// @CSharpLua.Template = "TimerStart({this}, {0}, {2}, {1})"
        public void Start(float timeout, Action handlerMethod, bool periodic = false) { }

        /// <summary>
        /// Pause this timer
        /// </summary>
        /// @CSharpLua.Template = "PauseTimer({this})"
        public void Pause() { }

        /// <summary>
        /// Resume the timer back to it previous timeout
        /// </summary>
        /// @CSharpLua.Template = "ResumeTimer({this})"
        public void Resume() { }

        /// <summary>
        /// The current elapsed time on this timer
        /// </summary>
        /// @CSharpLua.Template = "TimerGetElapsed({this})"
        public readonly float Elapsed;

        /// <summary>
        /// The timeout set on this timer
        /// </summary>
        /// @CSharpLua.Template = "TimerGetTimeout({this})"
        public readonly float Timeout;

        /// <summary>
        /// Get the remaining timeout
        /// </summary>
        /// @CSharpLua.Template = "TimerGetRemaining({this})"
        public readonly float Remaining;
    }


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore S4200 // Native methods should be wrapped
#pragma warning restore CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning restore CS0824 // Constructor is marked external
}
