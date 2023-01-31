using System;
using static War3Api.Common;
using static War3Api.Blizzard;

namespace Source.Modules
{
    public sealed class ArcingTT
    {
         internal static readonly float SIZE_MIN = 0.018f; // Minimum size of text
         internal static readonly float SIZE_BONUS = 0.012f; // Text size increase
         internal static readonly float TIME_LIFE = 1.0f; // How long the text lasts
         internal static readonly float TIME_FADE = 0.8f; // When does the text start to fade
         internal static readonly int Z_OFFSET = 70; // Height above unit
         internal static readonly int Z_OFFSET_BON = 40; // How much extra height the text gains
         internal static readonly int VELOCITY = 2; //  How fast the text moves in x/y plane
         internal static readonly float ANGLE = bj_PI / 2; // Movement angle of the text (only if ANGLE_RND is false)
         internal static readonly bool ANGLE_RND = true; // Is the angle random or fixed

        private static ArcingTT lastCreated;

        public static ArcingTT LastCreated { get => lastCreated; }


        private readonly timer tmr;
        public ArcingTT(string str, unit u, float x, float y, float duration, int size, player p)
        {
            tmr = CreateTimer();
            lastCreated = Create(str, u, x, y, duration, size, p);
        }

        public void Destroy()
        {
            PauseTimer(tmr);
            DestroyTimer(tmr);
        }

        public ArcingTT Create(string str, unit u, float x, float y, float duration, int size, player p)
        {
            float a = ANGLE_RND ? GetRandomReal(0, 2 * bj_PI) : ANGLE;
            float timeScale = Math.Max(duration, 0.001f);
            float lifeSpan = TIME_LIFE * timeScale;
            float asin = (float)(Math.Sin(a) * VELOCITY);
            float acos = (float)(Math.Cos(a) * VELOCITY);
            texttag? tt = null;
            if (IsUnitVisible(u, p))
            {
                tt = CreateTextTag();
                SetTextTagPermanent(tt, false);
                SetTextTagLifespan(tt, lifeSpan);
                SetTextTagFadepoint(tt, TIME_FADE * timeScale);
                SetTextTagText(tt, str, SIZE_MIN * size);
                SetTextTagPos(tt, x, y, Z_OFFSET);
            }

            float pass = 0;
            TimerStart(tmr, 0.03125f, true, () => {
                pass += 0.03125f;
                if (tt == null) return;
                if (pass >= lifeSpan)
                {
                    this.Destroy();
                    return;
                }
                float point = (float)Math.Sin(bj_PI * ((lifeSpan - pass) / timeScale));
                x += acos;
                y += asin;
                SetTextTagPos(tt, x, y, Z_OFFSET + Z_OFFSET_BON * point);
                SetTextTagText(tt, str, (SIZE_MIN + SIZE_BONUS * point) * size);
            });

            return this;
        }

        public static ArcingTT CreateEx(string str, unit u, float duration, int size)
        {
            return new(str, u, GetUnitX(u), GetUnitY(u), duration, size, GetLocalPlayer());
        }
    }
}
