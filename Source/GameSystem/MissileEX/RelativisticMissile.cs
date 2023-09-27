// ------------------------------------------------------------------------------
// <copyright file="RelativisticMissile.cs" company="Kray Oristine">
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
using System.Linq;
using WCSharp.Events;
using WCSharp.Shared;
using static War3Api.Common;
using Source.Shared;

namespace Source.GameSystem.MissileEX
{
    /// <summary>
    /// The only missile that use relativistic from chopinski, with all added helper method
    /// </summary>
    public abstract class RelativisticMissile : IPeriodicDisposableAction
    {
        // Doesn't fold multiple constant references properly unless you do this
        public const float ROTATION_SECONDS_TO_RADIANS = PeriodicEvents.SYSTEM_INTERVAL * Util.PI * 2;
        public const int SWEET_SPOT = 500;
        /// <summary>
        /// Dont do stupid shit with this
        /// </summary>
        protected static readonly group _enumGroup = CreateGroup();
        /// <summary>
        /// Should not be touched by any inheritance
        /// </summary>
        private static float _dilation;

        /// <summary>
        /// Whether the missile is active. This is automatically set to false prior to calling <see cref="OnImpact"/>.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// You know what is mean already, do you?
        /// </summary>
        public bool Paused { get; private set; }

        /// <summary>
        /// The caster, if it exists.
        /// </summary>
        public unit Caster { get; set; }

        /// <summary>
        /// The owner of the caster. Automatically set on launch.
        /// <para>Does NOT update automatically! If this is desired, you can use <see cref="MissileSystem.RegisterForOwnershipChanges"/>.</para>
        /// </summary>
        public player CastingPlayer { get; set; }

        /// <summary>
        /// The X coordinate from which the missile was fired.
        /// </summary>
        public float CasterX { get; set; }

        /// <summary>
        /// The Y coordinate from which the missile was fired.
        /// </summary>
        public float CasterY { get; set; }

        /// <summary>
        /// The Z coordinate from which the missile was fired.
        /// <para>By default, this is equal to UnitFlyHeight + <see cref="CasterLaunchZ"/> + GetLocationZ.</para>
        /// </summary>
        public virtual float CasterZ
        {
            get => this.casterZ + WarEX.GetLocZ(CasterX, CasterY);
            set => this.casterZ = value - WarEX.GetLocZ(CasterX, CasterY);
        }

        /// <summary>
        /// Use this to adjust the origin height of the missile when firing, as it will by default fire from the ground (plus unit flying height).
        /// </summary>
        public float CasterLaunchZ { get; set; }

        /// <summary>
        /// The target, if it exists. If the target dies during travel, this is set to null.
        /// </summary>
        public unit? Target { get; set; }

        /// <summary>
        /// The owner of the target. Automatically set on launch.
        /// <para>Does NOT update automatically! If this is desired, you can use <see cref="MissileSystem.RegisterForOwnershipChanges"/>.</para>
        /// </summary>
        public player TargetPlayer { get; set; }

        /// <summary>
        /// The X coordinate that this missile is moving towards. While <see cref="Target"/> is alive, this will automatically be updated.
        /// </summary>
        public float TargetX { get; set; }

        /// <summary>
        /// The Y coordinate that this missile is moving towards. While <see cref="Target"/> is alive, this will automatically be updated.
        /// </summary>
        public float TargetY { get; set; }

        /// <summary>
        /// The Z coordinate that this missile is moving towards. While <see cref="Target"/> is alive, this will automatically be updated.
        /// <para>By default, this is equal to UnitFlyHeight + <see cref="TargetImpactZ"/> + GetLocationZ.</para>
        /// </summary>
        public virtual float TargetZ
        {
            get => this.targetZ + WarEX.GetLocZ(TargetX, TargetY);
            set => this.targetZ = value - WarEX.GetLocZ(TargetX, TargetY);
        }

