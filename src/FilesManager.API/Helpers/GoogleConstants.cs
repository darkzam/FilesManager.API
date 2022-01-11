namespace FilesManager.API.Helpers
{
    public static class GoogleConstants
    {
        private const string BaseUrl = "https://drive.google.com/uc";
        private const string Action = "export=download";

        public static string GenerateDownloadUrl(string resourceId)
        {
            return $"{BaseUrl}?id={resourceId}&{Action}";
        }
    }
}
