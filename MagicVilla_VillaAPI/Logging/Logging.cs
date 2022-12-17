namespace MagicVilla_VillaAPI.Logging
{
    public class Logging : ILogging
    {
        public void Log(string msg, string type)
        {
            if(type=="error")
                Console.WriteLine("ERROR - "+msg);
            else
                Console.WriteLine(msg);
        }
    }
}
