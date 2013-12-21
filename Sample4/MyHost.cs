using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;
using Sample4;

namespace Sample3
{
    class MyHost : PSHost
    {
        private Program program;

        private CultureInfo originalCultureInfo =
            System.Threading.Thread.CurrentThread.CurrentCulture;

        private CultureInfo originalUICultureInfo =
            System.Threading.Thread.CurrentThread.CurrentUICulture;

        private Guid myId = Guid.NewGuid();

        public MyHost(Program program)
        {
            this.program = program;
        }

        public override System.Globalization.CultureInfo CurrentCulture
        {
            get { return this.originalCultureInfo; }
        }

        public override System.Globalization.CultureInfo CurrentUICulture
        {
            get { return this.originalUICultureInfo; }
        }

        public override Guid InstanceId
        {
            get { return this.myId; }
        }

        public override string Name
        {
            get { return "MySampleConsoleHost"; }
        }

        public override PSHostUserInterface UI
        {
            get { return null; }
        }

        public override Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public override void EnterNestedPrompt()
        {
            throw new NotImplementedException(
                "The method or operation is not implemented.");
        }

        public override void ExitNestedPrompt()
        {
            throw new NotImplementedException(
                "The method or operation is not implemented.");
        }

        public override void NotifyBeginApplication()
        {
            return;
        }

        public override void NotifyEndApplication()
        {
            return;
        }

        public override void SetShouldExit(int exitCode)
        {
            this.program.ShouldExit = true;
            this.program.ExitCode = exitCode;
        }
    }
}
