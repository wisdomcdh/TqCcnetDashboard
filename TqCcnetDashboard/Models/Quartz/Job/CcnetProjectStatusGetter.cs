﻿using Quartz;
using TqLib.ccnet.Local;

namespace TqCcnetDashboard.Quartz.Job
{
    public class CcnetProjectStatusGetter : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var host = context.JobDetail.JobDataMap.GetString("HOST");

            try
            {
                using (var ccnet = new CCNET(host))
                {
                    var projects = ccnet.Server.GetProjectStatus();
                    CurrentSite.ProjectStatusCollection[host] = projects;
                    CurrentSite.OnProjectChagned(host);
                }
            }
            catch { }
        }
    }
}