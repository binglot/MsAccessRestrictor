/*
 * A job object allows groups of processes to be managed as a unit. By setting the ActiveProcessLimit 
 * property on a job object we can limit the number of processes that can exist within a single job. 
 * As a result, prevent the assigned process from spawning new ones.
 * 
 * More on: http://blog.didierstevens.com/2010/09/13/runinsidelimitedjob/
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    class RunInsideLimitedJob : IFeature, IDisposable {
        const string MsAccessName = "MSACCESS";
        const string JobName = "MsAccess_JobGroup";
        readonly IntPtr _msAccessWindow;
        IntPtr _job;

        public RunInsideLimitedJob() {
            var process = Process.GetProcessesByName(MsAccessName);

            if (process.Count() == 1) {
                var attributes = new SecurityAttributes();
                _msAccessWindow = process[0].Handle;
                _job = WinApi.CreateJobObject(ref attributes, JobName);
            }
        }

        public void Run() {
            if (_job == IntPtr.Zero) {
                return;
            }

            if (SetActiveProcessLimit(_job, 1)) {
                AddProcess(_job, _msAccessWindow);
            }
        }

        public void Clear() {
            if (_job == IntPtr.Zero) {
                return;
            }

            SetActiveProcessLimit(_job, 10);
        }

        public void Dispose() {
            if (_job == IntPtr.Zero) {
                return;
            }

            WinApi.CloseHandle(_job);
            _job = IntPtr.Zero;
        }

        private static bool SetActiveProcessLimit(IntPtr job, short processLimit) {
            // Create a Process Job description
            var jobDescription = new JobObjectBasicLimitInformation {
                ActiveProcessLimit = processLimit,
                LimitFlags = (short)LimitFlags.JobObjectLimitActiveProcess
            };

            // Get a pointer to the job description
            var pointerToJobDescription = Marshal.AllocHGlobal(Marshal.SizeOf(jobDescription));
            Marshal.StructureToPtr(jobDescription, pointerToJobDescription, false);

            // Set the description to the created job
            var jobObjectInfoLength = (uint)Marshal.SizeOf(jobDescription);
            var setObjectInfo = WinApi.SetInformationJobObject(job, JobObjectInfoType.BasicLimitInformation,
                                                               pointerToJobDescription, jobObjectInfoLength);
            if (!setObjectInfo) {
                Debug.WriteLine(String.Format("Unable to set information.  Error: {0}", Marshal.GetLastWin32Error()));
                return false;
            }

            return true;
        }

        private static void AddProcess(IntPtr job, IntPtr process) {
            WinApi.AssignProcessToJobObject(job, process);
        }
    }

    #region Interop Structs

    public enum JobObjectInfoType {
        AssociateCompletionPortInformation = 7,
        BasicLimitInformation = 2,
        BasicUIRestrictions = 4,
        EndOfJobTimeInformation = 6,
        ExtendedLimitInformation = 9,
        SecurityLimitInformation = 5,
        GroupInformation = 11
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct JobObjectBasicLimitInformation {
        public Int64 PerProcessUserTimeLimit;
        public Int64 PerJobUserTimeLimit;
        public Int16 LimitFlags;
        public UInt32 MinimumWorkingSetSize;
        public UInt32 MaximumWorkingSetSize;
        public Int16 ActiveProcessLimit;
        public Int64 Affinity;
        public Int16 PriorityClass;
        public Int16 SchedulingClass;
    }

    public enum LimitFlags {
        JobObjectLimitActiveProcess = 0x00000008,
        JobObjectLimitAffinity = 0x00000010,
        JobObjectLimitBreakawayOk = 0x00000800,
        JobObjectLimitDieOnUnhandledException = 0x00000400,
        JobObjectLimitJobMemory = 0x00000200,
        JobObjectLimitJobTime = 0x00000004,
        JobObjectLimitKillOnJobClose = 0x00002000,
        JobObjectLimitPreserveJobTime = 0x00000040,
        JobObjectLimitPriorityClass = 0x00000020,
        JobObjectLimitProcessMemory = 0x00000100,
        JobObjectLimitProcessTime = 0x00000002,
        JobObjectLimitSchedulingClass = 0x00000080,
        JobObjectLimitSilentBreakawayOk = 0x00001000,
        JobObjectLimitWorkingset = 0x00000001
    }

    #endregion
}
