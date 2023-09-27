// ------------------------------------------------------------------------------
// <copyright file="SaveSlot.cs" company="Kray Oristine">
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
using System.Text;
using Source.GameSystem.W3OOP;
using Source.Shared;
using static War3Api.Common;


namespace Source.GameSystem.UI
{
    public static class SaveSlot
    {
        private static Frame _slotInfo;
        private static Frame _slotList;
        private static Frame _prevPageBtn;
        private static Frame _nextPageBtn;
        private static Frame _deleteSaveBtn;
        private static Frame _si_heroImage;
        private static Frame _si_heroName;
        private static Frame _si_heroClass;
        private static Frame _si_heroPlaytime;
        private static Frame _si_heroProperty;
        private static Frame _si_heroExtra;

        private static readonly List<Frame> _slotBtn = new();
        private static readonly Trigger _trig = new();
        private static readonly Dictionary<int, int> _activePage = new();
        private static readonly Dictionary<int, bool> _wantDelete = new();
        private static int _activeSlot = -1;
        private static readonly Trigger _trig2 = new();
        private static readonly Trigger _trig3 = new();
        private const int maxPage = 2;

        public static bool IsOpen { get => _slotList.IsVisible(); }

        private static void SS_SlotOnClick()
        {
            var triggerFrame = Frame.GetTriggerFrame();
            if (triggerFrame == null) return;
            if (GetLocalPlayer() == GetTriggerPlayer())
            {
                // Use only local code (no net traffic)
                if (triggerFrame == _deleteSaveBtn)
                {

                }

                for (int i = 0; i < _slotBtn.Count; i++)
                {
                    if (_slotBtn[i] == triggerFrame)
                    {
                        _activeSlot = i;
                    }
                }

            }
        }

        private static void SS_PageMove(int targetPage)
        {
            int pagePos = targetPage - 1;
            for (int i = pagePos * 4; i < 4 * pagePos + 4; i++)
            {
                _slotBtn[i].SetVisible(false);
                _slotBtn[i - 4].SetVisible(true);
            }
        }

        private static void SS_PageOnClick()
        {
            var triggerFrame = Frame.GetTriggerFrame();
            var p = GetTriggerPlayer();
            if (triggerFrame == null) return;
            if (GetLocalPlayer() == p)
            {
                int pid = GetPlayerId(p);
                if (!_activePage.TryGetValue(pid, out var currentPage)) currentPage = 1;
                if (triggerFrame == _prevPageBtn)
                {
                    if (currentPage == 1) return;
                    currentPage--;
                    SS_PageMove(currentPage);
                    if (currentPage == 1) _prevPageBtn.SetEnable(false);
                    _activePage[pid] = currentPage;
                    return;
                }

                if (triggerFrame == _nextPageBtn)
                {


                    return;
                }
            }
        }

        private static void SS_DeleteSaveOnClick()
        {
            var triggerFrame = Frame.GetTriggerFrame();
            var p = GetTriggerPlayer();
            if (triggerFrame == null) return;
            int pid = GetPlayerId(p);
            if (!_wantDelete[pid])
            {
                _wantDelete[pid] = true;
                if (GetLocalPlayer() == p) _deleteSaveBtn.SetText("|cfffcd20Click again to delete|r");
                return;
            }

            _wantDelete[pid] = false;
            if (GetLocalPlayer() == p)
            {
                _deleteSaveBtn.SetEnable(false);
                _slotInfo.SetVisible(false);



                _deleteSaveBtn.SetText("|cfffcd20Delete Save");

            }
        }

