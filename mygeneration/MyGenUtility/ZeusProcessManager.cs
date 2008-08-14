﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

using Zeus;
using MyGeneration;

namespace MyGeneration
{
    public static class ZeusProcessManager
    {
        private static Queue<ZeusProcess> processQueue = new Queue<ZeusProcess>();
        private static ZeusProcess runningProcess;
        private static Thread monitorThread;

        public static Guid ExecuteTemplate(string filename, ZeusProcessStatusDelegate callback)
        {
            ZeusProcess zp = new ZeusProcess(ZeusProcessType.ExecuteTemplate, callback, filename);
            processQueue.Enqueue(zp);
            Start();
            return zp.ID;
        }

        public static Guid ExecuteSavedInput(string filename, ZeusProcessStatusDelegate callback)
        {
            ZeusProcess zp = new ZeusProcess(ZeusProcessType.ExecuteSavedInput, callback, filename);
            processQueue.Enqueue(zp);
            Start();
            return zp.ID;
        }

        public static Guid RecordTemplateInput(string templateFilename, string saveToFilename, ZeusProcessStatusDelegate callback)
        {
            ZeusProcess zp = new ZeusProcess(ZeusProcessType.RecordTemplateInput, callback, templateFilename, saveToFilename);
            processQueue.Enqueue(zp);
            Start();
            return zp.ID;
        }

        public static Guid ExecuteProject(string filename, ZeusProcessStatusDelegate callback)
        {
            ZeusProcess zp = new ZeusProcess(ZeusProcessType.ExecuteProject, callback, filename);
            processQueue.Enqueue(zp);
            Start();
            return zp.ID;
        }

        public static Guid ExecuteModule(string filename, string modulePath, ZeusProcessStatusDelegate callback)
        {
            ZeusProcess zp = new ZeusProcess(ZeusProcessType.ExecuteProjectModule, callback, filename, modulePath);
            processQueue.Enqueue(zp);
            Start();
            return zp.ID;
        }

        public static Guid ExecuteProjectItem(string filename, string instancePath, ZeusProcessStatusDelegate callback)
        {
            ZeusProcess zp = new ZeusProcess(ZeusProcessType.ExecuteProjectItem, callback, filename, instancePath);
            processQueue.Enqueue(zp);
            Start();
            return zp.ID;
        }

        public static void Start()
        {
            if ((monitorThread == null) ||
                (monitorThread.ThreadState == System.Threading.ThreadState.Aborted) ||
                (monitorThread.ThreadState == System.Threading.ThreadState.Stopped))
            {
                monitorThread = new Thread(new ThreadStart(runMonitorThread));
            }

            if  (monitorThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                monitorThread.Start();
            }
        }

        private static void runMonitorThread()
        {
            try
            {
                while (processQueue.Count > 0 || runningProcess != null)
                {
                    if (runningProcess == null)
                    {
                        runningProcess = processQueue.Dequeue();
                    }

                    runningProcess.Start();
                    while (!runningProcess.IsDormant)
                    {
                        Thread.Sleep(250);
                    }
                    runningProcess.Join();
                    runningProcess = null;
                }
            }
            catch (ThreadAbortException)
            {
                if (runningProcess != null)
                {
                    runningProcess.Kill();
                }
                processQueue.Clear();
            }
        }

