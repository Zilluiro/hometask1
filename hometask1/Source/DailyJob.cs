using Quartz;

namespace hometask1.Source
{
    internal class DailyJob : IJob
    {
        public const string JOB_NAME = "meta";
        public const string GROUP_NAME = "group1";
        public const string TRIGGER_NAME = "trigger_meta";
            
        public async Task Execute(IJobExecutionContext context)
        {
            var saver = context.MergedJobDataMap["saver"] as Saver;

            var folder = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
            await saver.SaveMetaFileAsync(folder);
        }
    }
}
