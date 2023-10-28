// ------------------------------------------------------------------------------
// <copyright file="Frame.cs" company="Kray Oristine">
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
using Source.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using static War3Api.Common;

namespace Source.GameSystem.W3OOP
{
#pragma warning disable CS0824 // Constructor is marked external
#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    /// <summary>
    ///  Warcraft Frame Wrapper
    /// </summary>
    /// @CSharpLua.Ignore
    public sealed class Frame : IDisposable
    {

        #region origin UI

        /// @CSharpLua.Ignore
        public sealed class OriginType
        {
            private OriginType() { }

            /// <summary>
            /// The entire game UI
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_GAME_UI"
            public static readonly OriginType GAME_UI;

            /// <summary>
            /// the buttons to order units around, like ITEM_BUTTON, Reappear/update every selection. (range from 0 - 11)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_COMMAND_BUTTON"
            public static readonly OriginType COMMAND_BUTTON;

            /// <summary>
            /// Parent of all HERO_BUTTONS, HeroButtons share the same visibility.
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_HERO_BAR"
            public static readonly OriginType HERO_BAR;

            /// <summary>
            /// The clickable buttons of own/allied heroes on the left of the screen (range from 0 - 6)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_HERO_BUTTON"
            public static readonly OriginType HERO_BUTTON;

            /// <summary>
            /// The clickable buttons of own/allied heroes on the left of the screen (range from 0 - 6)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_HERO_HP_BAR"
            public static readonly OriginType HERO_HP_BAR;

            /// <summary>
            /// The clickable buttons of own/allied heroes on the left of the screen (range from 0 - 6)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_HERO_MANA_BAR"
            public static readonly OriginType HERO_MANA_BAR;

            /// <summary>
            /// The glowing when a hero has skill points; connected to HeroButtons.<br/>
            /// They reappear when a hero gains a new skill point even when all origin frames are hidden. (range from 0 - 6)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_HERO_BUTTON_INDICATOR"
            public static readonly OriginType HERO_BUTTON_INDICATOR;

            /// <summary>
            /// Items in the inventory. Reappear/updates every selection, When its parent is visible. (range from 0 - 5)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_ITEM_BUTTON"
            public static readonly OriginType ITEM_BUTTON;

            /// <summary>
            /// The mini map
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_MINIMAP"
            public static readonly OriginType MINIMAP;

            /// <summary>
            /// 0 is the Button at top to 4 Button at the bottom (range from 0 - 4)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_MINIMAP_BUTTON"
            public static readonly OriginType MINIMAP_BUTTON;

            /// <summary>
            /// Game top left button (range from 0 - 3)
            /// <list type="bullet">
            ///     <item>
            ///         <term>0. Menu</term>
            ///         <description>The game F10 menu screen</description>
            ///     </item>
            ///     <item>
            ///         <term>1. Allies</term>
            ///         <description>The game ally menu screen</description>
            ///     </item>
            ///     <item>
            ///         <term>2. Log</term>
            ///         <description>The game message log screen</description>
            ///     </item>
            ///     <item>
            ///         <term>3. Quest</term>
            ///         <description>The game quest menu</description>
            ///     </item>
            /// </list>
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_SYSTEM_BUTTON"
            public static readonly OriginType SYSTEM_BUTTON;

            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_TOOLTIP"
            public static readonly OriginType TOOLTIP;

            /// <summary>
            /// Handles the Tooltip window
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_UBERTOOLTIP"
            public static readonly OriginType UBERTOOLTIP;

            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_CHAT_MSG"
            public static readonly OriginType CHAT_MSG;

            /// <summary>
            /// print frame for game display message / DisplayTextToPlayer
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_UNIT_MSG"
            public static readonly OriginType UNIT_MSG;

            /// <summary>
            /// Frame: UpKeep Change Warning Message, below the dayTime Clock
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_TOP_MSG"
            public static readonly OriginType TOP_MSG;

            /// <summary>
            /// Face of the main selected Unit,<br/>
            /// uses a different coordination system 0,0 being the absolute bottomLeft which makes it difficult to be used together with other frames (not 4:3, like for the others)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_PORTRAIT"
            public static readonly OriginType PORTRAIT;