        public static bool IsDormant
        {
            get
            {
                if (monitorThread == null) return true;
                else
                {
                    if (monitorThread.ThreadState == System.Threading.ThreadState.Aborted ||
                        monitorThread.ThreadState == System.Threading.ThreadState.Stopped ||
                        monitorThread.ThreadState == System.Threading.ThreadState.Unstarted)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static void KillAll()
        {
            if (monitorThread != null)
            {
                monitorThread.Abort();
            }

            monitorThread.Join(10 * 1000);
            monitorThread = null;
        }
    }

    public enum ZeusProcessType
    {
        ExecuteTemplate = 0,
        RecordTemplateInput,
        ExecuteSavedInput,
        ExecuteProject,
        ExecuteProjectModule,
        ExecuteProjectItem,

    }

    public class ZeusProcessStatusEventArgs : EventArgs
    {
        private bool _isRunning = false;
        private string _message = string.Empty;
        private Guid _id = Guid.NewGuid();

        public ZeusProcessStatusEventArgs(Guid id, bool isRunning, string message)
            : base()
        {
            this._id = id;
            this._isRunning = isRunning;
            this._message = message;
        }

        public Guid ID
        {
            get { return _id; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public string Message
        {
            get { return _message; }
        }
    }

    public delegate void ZeusProcessStatusDelegate(ZeusProcessStatusEventArgs success);

    public class ZeusProcess
    {
        private Guid _id = Guid.NewGuid();
        private Thread t;
        private ThreadData td;

        private class ThreadData
        {
            public ThreadData(ZeusProcessStatusDelegate cbk, Process p, Guid id)
            {
                CallbackHandlers += cbk;
                SysProcess = p;
                ID = id;
            }
            protected event ZeusProcessStatusDelegate CallbackHandlers;
            public void Callback(ZeusProcessStatusEventArgs args)
            {
                if (CallbackHandlers != null) CallbackHandlers(args);
            }
            public Process SysProcess;
            public Guid ID;
        }

        public ZeusProcess(ZeusProcessType type, ZeusProcessStatusDelegate callback, params string[] args)
        {
            ProcessStartInfo si = new ProcessStartInfo();

            if (args.Length > 0)
            {
                si.FileName = FileTools.ApplicationPath + "\\ZeusCmd.exe";
                si.CreateNoWindow = true;
                si.UseShellExecute = false;
                si.RedirectStandardOutput = true;
                string cmdArgs = "-internaluse ";
                if (type == ZeusProcessType.ExecuteTemplate)
                {
                    cmdArgs += "-t \"" + args[0] + "\"";
                }
                else if (type == ZeusProcessType.ExecuteSavedInput)
                {
                    cmdArgs += "-i \"" + args[0] + "\"";
                }
                else if (type == ZeusProcessType.RecordTemplateInput)
                {
                    cmdArgs += "-t \"" + args[0] + "\" -c \"" + args[1] + "\"";
                }
                else if (type == ZeusProcessType.ExecuteProject)
                {
                    cmdArgs += "-p \"" + args[0] + "\"";
                }
                else if (type == ZeusProcessType.ExecuteProjectModule)
                {
                    cmdArgs += "-p \"" + args[0] + "\" -m \"" + args[1] + "\"";
                }
                else if (type == ZeusProcessType.ExecuteProjectItem)
                {
                    cmdArgs += "-p \"" + args[0] + "\" -ti \"" + args[1] + "\"";
                }
                if (!string.IsNullOrEmpty(cmdArgs)) si.Arguments = cmdArgs;
            }
            
            Process process = new Process();
            process.StartInfo = si;
            ParameterizedThreadStart ts = new ParameterizedThreadStart(Start);
            t = new Thread(ts);
            td = new ThreadData(callback, process, _id);

        }

        public void Start()
        {
            t.Start(td);
        }

        public Guid ID
        {
            get { return _id; }
        }

        public bool IsDormant
        {
            get 
            {
                if (t == null) return true;
                else
                {
                    if (t.ThreadState == System.Threading.ThreadState.Aborted ||
                        t.ThreadState == System.Threading.ThreadState.Stopped ||
                        t.ThreadState == System.Threading.ThreadState.Unstarted)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool Kill()
        {
            bool joinSucceeded = false;
            if (t != null)
            {
                t.Abort();
                joinSucceeded = t.Join(5 * 1000); // 5 seconds
                t = null;
            }
            return joinSucceeded;
        }

        public bool Join()
        {
            bool joinSucceeded = false;
            if (t != null)
            {
                joinSucceeded = t.Join(5 * 1000); // 5 seconds max
                t = null;
            }
            return joinSucceeded;
        }

        private void Start(object o)
        {
            if (o is ThreadData)
            {
                ThreadData td = o as ThreadData;
                Process p = td.SysProcess;
                try
                {
                    string l;
                    p.Start();
                    while (!p.HasExited)
                    {
                        l = p.StandardOutput.ReadLine();
                        td.Callback(new ZeusProcessStatusEventArgs(td.ID, true, l));
                        // need to bubble up this line, and the status in a thread safe way.

                        // if marked to "kill", break out and kill process!
                    }

                    
                    do
                    {
                        try
                        {
                            l = p.StandardOutput.ReadLine();
                            if (l != null) td.Callback(new ZeusProcessStatusEventArgs(td.ID, true, l));
                        }
                        catch
                        {
                            l = null;
                        }
                    }
                    while (l != null);

                    td.Callback(new ZeusProcessStatusEventArgs(td.ID, false, "Completed"));
                }
                catch (ThreadAbortException)
                {
                    try
                    {
                        if (p != null && !p.HasExited) p.Kill();
                    }
                    catch { }
                    //td.Callback(new ZeusProcessStatusEventArgs(td.ID, false, "Killed Process"));
                }
            }
        }
    }
}