        /// <summary>
        /// Use this to adjust the target height of the missile, as it will by default aim towards the ground (plus unit flying height).
        /// </summary>
        public float TargetImpactZ { get; set; }

        /// <summary>
        /// The current X position of the missile.
        /// <para>Depending on the type of missile, MissileX sets may be ignored.</para>
        /// </summary>
        public float MissileX { get; protected set; }

        /// <summary>
        /// The current Y position of the missile.
        /// <para>Depending on the type of missile, MissileY sets may be ignored.</para>
        /// </summary>
        public float MissileY { get; protected set; }

        /// <summary>
        /// The current Z position of the missile.
        /// <para>Depending on the type of missile, MissileZ sets may be ignored.</para>
        /// </summary>
        public virtual float MissileZ
        {
            get => this.missileZ + WarEX.GetLocZ(MissileX, MissileY);
            set => this.missileZ = value - WarEX.GetLocZ(MissileX, MissileY);
        }

        /// <summary>
        /// The internal speed field. Defined in units per <see cref="PeriodicEvents.SYSTEM_INTERVAL"/>.
        /// </summary>
        protected float speed;

        /// <summary>
        /// The speed of the missile. Defined in units per second.
        /// </summary>
        public virtual float Speed
        {
            get => this.speed / PeriodicEvents.SYSTEM_INTERVAL;
            set => this.speed = value * PeriodicEvents.SYSTEM_INTERVAL;
        }

        /// <summary>
        /// By default impact triggers when the distance to the target is less than the missile's speed per tick.
        /// <para>Use this to increase that distance by a static number (default 0).</para>
        /// </summary>
        public float ImpactLeeway { get; set; }

        /// <summary>
        /// <para>Keeps track of all units collided with thus far.</para>
        /// <para>Null if <see cref="CollisionRadius"/> has never been set to a value greater than 0.</para>
        /// <para>Removing a unit from this means <see cref="OnCollision(unit)"/> will trigger for that unit once more.</para>
        /// </summary>
        public HashSet<unit> TargetsHit { get; private set; }

        protected float collisionRadius;

        /// <summary>
        /// The collision radius of the missile which is used to trigger <see cref="OnCollision(unit)"/>.
        /// <para>Leave at default (0) to disable. Will ignore values below 0.</para>
        /// </summary>
        public float CollisionRadius
        {
            get => this.collisionRadius;
            set
            {
                this.collisionRadius = Math.Max(0, value);
                if (this.collisionRadius > 0 && TargetsHit == null)
                {
                    TargetsHit = new HashSet<unit>();
                }
            }
        }

        /// <summary>
        /// The interval at which the missile will call <see cref="OnPeriodic"/>. Leave at default (0) to disable.
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// The time left until the next call to <see cref="OnPeriodic"/>.
        /// </summary>
        public float IntervalLeft { get; set; }

        /// <summary>
        /// The internal spin period field. Defined in radians per <see cref="PeriodicEvents.SYSTEM_INTERVAL"/>.
        /// </summary>
        protected float spinPeriod;

        /// <summary>
        /// The amount of time it takes for the projectile to spin once during flight in seconds.
        /// <para>Use negative values to go clockwise.</para>
        /// </summary>
        public float SpinPeriod
        {
            get => this.spinPeriod == 0 ? 0 : ROTATION_SECONDS_TO_RADIANS / this.spinPeriod;
            set => this.spinPeriod = value == 0 ? 0 : ROTATION_SECONDS_TO_RADIANS / value;
        }

        /// <summary>
        /// The yaw of the projectile. Defined in radians.
        /// <para>Depending on the type of missile, yaw sets may be ignored.</para>
        /// </summary>
        protected float yaw;

        /// <summary>
        /// The yaw of the projectile. Defined in degrees.
        /// <para>Depending on the type of missile, yaw sets may be ignored.</para>
        /// </summary>
        public float Yaw
        {
            get => this.yaw * Util.RAD2DEG;
            set => this.yaw = value * Util.DEG2RAD;
        }