            /// <summary>
            /// The visible playground, units, items, effects, fog... every object participating in the game is displayed on it.
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_WORLD_FRAME"
            public static readonly OriginType WORLD_FRAME;

            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_SIMPLE_UI_PARENT"
            public static readonly OriginType SIMPLE_UI_PARENT;

            /// <summary>
            /// Does not exist until any selection was done and a short time passed.<br/>
            /// Force a Selection and wait a short time. Does only pos itself correctly with FRAMEPOINT_CENTER. Probably a String-Frame. (BlzFrameGetText, BlzFrameSetFont are working onto it and BlzFrameSetAlpha/BlzFrameSetLevel crash the game like String-Frames tend to do)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_PORTRAIT_HP_TEXT"
            public static readonly OriginType PORTRAIT_HP_TEXT;

            /// <summary>
            /// Does not exist until any selection was done and a short time passed.<br/>
            /// Force a Selection and wait a short time. Does only pos itself correctly with FRAMEPOINT_CENTER. Probably a String-Frame. (BlzFrameGetText, BlzFrameSetFont are working onto it and BlzFrameSetAlpha/BlzFrameSetLevel crash the game like String-Frames tend to do)
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_PORTRAIT_MANA_TEXT"
            public static readonly OriginType PORTRAIT_MANA_TEXT;

            /// <summary>
            /// The Frame has a size to fit all 8 possible Buffs
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_UNIT_PANEL_BUFF_BAR"
            public static readonly OriginType UNIT_PANEL_BUFF_BAR;

            /// <summary>
            /// Attached to ORIGIN_FRAME_UNIT_PANEL_BUFF_BAR on default. Probably a String-Frame.
            /// </summary>
            /// @CSharpLua.Template = "ORIGIN_FRAME_UNIT_PANEL_BUFF_BAR_LABEL"
            public static readonly OriginType UNIT_PANEL_BUFF_BAR_LABEL;
        }

        /// <summary>
        ///  Get a <see cref="Frame"/> by specifying a specific <see cref="OriginType"/> and index (in most cases it should be 0)<br/><br/>
        /// </summary>
        /// <param name="frameType"></param>
        /// <param name="index"></param>
        /// <returns>An origin frame</returns>
        /// <remarks>Will occupy 1 _handle id each time it return a frame not yet exists in the game</remarks>
        /// @CSharpLua.Template = "BlzGetOriginFrame({0}, {1})"
        public static extern Frame GetOrigin(OriginType frameType, int index);

        /// <summary>
        /// Hides/Shows most of the default in-game UI. Unaffected: Mouse, Command Buttons, Chat, Messages, TimerDialog, Multiboard, Leaderboard and ConsoleUIBackdrop.<br/>
        /// (De)Activate some auto-repositioning of default frames (see: <see cref="EnableAutoUIPosition"/>).
        /// </summary>
        /// <param name="enable"></param>
        /// @CSharpLua.Template = "BlzHideOriginFrames({0})"
        public static extern void HideOrigin(bool enable);

        /// <summary>
        /// Disabling this will prevent the game using default positions for changed hidden frames as soon they reappear/their state is changed.
        /// </summary>
        /// <param name="enable"></param>
        /// @CsharpLua.Template = "BlzEnableAutoUIPosition({0})"
        public static extern void EnableAutoUIPosition(bool enable);

        #endregion

        #region frame creation

        /// <summary>
        /// Create a new Frame using a Frame-BluePrint name (fdf) as child of owner.<br/>
        /// BluePrint needs to be loaded over TOC and fdf.<br/>
        /// Owner and BluePrint have to be from the Frame family.
        /// </summary>
        /// <param name="name">The frame name defined in FDF</param>
        /// <param name="owner">The parent of this frame</param>
        /// <param name="priority"></param>
        /// <param name="createContext">Index to be used by <see cref="GetByName(string,int)"/></param>
        /// <remarks>Can only create rootFrames (not subFrames)</remarks>
        /// @CSharpLua.Template = "BlzCreateFrame({0}, {1}, {2}, {3})"
        public extern Frame(string name, Frame? owner, int priority, int createContext);

