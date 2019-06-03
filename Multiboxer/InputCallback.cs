﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace Multiboxer
{
    class InputCallback
    {
        /* InputCallback
         * Handles subscribe/unsubscribe from global keyboard/mouse hooks and callback methods.
         * Also holds the main ProcessManager instance, which is accessible only through InputCallback */

        public ProcessManager ProcManager;

        private IKeyboardMouseEvents m_GlobalHook;

        public InputCallback()
        {
            ProcManager = new ProcessManager();
        }

        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyDown += InputCallback_OnKeyDown;
            m_GlobalHook.KeyUp += InputCallback_OnKeyUp;
            //m_GlobalHook.MouseDown += Hook_OnMouseDown;
            //m_GlobalHook.MouseUp += Hook_OnMouseUp;
        }

        private void InputCallback_OnKeyDown(object sender, KeyEventArgs e)
        {
            ProcManager.RefreshGameProcessList();

            bool keyIsBlacklisted = false;

            int i = 0;

            foreach (Keys key in ProcManager.IgnoredKeys)
            {
                if (e.KeyCode.Equals(ProcManager.IgnoredKeys[i]))
                {
                    keyIsBlacklisted = true;
                }

                i++;
            }

            if (keyIsBlacklisted)
            {
                return;
            }
            else
            {
                foreach (Process p in ProcManager.GameProcessList)
                {
                    if (!p.Id.Equals(ProcManager.MasterClient.Id))
                    {
                        WindowUtil.PostKeyDown(p.MainWindowHandle, (Keys)e.KeyValue);
                    }
                }
            }
        }

        private void InputCallback_OnKeyUp(object sender, KeyEventArgs e)
        {
            ProcManager.RefreshGameProcessList();

            bool keyIsBlacklisted = false;

            int i = 0;

            foreach (Keys key in ProcManager.IgnoredKeys)
            {
                if (e.KeyCode.Equals(ProcManager.IgnoredKeys[i]))
                {
                    keyIsBlacklisted = true;
                }

                i++;
            }

            if (keyIsBlacklisted)
            {
                return;
            }
            else
            {
                foreach (Process p in ProcManager.GameProcessList)
                {
                    if (!p.Id.Equals(ProcManager.MasterClient.Id))
                    {
                        WindowUtil.PostKeyUp(p.MainWindowHandle, (Keys)e.KeyValue);
                    }
                }
            }
        }

        private void InputCallback_OnMouseDown(object sender, MouseEventArgs e)
        {
            ProcManager.RefreshGameProcessList();

            foreach (Process p in ProcManager.GameProcessList)
            {
                if (!p.Id.Equals(ProcManager.MasterClient.Id))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        WindowUtil.PostMouseLeftDown(p.MainWindowHandle);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        WindowUtil.PostMouseRightDown(p.MainWindowHandle);
                    }
                }
            }
        }

        private void InputCallback_OnMouseUp(object sender, MouseEventArgs e)
        {
            ProcManager.RefreshGameProcessList();

            foreach (Process p in ProcManager.GameProcessList)
            {
                if (!p.Id.Equals(ProcManager.MasterClient.Id)) // .Equals() compares the contents, == compares the reference
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        WindowUtil.PostMouseLeftUp(p.MainWindowHandle);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        WindowUtil.PostMouseRightUp(p.MainWindowHandle);
                    }
                }
            }
        }

        public void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= InputCallback_OnKeyDown;
            m_GlobalHook.KeyUp -= InputCallback_OnKeyUp;
            //m_GlobalHook.MouseDown -= Hook_OnMouseDown;
            //m_GlobalHook.MouseUp -= Hook_OnMouseUp;

            m_GlobalHook.Dispose();
        }
    }
}