        public static void Init()
        {
            Frame.GetByName("ConsoleUIBackdrop", 0).SetSize(0, 0.0001f);
            var origin = Frame.GetByName("ConsoleUIBackdrop", 0);

            // core and main + background frame for slot information
            _slotInfo = new("QuestButtonPushedBackdropTemplate", origin, 0, 0);
            _slotInfo.SetAbsolutePoint(Frame.FramePoint.TOP_LEFT, 0.22914f, 0.46843f);
            _slotInfo.SetAbsolutePoint(Frame.FramePoint.BOTTOM_RIGHT, 0.54852f, 0.17624f);
            // core and main + background frame for slot list and shit
            _slotList = new("QuestButtonBaseTemplate", origin, 0, 0);
            _slotList.SetAbsolutePoint(Frame.FramePoint.TOP_LEFT, 0.00133f, 0.46566f);
            _slotList.SetAbsolutePoint(Frame.FramePoint.BOTTOM_RIGHT, 0.21127f, 0.1757f);


            // slot list and action when a button is clicked
            _nextPageBtn = new("ScriptDialogButton", _slotList, 0, 0);
            _nextPageBtn.SetPoint(Frame.FramePoint.TOP_LEFT, _slotList, Frame.FramePoint.TOP_LEFT, 0.13848f, -0.28288f);
            _nextPageBtn.SetPoint(Frame.FramePoint.BOTTOM_RIGHT, _slotList, Frame.FramePoint.BOTTOM_RIGHT, -0.016740f, -0.020770f);
            _nextPageBtn.SetText("|cfffcd20>>|r");
            _nextPageBtn.SetScale(1.0f);
            _prevPageBtn = new("ScriptDialogButton", _slotList, 0, 0);
            _nextPageBtn.SetPoint(Frame.FramePoint.TOP_LEFT, _slotList, Frame.FramePoint.TOP_LEFT, 0.016240f, -0.28288f);
            _nextPageBtn.SetPoint(Frame.FramePoint.BOTTOM_RIGHT, _slotList, Frame.FramePoint.BOTTOM_RIGHT, -0.13898f, -0.019690f);
            _prevPageBtn.SetEnable(false);
            _prevPageBtn.SetText("|cfffcd20<<|r");
            _prevPageBtn.SetScale(1.0f);

            _nextPageBtn.RegisterEvent(_trig2, Frame.Event.MOUSE_CLICK);
            _prevPageBtn.RegisterEvent(_trig2, Frame.Event.MOUSE_CLICK);
            _trig2.AddAction(SS_PageOnClick);

            _deleteSaveBtn = new("ScriptDialogButton", _slotInfo, 0, 0);
            _deleteSaveBtn.SetPoint(Frame.FramePoint.TOP_LEFT, _slotInfo, Frame.FramePoint.TOP_LEFT, 0.18649f, -0.25207f);
            _deleteSaveBtn.SetPoint(Frame.FramePoint.BOTTOM_RIGHT, _slotInfo, Frame.FramePoint.BOTTOM_RIGHT, -0.013400f, 0.011120f);
            _deleteSaveBtn.SetText("|cfffcd20Delete Save|r");
            _deleteSaveBtn.SetScale(1.2f);
            _deleteSaveBtn.RegisterEvent(_trig3, Frame.Event.MOUSE_CLICK);
            _trig3.AddAction(SS_DeleteSaveOnClick);

            int i = 0;
            int j = 1;
            while (true) {
                var btn = new Frame("ScriptDialogButton", _slotList, 0, 0);
                _slotBtn.Add(btn);
                btn.SetEnable(false);
                btn.SetPoint(Frame.FramePoint.TOP_LEFT, _slotList, Frame.FramePoint.TOP_LEFT, 0.01352f, -0.01639f - (0.06907f * i));
                btn.SetPoint(Frame.FramePoint.BOTTOM_RIGHT, _slotList, Frame.FramePoint.BOTTOM_RIGHT, -0.01128f, 0.2245f - (0.06907f * i));
                btn.SetText($"|cfffcd20DSlot {_slotBtn.Count}|r");
                btn.SetTextAlignment(Frame.TextAlignment.MIDDLE, Frame.TextAlignment.CENTER);
                btn.SetScale(1.71f);
                btn.RegisterEvent(_trig, Frame.Event.MOUSE_CLICK);
                i++;
                if (i >= 4 && j++ <= maxPage) i = 0;
                else if (j > maxPage) break;
            }
            Utility.FillDictionary(_activePage, 1, 24);
            Utility.FillDictionary(_wantDelete, false, 24);
            _slotInfo.SetVisible(false);
            _trig.AddAction(SS_SlotOnClick);

            // slot info

        }
    }
}