        /// /// <summary>
        /// Create a new Frame using a Frame-BluePrint name (fdf) as child of owner.<br/>
        /// BluePrint needs to be loaded over TOC and fdf.<br/>
        /// Owner and BluePrint have to be from the Frame family.
        /// </summary>
        /// <param name="name">The frame name defined in FDF</param>
        /// <param name="owner">The parent of this frame</param>
        /// <param name="priority"></param>
        /// <remarks>Can only create rootFrames (not subFrames)</remarks>
        /// @CSharpLua.Template = "BlzCreateSimpleFrame({0}, {1}, {2})"
        public extern Frame(string name, Frame? owner, int priority);

        /// <summary>
        /// Create and Define a new (Simple)Frame.<br/>
        /// Can use a root-(Simple)Frame-BluePrint with inherits, when that is done it needs to be a loaded BluePrint.
        /// </summary>
        /// <param name="typeName">Frame type name</param>
        /// <param name="name">Frame name</param>
        /// <param name="owner">The parent of this frame</param>
        /// <param name="inherits">The parent rootFrames frame to inherits</param>
        /// <param name="createContext">Index to be used by <see cref="GetByName(string,int)"/></param>
        /// @CSharpLua.Template = "BlzCreateFrameByType({0}, {1}, {2}, {3}, {4})"
        public extern Frame(string typeName, string name, Frame? owner, string inherits, int createContext);

        /// <summary>
        /// Create a new Frame using a Frame-BluePrint name (fdf) as child of owner.<br/>
        /// BluePrint needs to be loaded over TOC and fdf.<br/>
        /// Owner and BluePrint have to be from the Frame family.
        /// </summary>
        /// <param name="name">The frame name defined in FDF</param>
        /// <param name="owner">The parent of this frame</param>
        /// <param name="priority"></param>
        /// <param name="createContext">Index to be used by <see cref="GetByName(string,int)"/></param>
        /// <remarks>Can only create rootFrames (not subFrames)</remarks>
        /// @CSharpLua.Template = "BlzCreateFrame({0}, {1}, {2}, {3})"
        public extern Frame Create(string name, Frame? owner, int priority, int createContext);

        /// /// <summary>
        /// Create a new Frame using a Frame-BluePrint name (fdf) as child of owner.<br/>
        /// BluePrint needs to be loaded over TOC and fdf.<br/>
        /// Owner and BluePrint have to be from the Frame family.
        /// </summary>
        /// <param name="name">The frame name defined in FDF</param>
        /// <param name="owner">The parent of this frame</param>
        /// <param name="priority"></param>
        /// <remarks>Can only create rootFrames (not subFrames)</remarks>
        /// @CSharpLua.Template = "BlzCreateSimpleFrame({0}, {1}, {2})"
        public extern Frame CreateSimple(string name, Frame? owner, int priority);

        /// <summary>
        /// Create and Define a new (Simple)Frame.<br/>
        /// Can use a root-(Simple)Frame-BluePrint with inherits, when that is done it needs to be a loaded BluePrint.
        /// </summary>
        /// <param name="typeName">Frame type name</param>
        /// <param name="name">Frame name</param>
        /// <param name="owner">The parent of this frame</param>
        /// <param name="inherits">The parent rootFrames frame to inherits</param>
        /// <param name="createContext">Index to be used by <see cref="GetByName(string,int)"/></param>
        /// @CSharpLua.Template = "BlzCreateFrameByType({0}, {1}, {2}, {3}, {4})"
        public extern Frame CreateByType(string typeName, string name, Frame? owner, string inherits, int createContext);

        /// <summary>
        /// Get back the <see cref="framehandle"/> of this frame object
        /// </summary>
        /// <returns>It original <see cref="framehandle"/></returns>
        /// @CSharpLua.Get = "{this}"
        public framehandle Handle { get; }

        #endregion

        #region frame management

        /// <summary>
        /// Load the TOC file from the given path
        /// </summary>
        /// <param name="path"></param>
        /// @CSharpLua.Template = "BlzLoadTOCFile({0})"
        public static extern bool LoadTOC(string path);

        /// <summary>
        /// Get the frame from it name, and get it from the game internal Frame-storage
        /// </summary>
        /// <param name="name">Target frame name</param>
        /// <param name="createContext">Index of the target frame</param>
        /// <returns>A warcraft frame</returns>
        /// <remarks>Will occupy 1 _handle id each time it return a frame not yet exists in the game</remarks>
        /// @CSharpLua.Template = "BlzGetFrameByName({0}, {1})"
        public static extern Frame GetByName(string name, int createContext);

