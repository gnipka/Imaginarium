using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WcfHosting
{
    partial class WinService : ServiceBase
    {
        private WcfService _Wcf;
        public WinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _Wcf = WcfService.Service;
                _Wcf.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, System.Diagnostics.EventLogEntryType.Information);
            }
        }

        protected override void OnStop()
        {
            if (_Wcf != null)
            { _Wcf.Stop(); }
        }
    }
}
