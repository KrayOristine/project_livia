// ------------------------------------------------------------------------------
// <copyright file="Trigger.cs" company="Kray Oristine">
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using static War3Api.Common;

namespace Source.GameSystem.W3OOP
{
#pragma warning disable CS0824 // Constructor is marked external
#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    /// <summary>
    /// Warcraft Trigger Wrapper
    /// </summary>
    /// @CSharpLua.Ignore
    public sealed class Trigger : IDisposable
    {
        #region Trigger Management

        /// <summary>
        /// Create trigger
        /// </summary>
        /// @CSharpLua.Template = "CreateTrigger()"
        public extern Trigger();

        /// <summary>
        /// Destroy this trigger
        /// </summary>
        /// @CSharpLua.Template = "DestroyTrigger({this})"
        public extern void Dispose();

        /// <summary>
        /// Reset <see cref="EvalCount"/> and <see cref="ExecCount"/> count of this trigger
        /// </summary>
        /// @CSharpLua.Template = "ResetTrigger({this})"
        public extern void Reset();

        /// <summary>
        /// Disable this trigger from being executed from event it registered but still allow <see cref="Execute"/> and <see cref="Evaluate"/> to work
        /// </summary>
        /// @CSharpLua.Template = "DisableTrigger({this})"
        public extern void Disable();

        /// <summary>
        /// Enable this trigger to being able to fired from registered event
        /// </summary>
        /// @CSharpLua.Template = "EnableTrigger({this})"
        public extern void Enable();

        /// <summary>
        /// Get the current status of the trigger
        /// </summary>
        /// @CSharpLua.Template = "IsTriggerEnabled({this})"
        public readonly bool IsEnabled;

        /// <summary>
        /// Get the attribute flag this flag indicate that this trigger will wait for sleep action
        /// </summary>
        /// @CSharpLua.Template = "IsTriggerWaitOnSleeps({this})"
        public readonly bool IsAllowedToWait;

        /// <summary>
        /// Allow this trigger to wait for sleep action?
        /// </summary>
        /// <param name="flag"></param>
        /// @CSharpLua.Template = "TriggerWaitOnSleeps({this}, {0})"
        public extern void WaitOnSleeps(bool flag);

        /// <summary>
        /// Makes a trigger execution sleep for a given duration. The thread will yield so other threads can do their work.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///      <item>
        ///      This works only in a trigger action execution context, not in trigger conditions nor for example in timer functions or `ForGroup` functions.However, it also works in `<see cref="Execute"/>` contexts, even if the `<see cref="Execute"/>` call is not in a trigger action execution context.
        ///      </item>
        ///      <item>
        ///      This has many implications, see other trigger-related natives.
        ///      </item>
        ///      <item>
        ///      This ticks while the game was paused with the `<see cref="PauseGame(bool)"/>` native, unlike timers.
        ///      </item>
        ///      <item>
        ///      This does not tick while the game was paused by the user, neither in single-player nor in multi-player. (But the Trigger Editor of the World Editor denotes it as a real-time wait. Is this a bug?)
        ///      </item>
        ///      <item>
        ///      This ticks independently from game speed, i.e., in Fast mode, it will be about the same as a game time, in Normal mode, it will be about 25% faster than game time and in Slow mode, it will be about 67% faster than game time, see table below.<br/>
        ///      Game speed | TSA speed (%) | game speed (%)<br/>
        ///      ------------|---------------|---------------<br/>
        ///      Fast        | 100%          | 100%<br/>
        ///      Normal      | 100%          | 80%<br/>
        ///      Slow        | 100%          | 60%<br/><br/>
        ///      Example elapsed game time after TSA with timeout = 30:<br/>
        ///      Game speed | game time elapsed<br/>
        ///      -----------|------------------<br/>
        ///      Fast       | 30s<br/>
        ///      Normal     | 24s<br/>
        ///      Slow       | 18s
        ///      </item>
        /// </list>
        /// </remarks>
        /// @CSharpLua.Template = "TriggerSleepAction({this}, {0})"
        public extern void Sleep(float timeout);

        /// <summary>
        /// Get how many times this trigger has evaluated.
        /// </summary>
        /// <returns>Integer count describe how often this trigger evaluated</returns>
        /// @CSharpLua.Template = "GetTriggerEvalCount({this})"
        public extern int EvalCount();

        /// <summary>
        /// Get how many times this trigger has executed.
        /// </summary>
        /// <returns>Integer count describe how often this trigger executed</returns>
        /// @CSharpLua.Template = "GetTriggerExecCount({this})"
        public extern int ExecCount();

        /// <summary>
        /// Execute this trigger, ignoring it condition
        /// </summary>
        /// @CSharpLua.Template = "TriggerExecute({this})"
        public extern void Execute();

        /// <summary>
        /// Evaluates all functions that were added to the trigger via <see cref="AddCondition(Func{bool})"/> or <see cref="AddCondition(boolexpr)"/>.<br/>
        /// All return-values from all added condition-functions are anded together as the final return-value.
        /// </summary>
        /// <returns>The boolean value of the return value from the condition-function.<br/>
        /// If the condition-functions return 0/0.0/null, then it will return false. Note that an empty string "" would return true.</returns>
        /// <remarks>All functions added to condition are run in the order they were added.</remarks>
        /// @CSharpLua.Template = "TriggerEvaluate({this})"
        public extern bool Evaluate();