        /// <summary>
        /// Get the parent of this frame
        /// </summary>
        /// <returns>The parent frame</returns>
        /// <remarks>Will occupy 1 _handle id each time it return a frame not yet exists in the game</remarks>
        /// @CSharpLua.Get = "BlzFrameGetParent({this})"
        public Frame Parent { get; }

        /// <summary>
        /// Get the amount of children within this frame
        /// </summary>
        /// <returns></returns>
        /// @CSharpLua.Get = "BlzFrameGetChildrenCount({this})"
        public int ChildrenCount { get; }

        /// <summary>
        /// Get the selected children frame at given index within this frame
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzFrameGetChild({this}, {0})"
        public extern Frame GetChild(int index);

        /// <summary>
        /// Destroy this frame
        /// </summary>
        /// @CSharpLua.Template = "BlzDestroyFrame({this})"
        public extern void Dispose();

        #endregion

        #region frame position management

        /// @CSharpLua.Ignore
        public sealed class FramePoint
        {
            private FramePoint() { }

            /// @CSharpLua.Template = "FRAMEPOINT_TOPLEFT"
            public static readonly FramePoint TOP_LEFT;

            /// @CSharpLua.Template = "FRAMEPOINT_TOP"
            public static readonly FramePoint TOP;

            /// @CSharpLua.Template = "FRAMEPOINT_TOPRIGHT"
            public static readonly FramePoint TOP_RIGHT;

            /// @CSharpLua.Template = "FRAMEPOINT_LEFT"
            public static readonly FramePoint LEFT;

            /// @CSharpLua.Template = "FRAMEPOINT_CENTER"
            public static readonly FramePoint CENTER;

            /// @CSharpLua.Template = "FRAMEPOINT_RIGHT"
            public static readonly FramePoint RIGHT;

            /// @CSharpLua.Template = "FRAMEPOINT_BOTTOMLEFT"
            public static readonly FramePoint BOTTOM_LEFT;

            /// @CSharpLua.Template = "FRAMEPOINT_BOTTOM"
            public static readonly FramePoint BOTTOM;

            /// @CSharpLua.Template = "FRAMEPOINT_BOTTOMRIGHT"
            public static readonly FramePoint BOTTOM_RIGHT;

        }

        /// <summary>
        /// Unbinds a point of this frame and places it relative to a point of <paramref name="relative"/> frame.<br/>
        /// When <paramref name="relative"/> moves this frame point will keep this rule and moves with it.<br/>
        /// Each point of a frame can be placed to one point. By placing multiple points of one Frame a Size is enforced.
        /// </summary>
        /// <param name="point">Target point</param>
        /// <param name="relative">A reference frame to be bound</param>
        /// <param name="relativePoint">A reference point to be bound</param>
        /// <param name="x">Target X Position</param>
        /// <param name="y">Target Y Position</param>
        /// @CSharpLua.Template = "BlzFrameSetPoint({this}, {0}, {1}, {2}, {3}, {4})"
        public extern void SetPoint(FramePoint point, Frame relative, FramePoint relativePoint, float x, float y);

        /// <summary>
        /// Set this frame position to the absolute X, Y.<br/>
        /// Specify a frame point cause this frame to be moved relatively to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// @CSharpLua.Template = "BlzFrameSetAbsPoint({this}, {0}, {1}, {2})"
        public extern void SetAbsolutePoint(FramePoint? point, float x, float y);


        /// <summary>
        /// removes all current bound points of this frame.
        /// </summary>
        /// @CSharpLua.Template = "BlzFrameClearAllPoints({this})"
        public extern void ClearAllPoints();

        /// <summary>
        /// Cause this frame to copy the <paramref name="relative"/> frame in size and position<br/>
        /// This frame will be updated when <paramref name="relative"/> frame is changed later on.
        /// </summary>
        /// <param name="relative">Source frame to be copied</param>
        /// @CSharpLua.Template = "BlzFrameSetAllPoints({this}, {0})"
        public extern void SetAllPoints(Frame relative);

        #endregion