        /// <summary>
        /// The pitch of the projectile. Defined in radians.
        /// <para>Depending on the type of missile, pitch sets may be ignored.</para>
        /// </summary>
        protected float pitch;

        /// <summary>
        /// The pitch of the projectile. Defined in degrees.
        /// <para>Depending on the type of missile, pitch sets may be ignored.</para>
        /// </summary>
        public float Pitch
        {
            get => this.pitch * Util.RAD2DEG;
            set => this.pitch = value * Util.DEG2RAD;
        }

        /// <summary>
        /// The roll of the projectile. Defined in radians.
        /// <para>Depending on the type of missile, roll sets may be ignored.</para>
        /// </summary>
        protected float roll;

        /// <summary>
        /// The roll of the projectile. Defined in degrees.
        /// <para>Depending on the type of missile, roll sets may be ignored.</para>
        /// </summary>
        public float Roll
        {
            get => this.roll * Util.RAD2DEG;
            set => this.roll = value * Util.DEG2RAD;
        }

        /// <summary>
        /// Identical to <see cref="Yaw"/>.
        /// </summary>
        public float CurrentAngle
        {
            get => this.yaw * Util.RAD2DEG;
            set => this.yaw = value * Util.DEG2RAD;
        }

        /// <summary>
        /// Internal effect string.
        /// </summary>
        protected string effectString;

        /// <summary>
        /// The effect string of the missile. If empty/null, the missile will be invisible.
        /// <para>If changed mid-flight, automatically modifies the missile.</para>
        /// </summary>
        public string EffectString
        {
            get => this.effectString;
            set
            {
                if (this.effectString != value)
                {
                    if (Active)
                    {
                        if (Effect != null)
                        {
                            DestroyEffect(Effect);
                        }

                        if (!string.IsNullOrEmpty(value))
                        {
                            Effect = AddSpecialEffect(value, MissileX, MissileY);
                            BlzSetSpecialEffectZ(Effect, MissileZ);
                            BlzSetSpecialEffectOrientation(Effect, this.yaw, this.pitch, this.roll);
                            if (this.effectScale != 1)
                            {
                                BlzSetSpecialEffectScale(Effect, this.effectScale);
                            }
                        }
                    }

                    this.effectString = value;
                }
            }
        }

        /// <summary>
        /// Internal effect scale. Used only when there is no effect present yet.
        /// </summary>
        protected float effectScale = 1.0f;

        /// <summary>
        /// The effect scale of the missile.
        /// <para>If changed mid-flight, automatically modifies the missile.</para>
        /// </summary>
        public float EffectScale
        {
            get => Effect == null ? this.effectScale : BlzGetSpecialEffectScale(Effect);
            set
            {
                if (Effect != null)
                {
                    BlzSetSpecialEffectScale(Effect, value);
                }
                this.effectScale = value;
            }
        }

        /// <summary>
        /// The effect being used by the missile. Creation of the effect should be done by setting <see cref="EffectString"/>, not by setting this property.
        /// </summary>
        public effect Effect { get; protected set; }

        /// <summary>
        /// The Z of the missile without accounting for terrain.
        /// </summary>
        protected float missileZ;

        /// <summary>
        /// The Z of the caster without accounting for terrain.
        /// </summary>
        protected float casterZ;

        /// <summary>
        /// The Z of the target without accounting for terrain.
        /// </summary>
        protected float targetZ;

        private RelativisticMissile(unit caster)
        {
            Caster = caster;
            CastingPlayer = GetOwningPlayer(caster);
            CasterX = GetUnitX(caster);
            CasterY = GetUnitY(caster);
            this.casterZ = GetUnitFlyHeight(caster);
        }

        private RelativisticMissile(player castingPlayer, float casterX, float casterY)
        {
            CastingPlayer = castingPlayer;
            CasterX = casterX;
            CasterY = casterY;
        }

