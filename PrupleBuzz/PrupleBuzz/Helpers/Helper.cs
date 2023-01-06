namespace PrupleBuzz.Helpers
{
    public  class Helper
    {

        public static void DeleteFile(string env,string path,string filename)
        {
          string FullPath=Path.Combine(env,path,filename);

            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }

        }
    }
}
