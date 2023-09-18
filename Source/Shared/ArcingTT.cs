// ------------------------------------------------------------------------------
// <copyright file="ArcingTT.cs" company="Kray Oristine">
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
using WCSharp.Events;
using static War3Api.Blizzard;
using static War3Api.Common;


namespace Source.Shared
{
    public sealed class ArcingTT : IPeriodicAction
    {
        private const float SIZE_MIN = 0.018f; // Minimum size of text
        private const float SIZE_BONUS = 0.012f; // Text size increase
        private const float TIME_LIFE = 1.0f; // How long the text lasts
        private const float TIME_FADE = 0.8f; // When does the text start to fade
        private const int Z_OFFSET = 70; // Height above unit
        private const int Z_OFFSET_BON = 50; // How much extra height the text gains
        private const int VELOCITY = 4; //  How fast the text moves in x/y plane
        private const float ANGLE = bj_PI / 2; // Movement angle of the text (only if ANGLE_RND is false)

        private static readonly PeriodicTrigger<ArcingTT> periodicTrigger = new(1.0f / 32.0f);
        private static readonly Stack<ArcingTT> cache = new();

        public float passed = 0;
        public float lifeSpan;
        public float asin;
        public float acos;
        public float timeScale;
        public texttag? tt;
        public float x;
        public float y;
        public string text;
        public int size;
        public bool Active { get; set; }

        public void Action()
        {
            passed += 1.0f / 32.0f;
            if (passed >= lifeSpan)
            {
                Active = false;
                tt = null;
                cache.Push(this);
                return;
            }
            if (tt == null) return;
            float point = (float)Math.Sin(Math.PI * ((lifeSpan - passed) / timeScale));
            x += acos;
            y += asin;
            SetTextTagPos(tt, x, y, Z_OFFSET + Z_OFFSET_BON * point);
            SetTextTagText(tt, text, (SIZE_MIN + SIZE_BONUS * point) * size);
        }

        internal static ArcingTT GetCache(float time, float life, float asin, float acos, texttag? tt, string text, float x, float y)
        {
            if (cache.Count > 0)
            {
                var temp = cache.Pop();
                temp.lifeSpan = life;
                temp.timeScale = time;
                temp.asin = asin;
                temp.acos = acos;
                temp.tt = tt;
                temp.text = text;
                temp.x = x;
                temp.y = y;
                return temp;
            }

            return new(time, life, asin, acos, tt, text, x, y);
        }

        internal ArcingTT(float time, float life, float asin, float acos, texttag? tt, string text, float x, float y)
        {
            lifeSpan = life;
            timeScale = time;
            this.asin = asin;
            this.acos = acos;
            this.tt = tt;
            this.text = text;
            this.x = x;
            this.y = y;
        }

        public static ArcingTT Create(string str, unit u, float x, float y, float duration, int size, player? p)
        {
            p ??= GetLocalPlayer();

            float a = GetRandomReal(0, 2 * bj_PI);
            float time = Math.Max(duration, 0.001f);
            float life = TIME_LIFE * time;
            float angleSin = Sin(a) * VELOCITY;
            float angleCos = Cos(a) * VELOCITY;
            texttag? tag = null;
            if (IsUnitVisible(u, p))
            {
                tag = CreateTextTag();
                SetTextTagPermanent(tag, false);
                SetTextTagLifespan(tag, life);
                SetTextTagFadepoint(tag, TIME_FADE * time);
                SetTextTagText(tag, str, SIZE_MIN * size);
                SetTextTagPos(tag, x, y, Z_OFFSET);
            }

            var t = GetCache(time, life, angleSin, angleCos, tag, str, x, y);

            periodicTrigger.Add(t);

            return t;
        }
    }
}