        /// <summary>
        /// Creates a new missile instance with the given parameters.
        /// <para>Will automatically set <see cref="CastingPlayer"/>, <see cref="CasterX"/>, <see cref="CasterY"/>,
        /// <see cref="TargetPlayer"/>, <see cref="TargetX"/> and <see cref="TargetY"/>.</para>
        /// </summary>
        public RelativisticMissile(unit caster, unit target) : this(caster)
        {
            Target = target;
            TargetPlayer = GetOwningPlayer(target);
            TargetX = GetUnitX(target);
            TargetY = GetUnitY(target);
            this.targetZ = GetUnitFlyHeight(target);
        }

        /// <summary>
        /// Creates a new missile instance with the given parameters.
        /// <para>Will automatically set <see cref="CastingPlayer"/>, <see cref="CasterX"/> and <see cref="CasterY"/>.</para>
        /// </summary>
        public RelativisticMissile(unit caster, float targetX, float targetY) : this(caster)
        {
            TargetX = targetX;
            TargetY = targetY;
        }

        /// <summary>
        /// Creates a new missile instance with the given parameters.
        /// <para>Will automatically set <see cref="TargetPlayer"/>, <see cref="TargetX"/> and <see cref="TargetY"/>.</para>
        /// </summary>
        public RelativisticMissile(player castingPlayer, float casterX, float casterY, unit target) : this(castingPlayer, casterX, casterY)
        {
            Target = target;
            TargetPlayer = GetOwningPlayer(target);
            TargetX = GetUnitX(target);
            TargetY = GetUnitY(target);
            this.targetZ = GetUnitFlyHeight(target);
        }

        /// <summary>
        /// Creates a new missile instance with the given parameters.
        /// </summary>
        public RelativisticMissile(player castingPlayer, float casterX, float casterY, float targetX, float targetY) : this(castingPlayer, casterX, casterY)
        {
            TargetX = targetX;
            TargetY = targetY;
        }

        /// <summary>
        /// Called by the system. Do not call yourself.
        /// </summary>
        public void Launch()
        {
            if (Active) return;
            Active = true;
            MissileSystem.Add(this);

            UpdateDilation();

        }

        /// <summary>
        /// Called by the system. DO NOT CALL BY YOUR FUCKIN SELF
        /// </summary>
        public virtual void OnLaunch() { }

        public static void UpdateDilation()
        {
            if (MissileSystem.Active + 1 > SWEET_SPOT) _dilation = (MissileSystem.Active + 1) / (float)SWEET_SPOT;
            else _dilation = 1f;
        }

        /// <summary>
        /// Called by the system. Do not call yourself.
        /// </summary>
        public virtual void Action()
        {
        }

        /// <summary>
        /// <para>Call this to check if this missile is collided with other missile</para>
        /// <para>Should not call if <see cref="collisionRadius"/> is 0</para>
        /// <para>This is not called by the system, user can call this on <see cref="OnPeriodic"/> if they want missile collision to work</para>
        /// This is the built-in helper function for user
        /// </summary>
        public void CheckMissileCollision()
        {
            for (int i = 0; i < MissileSystem.Missiles.Count; i++)
            {
                var ms = MissileSystem.Missiles[i];
                if (ms == this) continue;
                var dx = ms.MissileX - MissileX;
                var dy = ms.MissileY - MissileY;
                if (Shared.Lua.Math.Sqrt(dx * dx + dy * dy) <= collisionRadius) OnMissile(ms);
            }
        }

        public void Pause(bool flag)
        {
            Paused = flag;
            if (!Paused)
            {
                Active = true;
                MissileSystem.Add(this);

                UpdateDilation();
            }
        }