        /// <summary>
        /// Execute this trigger without ignoring it condition
        /// </summary>
        /// @CSharpLua.Template = "if(TriggerEvaluate({this})then;TriggerExecute({this});end"
        public extern void ConditionalExecute();

        /// <summary>
        /// Add a method as a condition to this trigger
        /// </summary>
        /// <returns>A useless warcraft object that has nothing to do with it</returns>
        /// @CSharpLua.Template = "TriggerAddCondition({this}, Condition({0}))"
        public extern triggercondition AddCondition(Func<bool> whichMethod);

        /// <summary>
        /// Add a boolexpr as a condition to this trigger
        /// </summary>
        /// <returns>A useless warcraft object that has nothing to do with it</returns>
        /// @CSharpLua.Template = "TriggerAddCondition({this}, {0})"
        public extern triggercondition AddCondition(boolexpr whichCondition);

        /// <summary>
        /// Remove a specific condition from this trigger
        /// </summary>
        /// <param name="condition"></param>
        /// @CSharpLua.Template = "TriggerRemoveCondition({this}, {0})"
        public extern void RemoveCondition(triggercondition condition);

        /// <summary>
        /// Clear and remove all condition in this trigger
        /// </summary>
        /// @CSharpLua.Template = "TriggerClearCondition({this})"
        public extern void ClearCondition();

        /// <summary>
        /// Add a method as a action to this trigger that will be called when this trigger is fired<br/>
        /// from it registered event or through <see cref="Execute"/>.<br/><br/>
        /// @NOTE:
        /// <list type="number">
        /// <item>
        /// New actions added to the trigger during the execution of the actions won't be subject for execution for this run.
        /// </item>
        /// <item>
        /// More than one action can be added to the trigger. The actions run in the order they were added.
        /// </item>
        /// <item>
        /// If an action execution crashes, subsequent actions will be unaffected and still be called.
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="whichMethod"></param>
        /// <returns>An almost useless warcraft object that has nothing to do with it except for removing it from this trigger via <see cref="RemoveAction"/></returns>
        /// @CSharpLua.Template = "TriggerAddAction({this}, {0})"
        public extern triggeraction AddAction(Action whichMethod);

        /// <summary>
        /// Remove an action from this trigger
        /// </summary>
        /// <param name="action"></param>
        /// @CSharpLua.Template = "TriggerRemoveAction({this}, {0})"
        public extern void RemoveAction(triggeraction action);

        /// <summary>
        /// Clear and remove all action from this trigger
        /// </summary>
        /// @CSharpLua.Template = "TriggerClearAction({this})"
        public extern void ClearAction();

        /// <summary>
        /// Get the currently triggering trigger
        /// </summary>
        /// <returns></returns>
        /// @CSharpLua.Template = "GetTriggeringTrigger()"
        public extern Trigger GetTriggering();

        #endregion

        #region Trigger Event Registration

        /// <summary>
        /// Upon the <paramref name="varName"/> variable value changed to something based on higher or lower something based on <paramref name="opCode"/> and <paramref name="limitValue"/><br/>
        /// this trigger will be fired
        /// </summary>
        /// <param name="varName">Target variable to be monitored</param>
        /// <param name="opCode"></param>
        /// <param name="limitValue"></param>
        /// <returns>Useless warcraft object</returns>
        /// @CSharpLua.Template = "TriggerRegisterVariableEvent({this}, {0}, {1}, {2})"
        public extern @event OnVariableEvent(string varName, limitop opCode, float limitValue);

        /// <summary>
        /// Every <paramref name="timeout"/> second(s), this trigger will be fired and then stopped or don't if <paramref name="periodic"/> is true
        /// </summary>
        /// <param name="timeout">Time in seconds that this trigger will ran every moment it passed the timeout</param>
        /// <param name="periodic">Allow running multiple times?</param>
        /// <returns>Useless warcraft object</returns>
        /// <remarks>In 1.32.X the timeout is limited to 0.01 (if you set a lower value is still always run as same as 0.01)</remarks>
        /// @CSharpLua.Template = "TriggerRegisterTimerEvent({this}, {0}, {1})"
        public extern @event OnTimerEvent(float timeout, bool periodic = false);

        /// <summary>
        /// Attach <paramref name="tmr"/> timer to this trigger, making this trigger fired every time <paramref name="tmr"/> expire.
        /// </summary>
        /// <param name="tmr"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "TriggerRegisterTimerExpireEvent({this}, {0})"
        public extern @event OnTimerExpire(Timer tmr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="whichPlayer">The player that receive the sync data</param>
        /// <param name="prefix">The prefix that will trigger this trigger to run</param>
        /// <param name="fromServer"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzTriggerRegisterPlayerSyncEvent({this}, {0}, {1}, {2})"
        public extern @event OnSyncEvent(player whichPlayer, string prefix, bool fromServer = false);

        #endregion
    }


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore S4200 // Native methods should be wrapped
#pragma warning restore CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning restore CS0824 // Constructor is marked external
}
