namespace Launcher
{
    internal static class BuilderConfig
    {
        // The base path to the solution
        public const string BASE_PATH = @"D:\dev\project_livia\";

        public const string SOURCE_CODE_PATH = BASE_PATH + @"Source\";
        public const string ASSETS_PATH = BASE_PATH + @"Assets\";
        public const string MAP_PATH = BASE_PATH + @"BaseMap\" + BASE_MAP_NAME;
        public const string BASE_MAP_NAME = "source.w3x";

        // other resources
        public const string RESOURCES_PATH = BASE_PATH + @"Resources\";

        // Output
        public const string OUT_ARTIFACTS = BASE_PATH + @"Artifacts\";

        public const string OUT_SCRIPT_NAME = @"war3map.lua";
        public const string OUT_MAP_NAME = @"target.w3x";
    }
}
