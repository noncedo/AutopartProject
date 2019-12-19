using System;

namespace Service
{
    public static class Constants
    {
        public const String FtpHost = "FtpHost";
        public const String FtpUsername = "FtpUsername";
        public const String FtpPassword = "FtpPassword";

        public const String DMSourcePath = "DMSourcePath";
        public const String DMDestinationPath = "DMDestinationPath";
        public const String DMSourceFilename = "DMSourceFilename";
        public const String DMDestinationFilename = "DMDestinationFilename";
        public const String DMLocalWorkingPath = "DMLocalWorkingPath";

        public const String RVSPathOut = "RVSPathOut";
        public const String MVSPath = "MVSPath";
        public const String MVSExt = "MVSExt";

        public const String I02UOrderReplenishmentFileNameFromDealer = "I02UOrderReplenishmentFileNameFromDealer";
        public const String I02UOrderReplenishmentFileNameToRVS = "I02UOrderReplenishmentFileNameToRVS";

        public const String RVSPathIn = "RVSPathIn";
        public const String I02OrderReplenishmentFileNameFromRVS = "I02OrderReplenishmentFileNameFromRVS";
        public const String I02LocalWorkingPath = "I02LocalWorkingPath";
        public const String I02FileNameFromRVSHeaderOrDetailPosition = "I02FileNameFromRVSHeaderOrDetailPosition";
        public const String I02FileNameFromRVSDealerCodeStartPosition = "I02FileNameFromRVSDealerCodeStartPosition";
        public const String I02FileName = "I02FileName";

        public enum LogType { INFO, DEBUG, ERROR };
    }
}

