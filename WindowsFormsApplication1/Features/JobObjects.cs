using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MsAccessRestrictor.Interfaces;

namespace MsAccessRestrictor.Features {
    class JobObjects : IFeature {
        [DllImport("kernel32.dll", EntryPoint = "CreateJobObjectW", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateJobObject(SecurityAttributes JobAttributes, string lpName);

        public class SecurityAttributes {

            public int nLength;
            public IntPtr pSecurityDescriptor;
            public bool bInheritHandle;

            public SecurityAttributes() {
                bInheritHandle = true;
                nLength = 0;
                pSecurityDescriptor = IntPtr.Zero;
            }
        }

        [DllImport("kernel32.dll")]
        static extern bool SetInformationJobObject(IntPtr hJob, JOBOBJECTINFOCLASS JobObjectInfoClass, IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

        public enum JOBOBJECTINFOCLASS {
            JobObjectAssociateCompletionPortInformation = 7,
            JobObjectBasicLimitInformation = 2,
            JobObjectBasicUIRestrictions = 4,
            JobObjectEndOfJobTimeInformation = 6,
            JobObjectExtendedLimitInformation = 9,
            JobObjectSecurityLimitInformation = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        struct JOBOBJECT_BASIC_LIMIT_INFORMATION {
            private Int64 PerProcessUserTimeLimit;
            private Int64 PerJobUserTimeLimit;
            public Int16 LimitFlags;
            private UIntPtr MinimumWorkingSetSize;
            private UIntPtr MaximumWorkingSetSize;
            public Int16 ActiveProcessLimit;
            private Int64 Affinity;
            private Int16 PriorityClass;
            private Int16 SchedulingClass;
        }

        public enum LimitFlags {
            JOB_OBJECT_LIMIT_ACTIVE_PROCESS = 0x00000008,
            JOB_OBJECT_LIMIT_AFFINITY = 0x00000010,
            JOB_OBJECT_LIMIT_BREAKAWAY_OK = 0x00000800,
            JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION = 0x00000400,
            JOB_OBJECT_LIMIT_JOB_MEMORY = 0x00000200,
            JOB_OBJECT_LIMIT_JOB_TIME = 0x00000004,
            JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000,
            JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME = 0x00000040,
            JOB_OBJECT_LIMIT_PRIORITY_CLASS = 0x00000020,
            JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100,
            JOB_OBJECT_LIMIT_PROCESS_TIME = 0x00000002,
            JOB_OBJECT_LIMIT_SCHEDULING_CLASS = 0x00000080,
            JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK = 0x00001000,
            JOB_OBJECT_LIMIT_WORKINGSET = 0x00000001
        }


        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        private IntPtr _windowHandle;

        public void Run() {
            var process = Process.GetProcessesByName("MSACCESS");
            if (process.Count() == 0) {
                return;
            }

            _windowHandle = process[0].Handle;

            const string jobName = "MsAccess_JobGroup";

            // Create a Process Job and assign a MsAccess process to the job
            IntPtr jobHandle = CreateJobObject(null, jobName);
            AssignProcessToJobObject(jobHandle, _windowHandle);

            // Set the process limit of the job
            var limits = new JOBOBJECT_BASIC_LIMIT_INFORMATION {
                ActiveProcessLimit = 1,
                LimitFlags = (short)LimitFlags.JOB_OBJECT_LIMIT_ACTIVE_PROCESS
            };
            var pointerToJobLimitInfo = Marshal.AllocHGlobal(Marshal.SizeOf(limits));
            Marshal.StructureToPtr(limits, pointerToJobLimitInfo, false);
            SetInformationJobObject(jobHandle, JOBOBJECTINFOCLASS.JobObjectBasicLimitInformation,
                pointerToJobLimitInfo, (uint)Marshal.SizeOf(limits));

            // Assign the process to the job
            AssignProcessToJobObject(jobHandle, _windowHandle);
        }

        public void Clear() {
            //
        }
    }
}
