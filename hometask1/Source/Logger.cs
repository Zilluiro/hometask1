namespace hometask1.Source
{
    internal class Logger
    {
        public void Log(string info)
        {
            var datetime = DateTime.Now.ToString();

            Console.WriteLine($"[{datetime}] {info}");
        }
    }
}
