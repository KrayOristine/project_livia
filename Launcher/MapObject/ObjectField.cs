using War3Net.Build.Object;
using War3Net.Common.Extensions;

namespace Launcher.MapObject
{
    /// <summary>
    /// A helper class that make your life easier, this class provide all field in the world editor
    /// </summary>
    public static class ObjectField
    {
        private static readonly int UnitField = "u\0\0\0".FromRawcode();
        private static readonly int ItemField = "i\0\0\0".FromRawcode();
        private static readonly int DestructibleField = "b\0\0\0".FromRawcode();
        private static readonly int DoodadField = "d\0\0\0".FromRawcode();
        private static readonly int AbilityField = "u\0\0\0".FromRawcode();
        private static readonly int BuffField = "f\0\0\0".FromRawcode();
        private static readonly int UpgradeField = "g\0\0\0".FromRawcode();

        /// <summary>
        /// Convert a general field id into a unit field that is used for unit modification
        /// </summary>
        /// <param name="fieldId">Valid general field from <see cref="Abilities"/> or <see cref="Art"/>, etc.</param>
        /// <returns>A usable unit field for object editing</returns>
        public static int ConvertFieldForUnit(int fieldId)
        {
            return UnitField + fieldId;
        }

        public static int ConvertFieldForItem(int fieldId)
        {
            return ItemField + fieldId;
        }

        public static int ConvertFieldForDestructible(int fieldId)
        {
            return DestructibleField + fieldId;
        }

        public static int ConvertFieldForDoodad(int fieldId)
        {
            return DoodadField + fieldId;
        }

        public static int ConvertFieldForAbility(int fieldId)
        {
            return AbilityField + fieldId;
        }

        public static int ConvertFieldForBuff(int fieldId)
        {
            return BuffField + fieldId;
        }

