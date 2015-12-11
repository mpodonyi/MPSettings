using System;
using System.IO;

namespace MPSettings.Utils
{
    public class PathHelper
    {
       



        //private static string GetCurrentAppPath
        //{
        //    if (!Path.IsPathRooted(sqlFile))
        //        sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sqlFile);
        //    return File.ReadAllText(sqlFile);
        //}





//        public static string GetAppPath()
//        {


//#if NET
//            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
//#else
//            throw new NotSupportedException();
//#endif

//        }


//        public static string Combine(string path1, string path2)
//        {
//#if NET
//            return Path.Combine(path1, path2);
//#else
//            throw new NotSupportedException();
//#endif

//        }

        public static Stream GetApplicationFileStream(string filename, bool readAndWrite)
        {
#if NET
            string apppath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string combinedpath = System.IO.Path.Combine(apppath, filename);
            return new FileStream(combinedpath,
                readAndWrite ? FileMode.OpenOrCreate : FileMode.Open,
                readAndWrite ? FileAccess.ReadWrite : FileAccess.Read);
#else
            throw new NotSupportedException();
#endif



        }




    }
}
