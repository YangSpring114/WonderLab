namespace PathLib
{
    public static class PathLib
    {
        public static string getSubPath(string mainPath, string subPath) { 
            if(mainPath == null)
            {
                return subPath;
            }
            if(subPath == null)
            {
                return mainPath;
            }
            return Path.Combine(mainPath, subPath); 
        }
    }
}