        public static int ConvertFieldForUpgrade(int fieldId)
        {
            return UpgradeField + fieldId;
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Abilities
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A list of string that is comma-separated
            /// </summary>
            public static readonly int AbilityList = "\0abi".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A list of string that is comma-separated
            /// </summary>
            public static readonly int HeroAbilityList = "\0hab".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A list of string that is comma-separated
            /// </summary>
            public static readonly int AbilitySkinList = "\0abs".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A list of string that is comma-separated
            /// </summary>
            public static readonly int HeroAbilitySkinList = "\0has".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Art
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int ButtonPosX = "\0bpx".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int ButtonPosY = "\0upy".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int UnButtonPosX = "\0upx".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int UnButtonPosY = "\0upy".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int CasterUpgradeArt = "\0cua".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int GameInterfaceIcon = "\0ico".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int GameScoreScreenIcon = "\0ssi".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int ModelPath = "\0mdl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int DeathTime = "\0tdm".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int BlendTime = "\0ble".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int CastBackswing = "\0cbs".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int CastPoint = "\0cpt".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int RunSpeed = "\0run".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int WalkSpeed = "\0wal".FromRawcode();

            /// <summary>
            /// In angle.<br/>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int MaxPitch = "\0mxp".FromRawcode();

            /// <summary>
            /// In angle
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int MaxRoll = "\0mxr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int ElevationPoint = "\0ept".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ElevationRadius = "\0erd".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int FogRadius = "\0frd".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int BuildingGroundTexture = "\0ubs".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>, But accept 0 or 1 as it casted to boolean
            /// </summary>
            public static readonly int ShadowOnWater = "\0shr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/><br/>
            /// <list type="bullet">
            ///     <item>
            ///     <term>0</term>
            ///     <description>None</description>
            ///     </item>
            ///     <item>
            ///     <term>1</term>
            ///     <description>Reign of Chaos</description>
            ///     </item>
            ///     <item>
            ///     <term>2</term>
            ///     <description>The Frozen Throne</description>
            ///     </item>
            ///     <item>
            ///     <term>3</term>
            ///     <description>Reign of Chaos, The Frozen Throne</description>
            ///     </item>
            /// </list>
            /// </summary>
            public static readonly int FileVerFlag = "\0ver".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int OccluderHeight = "\0occ".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int OrientInterpolation = "\0ori".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ProjectileImpactZ = "\0imz".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ProjectileImpactZSwim = "\0isz".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ProjectileLaunchX = "\0lpx".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ProjectileLaunchY = "\0lpy".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ProjectileLaunchZ = "\0lpz".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ProjectileLaunchZSwim = "\0lsz".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>, in degree
            /// </summary>
            public static readonly int PropulsionWindow = "\0prw".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int RequiredAnimationNames = "\0ani".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int RequiredAnimationNamesAttachment = "\0aap".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int RequiredAttachmentLinkNames = "\0alp".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int RequiredBonesNames = "\0bpr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>, But accept 0 or 1 as it casted to boolean
            /// </summary>
            public static readonly int ScaleProperties = "\0scb".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ModelScale = "\0sca".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int SelectionCircleHeight = "\0slz".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>, But accept 0 or 1 as it casted to boolean
            /// </summary>
            public static readonly int SelectionCircleOnWater = "\0sew".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int SelectionScale = "\0ssc".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int ShadowImage = "\0shu".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ShadowCenterX = "\0shx".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ShadowCenterY = "\0shy".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ShadowHeight = "\0shz".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int ShadowWidth = "\0shw".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. For building
            /// </summary>
            public static readonly int ShadowBuildingTexture = "\0shb".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A list of string that is comma-separated
            /// </summary>
            public static readonly int SpecialModel = "\0spa".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A list of string that is comma-separated
            /// </summary>
            public static readonly int SpecialTarget = "\0taa".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// <list type="bullet">
            ///     <item>
            ///     <term>-1</term>
            ///     <description>Match owning player</description>
            ///     </item>
            ///     <item>
            ///     <term>0 to 23</term>
            ///     <description>Player 1 to Player 24</description>
            ///     </item>
            ///     <item>
            ///     <term>24</term>
            ///     <description>Neutral Hostile</description>
            ///     </item>
            /// </list>
            /// </summary>
            public static readonly int TeamColor = "\0tco".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Only accept 0 to 255
            /// </summary>
            public static readonly int TintingRed = "\0clr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Only accept 0 to 255
            /// </summary>
            public static readonly int TintingGreen = "\0clg".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Only accept 0 to 255
            /// </summary>
            public static readonly int TintingBlue = "\0clb".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Only accept 0 or 1 (false, true) as it will be casted to boolean
            /// </summary>
            public static readonly int UseExtendedLineOfSight = "\0los".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Combat
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int AcquisitionRange = "\0acq".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. And only accept value from the list below
            /// <list type="bullet">
            ///     <item>Ethereal</item>
            ///     <item>Flesh</item>
            ///     <item>Metal</item>
            ///     <item>Stone</item>
            ///     <item>Wood</item>
            /// </list>
            /// </summary>
            public static readonly int ArmorType = "\0arm".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1BackswingPoint = "\0bs1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1DamagePoint = "\0dp1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1FullDamageArea = "\0a1f".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1MediumDamageArea = "\0a1h".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1SmallDamageArea = "\0a1q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Only accept <see cref="FieldTarget"/> object
            /// </list>
            /// </summary>
            public static readonly int Attack1SplashTarget = "\0a1p".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1CooldownInterval = "\0a1c".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1BaseDamage = "\0a1b".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1MediumDamageFactor = "\0hd1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1SmallDamageFactor = "\0qd1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1DamageLossFactor = "\0dl1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1DamageDices = "\0a1d".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1DamageSides = "\0a1s".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1DamageSpillDistance = "\0sd1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1DamageSpillRadius = "\0sr1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1DamageUpgradeAmount = "\0du1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1TargetCount = "\0tc1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1MissileArc = "\0ma1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Attack1MissileModel = "\0a1m".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 0 or 1 as false or true
            /// </summary>
            public static readonly int Attack1MissileHoming = "\0mh1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1MissileSpeed = "\0a1z".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1Range = "\0a1r".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack1RangeMotionBuffer = "\0rb1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack1ShowUI = "\0wu1".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Only accept <see cref="FieldTarget"/> object
            /// </summary>
            public static readonly int Attack1TargetAllowed = "\0a1q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Attack1WeaponSound = "\0a1q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Attack1WeaponType = "\0a1q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2BackswingPoint = "\0bs2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2DamagePoint = "\0dp2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2FullDamageArea = "\0a2f".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2MediumDamageArea = "\0a2h".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2SmallDamageArea = "\0a2q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Only accept <see cref="FieldTarget"/> object
            /// </list>
            /// </summary>
            public static readonly int Attack2SplashTarget = "\0a2p".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2CooldownInterval = "\0a2c".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2BaseDamage = "\0a2b".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2MediumDamageFactor = "\0hd2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2SmallDamageFactor = "\0qd2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2DamageLossFactor = "\0dl2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2DamageDices = "\0a2d".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2DamageSides = "\0a2s".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2DamageSpillDistance = "\0sd2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2DamageSpillRadius = "\0sr2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2DamageUpgradeAmount = "\0du2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2TargetCount = "\0tc2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2MissileArc = "\0ma2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Attack2MissileModel = "\0a2m".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 0 or 1 as false or true
            /// </summary>
            public static readonly int Attack2MissileHoming = "\0mh2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2MissileSpeed = "\0a2z".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2Range = "\0a2r".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int Attack2RangeMotionBuffer = "\0rb2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Attack2ShowUI = "\0wu2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Only accept <see cref="FieldTarget"/> object
            /// </summary>
            public static readonly int Attack2TargetAllowed = "\0a2q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Attack2WeaponSound = "\0a2q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Attack2WeaponType = "\0a2q".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/><br/>
            /// Accept 0 for none, 1 for Attack 1 only, 2 for Attack 2 only, 3 for Both Attack
            /// </summary>
            public static readonly int AttackEnabled = "\0wu2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/><br/>
            /// <list type="bullet">
            ///     <item>
            ///         <term>0</term>
            ///         <description>Can't Rise, Does not decay</description>
            ///     </item>
            ///     <item>
            ///         <term>1</term>
            ///         <description>Can Rise, Does not decay</description>
            ///     </item>
            ///     <item>
            ///         <term>2</term>
            ///         <description>Can't Rise, Does decay</description>
            ///     </item>
            ///     <item>
            ///         <term>3</term>
            ///         <description>Can Rise, Does decay</description>
            ///     </item>
            ///
            /// </list>
            /// </summary>
            public static readonly int DeathTypes = "\0wu2".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/><br/>
            /// </summary>
            public static readonly int BaseDefense = "\0def".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Only accept "<c>normal, small, medium, large, fort, hero, divine, none</c>"
            /// </summary>
            public static readonly int DefenseType = "\0dty".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/><br/>
            /// </summary>
            public static readonly int DefenseUpgradeAmount = "\0def".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int MinimumAttackRange = "\0a2r".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Only accept <see cref="FieldTarget"/> object
            /// </list>
            /// </summary>
            public static readonly int TargetedAs = "\0a2p".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Editor
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int CanDropItemDeath = "\0dro".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int CategorizationCampaign = "\0cam".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int CategorizationSpecial = "\0spe".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int DisplayAsNeutralHostile = "\0hos".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int HasTilesetSpecificData = "\0tss".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int PlaceableInWorldEditor = "\0ine".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int UseClickHelper = "\0uch".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Movement
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 as true, false.
            /// </summary>
            public static readonly int GroupSeparationEnabled = "\0rpo".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int GroupSeparationGroupNumber = "\0rpg".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int GroupSeparationParameter = "\0rpp".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int GroupSeparationPriority = "\0rpr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int MoveHeight = "\0mvh".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int MoveHeightMinimum = "\0mvf".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int MovementSpeedBase = "\0mvs".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int MovementSpeedMax = "\0mas".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int MovementSpeedMin = "\0mis".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int TurnRate = "\0mvr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/><br/>
            /// Accept "<c>NONE, foot, horse, fly, hover, float, amph</c>"
            /// </summary>
            public static readonly int MovementType = "\0mvt".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Pathing
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int PathingAIPlacementRadius = "\0abr".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int PathingAIReplacementType = "\0abt".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int CollisionSize = "\0col".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int BuildingPathingMap = "\0pat".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A comma-separated list of string<br/>
            /// Accept: <c>"blighted, unamph, unbuildable, unfloat, unflyable, unwalkable"</c> for value and <c>"_"</c> for empty list
            /// </summary>
            public static readonly int BuildingPathingPlacementPrevented = "\0par".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. A comma-separated list of string<br/>
            /// Accept: <c>"blighted, unamph, unbuildable, unfloat, unflyable, unwalkable"</c> for value and <c>"_"</c> for empty list
            /// </summary>
            public static readonly int BuildingPathingPlacementRequired = "\0pap".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int BuildingPathingPlacementWaterRadius = "\0paw".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Sound
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int BuildingConstructionSound = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int LoopingFadeInRate = "\0lfi".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int LoopingFadeOutRate = "\0lfo".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int MovementSound = "\0msl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int RandomSoundLabel = "\0rsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int UnitSound = "\0snd".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Stats
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int AgilityPerLevel = "\0agp".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BuildTime = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int CanFlee = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int FoodCost = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int FoodProduced = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int FormationRank = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseGoldBounty = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int GoldBountyDices = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int GoldBountySides = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int GoldCost = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int HideHeroDeathMessage = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int HideHeroBar = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int HideHeroMinimap = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int HideOnMinimap = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseHP = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int BaseHPRegen = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int RegenType = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int IntPerLevel = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int IsBuillding = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Level = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseWoodBounty = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int WoodBountyDices = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int WoodBountySides = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int LumberCost = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int InitialMana = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseMana = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int BaseManaRegen = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int PointValue = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. Accept STR, INT or AGI
            /// </summary>
            public static readonly int PrimaryAttribute = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int Priority = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Race = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int RepairGoldCost = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int RepairWoodCost = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int RepairTime = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int SightRadiusDay = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int SightRadiusNight = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>. Accept 1 or 0 for true, false.
            /// </summary>
            public static readonly int Sleeps = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseAGI = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseINT = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int BaseSTR = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int InitialStockAfterDelay = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int StockMaximum = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int StockReplenishInterval = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int StockStartDelay = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Unreal"/>
            /// </summary>
            public static readonly int StrPerLevel = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int TransportedSize = "\0bsl".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Classification = "\0bsl".FromRawcode();
        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class TechTree
        {

        }

        /// <summary>
        /// Contain general field id, use <see cref="ConvertFieldForUnit(int)"/> or etc. to convert it to usable field!
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>. You DO NOT NEED TO CONVERT THIS FIELD!
            /// </summary>
            public static readonly int Description = "ides".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Hotkey = "\0hot".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Name = "\0nam".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int EditorSuffix = "\0nsf".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int ProperName = "\0pro".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.Int"/>
            /// </summary>
            public static readonly int ProperNameCount = "\0pru".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int AwakenTooltip = "\0awt".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int Tooltip = "\0tip".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int TooltipExtended = "\0tub".FromRawcode();

            /// <summary>
            /// This is a <see cref="ObjectDataType.String"/>
            /// </summary>
            public static readonly int ReviveTip = "\0tpr".FromRawcode();
        }
    }
}