        #region frame value management

        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Ignore
        public sealed class TextAlignment
        {
            private TextAlignment() { }

            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "TEXT_JUSTIFY_TOP"
            public static readonly TextAlignment TOP;
            ///
            /// </summary>
            /// @CSharpLua.Template = "TEXT_JUSTIFY_MIDDLE"
            public static readonly TextAlignment MIDDLE;
            ///
            /// </summary>
            /// @CSharpLua.Template = "TEXT_JUSTIFY_BOTTOM"
            public static readonly TextAlignment BOTTOM;
            ///
            /// </summary>
            /// @CSharpLua.Template = "TEXT_JUSTIFY_LEFT"
            public static readonly TextAlignment LEFT;
            ///
            /// </summary>
            /// @CSharpLua.Template = "TEXT_JUSTIFY_CENTER"
            public static readonly TextAlignment CENTER;
            ///
            /// </summary>
            /// @CSharpLua.Template = "TEXT_JUSTIFY_RIGHT"
            public static readonly TextAlignment RIGHT;
        }

        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetText({this})"
        /// @CSharpLua.Set = "BlzFrameSetText({this}, {0})"
        public extern string Text { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetAlpha({this})"
        /// @CSharpLua.Set = "BlzFrameSetAlpha({this}, {0})"
        public extern int Alpha { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetScale({this})"
        /// @CSharpLua.Set = "BlzFrameSetScale({this}, {0})"
        public extern float Scale { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetValue({this})"
        /// @CSharpLua.Set = "BlzFrameSetValue({this}, {0})"
        public extern float Value { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetTextSizeLimit({this})"
        /// @CSharpLua.Set = "BlzFrameSetTextSizeLimit({this}, {0})"
        public extern int TextSizeLimit { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetWidth({this})"
        /// @CSharpLua.Set = "BlzFrameSetSize({this}, {0}, BlzFrameGetHeight({this}))"
        public extern float Width { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetHeight({this})"
        /// @CSharpLua.Set = "BlzFrameSetSize({this}, BlzFrameGetWidth({this}), {0})"
        public extern float Height { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetEnable({this})"
        /// @CSharpLua.Set = "BlzFrameSetEnable({this}, {0})"
        public extern bool Enable { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetVisible({this})"
        /// @CSharpLua.Set = "BlzFrameSetVisible({this}, {0})"
        public extern bool Visible { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Get = "BlzFrameGetName({this})"
        public extern string Name { get; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// @CSharpLua.Template = "BlzFrameAddText({this}, {0})"
        public extern void AddText(string text);

        /// <summary>
        ///
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// @CSharpLua.Template = "BlzFrameSetSize({this}, {0}, {1})"
        public extern void SetSize(float width, float height);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="height"></param>
        /// <param name="flags"></param>
        /// @CSharpLua.Template = "BlzFrameSetFont({this}, {0}, {1}, {2})"
        public extern void SetFont(string fileName, float height, int flags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="flags"></param>
        /// @CSharpLua.Template = "BlzFrameSetFocus({this}, {0})"
        public extern void SetFocus(bool flags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="modelFile"></param>
        /// <param name="cameraIndex"></param>
        /// @CSharpLua.Template = "BlzFrameSetModel({this}, {0}, {1})"
        public extern void SetModel(string modelFile, int cameraIndex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// @CSharpLua.Template = "BlzFrameSetLevel({this}, {0})"
        public extern void SetLevel(int level);

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// @CSharpLua.Template = "BlzFrameSetParent({this}, {0})"
        public extern void SetParent(Frame parent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="textureFile"></param>
        /// <param name="flags"></param>
        /// <param name="blend"></param>
        /// @CSharpLua.Template = "BlzFrameSetTexture({this}, {0}, {1}, {2})"
        public extern void SetTexture(string textureFile, int flags, bool blend);

        /// <summary>
        ///
        /// </summary>
        /// <param name="tooltip"></param>
        /// @CSharpLua.Template = "BlzFrameSetTooltip({this}, {0})"
        public extern void SetTooltip(Frame tooltip);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stepSize"></param>
        /// @CSharpLua.Template = "BlzFrameSetStepSize({this}, {0})"
        public extern void SetStepSize(float stepSize);

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// @CSharpLua.Template = "BlzFrameSetTextColor({this}, {0}:ToArgb())"
        public extern void SetTextColor(Color color);

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// @CSharpLua.Template = "BlzFrameSetMinMaxValue({this}, {0})"
        public extern void SetMinMaxValue(float min, float max);

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        /// @CSharpLua.Template = "BlzFrameSetVertexColor({this}, {0}:ToArgb())"
        public extern void SetVertexColor(Color color);

        /// <summary>
        ///
        /// </summary>
        /// <param name="primaryProp"></param>
        /// <param name="flags"></param>
        /// @CSharpLua.Template = "BlzFrameSetSpriteAnimate({this}, {0}, {1})"
        public extern void SetSpriteAnimate(int primaryProp, int flags);

        /// <summary>
        ///
        /// </summary>
        /// <param name="vertical"></param>
        /// <param name="horizontal"></param>
        /// @CSharpLua.Template = "BlzFrameSetTextAlignment({this}, {0}, {1})"
        public extern void SetTextAlignment(TextAlignment vertical, TextAlignment horizontal);

        #endregion

        #region frame trigger management

        /// <summary>
        ///
        /// </summary>
        /// @CSharpLua.Ignore
        public sealed class Event
        {
            private Event() { }

            /// <summary>
            /// Mouse click (down 'nd up)
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_CONTROL_CLICK"
            public static readonly Event MOUSE_CLICK;
            /// <summary>
            /// Mouse enter frame area
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_ENTER"
            public static readonly Event MOUSE_ENTER;
            /// <summary>
            /// Mouse leave frame area
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_LEAVE"
            public static readonly Event MOUSE_LEAVE;
            /// <summary>
            /// Mouse go up (after down)
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_UP"
            public static readonly Event MOUSE_UP;
            /// <summary>
            /// Mouse go down
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_DOWN"
            public static readonly Event MOUSE_DOWN;
            /// <summary>
            /// On mouse wheel action
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_WHEEL"
            public static readonly Event MOUSE_WHEEL;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_CHECKBOX_CHECKED"
            public static readonly Event CHECKBOX_CHECKED;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_CHECKBOX_UNCHECKED"
            public static readonly Event CHECKBOX_UNCHECKED;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_EDITBOX_TEXT_CHANGED"
            public static readonly Event EDITBOX_TEXT_CHANGED;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_POPUPMENU_ITEM_CHANGED"
            public static readonly Event POPUPMENU_ITEM_CHANGED;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_MOUSE_DOUBLECLICK"
            public static readonly Event MOUSE_DOUBLECLICK;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_SPRITE_ANIM_UPDATE"
            public static readonly Event SPRITE_ANIM_UPDATE;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_SLIDER_CHANGED"
            public static readonly Event SLIDER_CHANGED;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_DIALOG_CANCEL"
            public static readonly Event DIALOG_CANCEL;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_DIALOG_ACCEPT"
            public static readonly Event DIALOG_ACCEPT;
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "FRAMEEVENT_EDITBOX_ENTER"
            public static readonly Event EDITBOX_ENTER;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="whichEvent"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzTriggerRegisterFrameEvent({0}, {this}, {1})"
        public extern @event RegisterEvent(Trigger trigger, Event whichEvent);

        /// <summary>
        /// Get the frame that is triggered
        /// </summary>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzGetTriggerFrame()"
        public static extern Frame GetTriggerFrame();

        /// <summary>
        /// Get the event at which it triggered
        /// </summary>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzGetTriggerFrameEvent()"
        public static extern Event GetTriggerEvent();

        /// <summary>
        /// Only set if the event has use of this
        /// </summary>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzGetTriggerFrameValue()"
        public static extern float GetTriggerValue();

        /// <summary>
        /// Only set if the event has use of this
        /// </summary>
        /// <returns></returns>
        /// @CSharpLua.Template = "BlzGetTriggerFrameText()"
        public static extern string GetTriggerText();

        /// <summary>
        /// Click this frame and trigger "<see cref="Event.MOUSE_CLICK"/>
        /// </summary>
        /// @CSharpLua.Template = "BlzFrameClick({this})"
        public extern void Click();

        /// <summary>
        /// Make the user mouse can't leave out of the frame.<br/>
        /// New cage will overwrite old ones. Some frame won't able to trap the mouse.
        /// </summary>
        /// <param name="enable"></param>
        /// @CSharpLua.Template = "BlzFrameCageMouse({this}, {0})"
        public extern void CageMouse(bool enable);

        #endregion
    }


#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore S4200 // Native methods should be wrapped
#pragma warning restore CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning restore CS0824 // Constructor is marked external
}
