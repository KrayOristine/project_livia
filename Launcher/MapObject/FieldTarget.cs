using System;
using System.Collections.Generic;

namespace Launcher.MapObject
{
    public sealed class FieldTarget
    {
        private readonly List<string> _list;
        private bool _empty = true;

        private bool _isClassification


        public FieldTarget()
        { _list = new() { "_" }; }

        public FieldTarget(Target target)
        { _list = new() { Enum.GetName(target).ToLower() }; _empty = false; }

        public FieldTarget(string targetList)
        {
            var list = targetList.Split(',');
            if (list.Length == 0 || list[0] == "_") throw new InvalidCastException("Invalid string casting to FieldTarget");

            foreach (var item in list)
            {
                _list.Add(item);
            }
        }

        /// <summary>
        /// Made this field become a classification target
        /// </summary>
        /// <param name="enable"></param>
        public void SetClassification(bool enable)
        {
            _isClassification = enable;
            _list.Clear();
            _empty = true;
            _list[0] = "_";
        }

        public FieldTarget Add(Target target)
        {
            if (_isClassification) return this;
            if (_empty) _list.Clear();

            _list.Add(Enum.GetName(target).ToLower());
            _empty = false;

            return this;
        }

        public FieldTarget Add(Classification target)
        {
            if (!_isClassification) return this;
            if (_empty) _list.Clear();

            _list.Add(Enum.GetName(target).ToLower());
            _empty = false;

            return this;
        }

        public FieldTarget Remove(Target target)
        {
            if (_isClassification || _empty) return this;

            _list.Remove(Enum.GetName(target).ToLower());

            return this;
        }

        public FieldTarget Remove(Classification target)
        {
            if (!_isClassification || _empty) return this;

            _list.Remove(Enum.GetName(target).ToLower());

            CheckEmpty();

            return this;
        }

        private void CheckEmpty()
        {
            if (_empty || _list.Count > 0) return;

            _list[0] = "_";
            _empty = true;
        }

        public void Clear()
        {
            _list.Clear();
            _list[0] = "_";

            _empty = true;
        }

        public override string ToString()
        {
            return string.Join(',', _list);
        }

        public static implicit operator string(FieldTarget ft) => ft.ToString();

        public static explicit operator FieldTarget(string ft) => new(ft);

        public enum Classification
        {
        }

        public enum Target
        {
            Air,
            Alive,
            Allies,
            Ancient,
            Bridge,
            Dead,
            Debris,
            Decoration,
            Enemies,
            Friend,
            Ground,
            Hero,
            Invulnerable,
            Item,
            Mechanical,
            Neutral,
            NonAncient,
            None,
            NonHero,
            NonSapper,
            NotSelf,
            Organic,
            Player,
            Sapper,
            Self,
            Structure,
            Terrain,
            Tree,
            Vulnerable,
            Wall,
            Ward
        }
    }
}