        /// <summary>
        /// Should be called when the missile would exit world bounds.
        /// </summary>
        protected void ExitWorldBounds()
        {
            if (Effect != null)
            {
                MissileX = BlzGetLocalSpecialEffectX(Effect);
                MissileY = BlzGetLocalSpecialEffectY(Effect);
                MissileZ = BlzGetLocalSpecialEffectZ(Effect);
            }

            if (Interval > 0)
            {
                RunInterval();
            }
            if (this.collisionRadius > 0)
            {
                RunCollisions();
            }

            Active = false;
            OnImpact();
        }

        /// <summary>
        /// Runs the Interval related code. Do not call if <see cref="Interval"/> is 0.
        /// </summary>
        protected void RunInterval()
        {
            IntervalLeft -= PeriodicEvents.SYSTEM_INTERVAL;
            if (IntervalLeft <= 0)
            {
                IntervalLeft += Interval;
                OnPeriodic();
            }
        }

        /// <summary>
        /// Runs the Collision related code. Do not call if <see cref="CollisionRadius"/> is 0.
        /// </summary>
        protected void RunCollisions()
        {
            GroupClear(_enumGroup);
            WarEX.EXGroupEnumUnitInRange(_enumGroup, MissileX, MissileY, collisionRadius);
            if (BlzGroupGetSize(_enumGroup) < 0) return;
            var i = 0;
            var u = BlzGroupUnitAt(_enumGroup, i);
            while (true){
                if (u == null) break;

                if (TargetsHit.Add(u)) OnCollision(u);
                u = BlzGroupUnitAt(_enumGroup, i++);
            }
        }

        /// <summary>
        /// Sets the missile position to that of the target and runs the interval, collision and impact code.
        /// </summary>
        protected void Impact()
        {
            MissileX = TargetX;
            MissileY = TargetY;
            this.missileZ = this.targetZ;
            BlzSetSpecialEffectPosition(Effect, MissileX, MissileY, MissileZ);
            BlzSetSpecialEffectPitch(Effect, 0);

            if (Interval > 0)
            {
                RunInterval();
            }
            if (this.collisionRadius > 0)
            {
                RunCollisions();
            }

            Active = false;
            OnImpact();
        }

        /// <summary>
        /// <para>Override this method if your missile has an effect that should trigger when colliding with another unit.</para>
        /// <para>For this to be active, <see cref="CollisionRadius"/> must be greater than 0.</para>
        /// <para>Note that there is no filter on this collision. This is called whenever it collides with anything not in <see cref="TargetsHit"/>.</para>
        /// <para>Before this method is called, the <paramref name="unit"/> is added to <see cref="TargetsHit"/>.</para>
        /// </summary>
        public virtual void OnCollision(unit unit)
        {
        }

        /// <summary>
        /// <para>Override this method if your missile has to do something when it paused!</para>
        /// </summary>
        public virtual void OnPause()
        {
        }

        /// <summary>
        /// <para>Override this method if your missile has to do something when it touched other missiles</para>
        /// <para>This method will only called if <see cref="CheckMissileCollision"/> is called</para>
        /// </summary>
        /// <param name="target">The missile that is collided with this missile</param>
        public virtual void OnMissile(RelativisticMissile target)
        {
        }

        /// <summary>
        /// Override this method if your missile has an effect that should trigger when it is destroyed for any reason.
        /// </summary>
        public virtual void OnDispose()
        {
        }

        /// <summary>
        /// Override this method if your missile has an impact effect.
        /// <para><see cref="Active"/> is automatically set to false prior to calling this method. If you do not want the missile to end, you need to set <see cref="Active"/> back to true.</para>
        /// </summary>
        public virtual void OnImpact()
        {
        }

        /// <summary>
        /// <para>Override this method if your missile has a periodic effect.</para>
        /// <para>For this to be active, <see cref="Interval"/> must be greater than 0.</para>
        /// </summary>
        public virtual void OnPeriodic()
        {
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            OnDispose();
            if (Effect != null)
            {
                DestroyEffect(Effect);
            }
        }
    }
}
