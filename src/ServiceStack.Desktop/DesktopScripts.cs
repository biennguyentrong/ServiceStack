using System;
using System.Collections.Generic;
using ServiceStack.Script;
// ReSharper disable InconsistentNaming

namespace ServiceStack.Desktop
{
    public class DesktopScripts : ScriptMethods
    {
        private readonly Func<ScriptScopeContext,IntPtr> windowFactory;
        public DesktopScripts(Func<ScriptScopeContext,IntPtr> windowFactory) => 
            this.windowFactory = windowFactory;

        bool DoWindow(ScriptScopeContext scope, Action<IntPtr> fn)
        {
            var hWnd = windowFactory(scope);
            if (hWnd != IntPtr.Zero)
            {
                fn(hWnd);
                return true;
            }
            return false;
        }
        
        T DoWindow<T>(ScriptScopeContext scope, Func<IntPtr,T> fn)
        {
            var hWnd = windowFactory(scope);
            return hWnd != IntPtr.Zero ? fn(hWnd) : default;
        }

        public bool openUrl(string url) =>
            NativeWin.Open(new Uri(url).ToString());
        public bool open(string cmd) =>
            NativeWin.Open(cmd);
        public Dictionary<string, string> desktopInfo() => 
            NativeWin.GetDesktopInfo();
        public Dictionary<string, object> deviceScreenResolution() =>
            NativeWin.toObject(NativeWin.GetScreenResolution());
        public string clipboard() =>
            NativeWin.GetClipboardAsString();
        public bool setClipboard(string data) => 
            NativeWin.SetStringInClipboard(data);
        public int messageBox(string text, string caption, uint type) => 
            NativeWin.MessageBox(0, text, caption, type);
        public string expandEnvVars(ScriptScopeContext scope, string path) => 
            NativeWin.expandEnvVars(path);

        public Dictionary<string, object> primaryMonitorInfo(ScriptScopeContext scope) =>
            DoWindow(scope, w => w.GetPrimaryMonitorInfo(out var mi) ? NativeWin.toObject(mi) : null);
        public bool windowSendToForeground(ScriptScopeContext scope) =>
            DoWindow(scope, w => w.SetForegroundWindow());
        public bool windowCenterToScreen(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.CenterToScreen());
        public bool windowCenterToScreen(ScriptScopeContext scope, bool useWorkArea) => 
            DoWindow(scope, w => w.CenterToScreen(useWorkArea));
        public bool windowSetFullScreen(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.SetWindowFullScreen());
        public bool windowSetFocus(ScriptScopeContext scope) => 
            DoWindow(scope, w => { w.SetFocus(); });
        public bool windowShowScrollBar(ScriptScopeContext scope, bool show) => 
            DoWindow(scope, w => w.ShowScrollBar(show));
        public bool windowSetPosition(ScriptScopeContext scope, int x, int y, int width, int height) =>
            DoWindow(scope, w => w.SetPosition(x,y,width,height));
        public bool windowSetPosition(ScriptScopeContext scope, int x, int y) =>
            DoWindow(scope, w => w.SetPosition(x,y));
        public bool windowSetSize(ScriptScopeContext scope, int width, int height) =>
            DoWindow(scope, w => w.SetSize(width, height));
        public bool windowRedrawFrame(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.RedrawFrame());
        public bool windowIsVisible(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.IsWindowVisible());
        public bool windowIsEnabled(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.IsWindowEnabled());
        public bool windowShow(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.ShowWindow(ShowWindowCommands.SW_SHOW));
        public bool windowHide(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.ShowWindow(ShowWindowCommands.SW_HIDE));
        public string windowText(ScriptScopeContext scope) => 
            DoWindow(scope, w => w.GetText());
        public bool windowSetText(ScriptScopeContext scope, string text) => 
            DoWindow(scope, w => w.SetText(text));
        public bool windowSetState(ScriptScopeContext scope, int state) => 
            DoWindow(scope, w => w.ShowWindow((ShowWindowCommands)state));
        
        public Dictionary<string, object> windowSize(ScriptScopeContext scope) =>
            DoWindow(scope, w => NativeWin.toObject(w.GetWindowSize()));
        public Dictionary<string, object> windowClientSize(ScriptScopeContext scope) => 
            DoWindow(scope, w => NativeWin.toObject(w.GetClientSize()));
        public Dictionary<string, object> windowClientRect(ScriptScopeContext scope) => 
            DoWindow(scope, w => NativeWin.toObject(w.GetClientRect()));

        public DialogResult openFile(ScriptScopeContext scope, Dictionary<string, object> options) => 
            DoWindow(scope, w => w.openFile(options));
    }
    
}