using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;

namespace Service
{
    public class I02Service : II02Service
    {
        private string outputFileName;
        private string dealerCode;
        private string previousDealerCode;
        private Int32 partCount;
        private bool firstTime;
        private string localWorkingPath;
        private string fileName;
        private string header;
        private byte[] newline;
        private IEnumerable<LogType> logTypes;
        int headerOrDetailPosition;

        public I02Service()
        {
            outputFileName = "";
            partCount = 0;
            dealerCode = "";
            previousDealerCode = "";
            firstTime = true;
            localWorkingPath = "";
            fileName = "";
            header = "";
            newline = Encoding.ASCII.GetBytes(Environment.NewLine);
        }

        

        public void RunProcess(IRepository repository, ILogger logger)
        {
            logTypes = repository.Get<LogType>();

            var process = repository.GetFirst<Process>(x => x.Name == "I02");

            var processRun = new ProcessRun { ProcessId = process.ProcessId, Start = DateTime.Now };

            repository.Create<ProcessRun>(processRun);
            repository.Save();

            logger.Log(repository, processRun.ProcessRunId, "Order Replenishment Process Start");
            logger.Log(repository, processRun.ProcessRunId, "Fetching RVS File");

            if (getRvsFile(repository, logger, processRun.ProcessRunId) == true)
            {
                splitFile(repository, logger, processRun.ProcessRunId);
            }

            
        }

        private bool splitFile(IRepository repository, ILogger logger, Int32 processRunId)
        {
            IEnumerable<Setting> settings;
            IEnumerable<Dealer> dealers;
            int headerOrDetailPosition;
            int fileNameFromRVSDealerCodeStartPosition;
            try
            {
                settings = repository.Get<Setting>();
                dealers = repository.Get<Dealer>(x => x.IsActive == true);
                localWorkingPath = settings.First(x => x.Key == Constants.I02LocalWorkingPath).Value;
                fileName = settings.First(x => x.Key == Constants.I02FileName).Value;
                headerOrDetailPosition = int.Parse(settings.First(x => x.Key == Constants.I02FileNameFromRVSHeaderOrDetailPosition).Value);
                fileNameFromRVSDealerCodeStartPosition = int.Parse(settings.First(x => x.Key == Constants.I02FileNameFromRVSDealerCodeStartPosition).Value);
            }
            catch (DbUpdateException ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Error loading settings from DB: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            catch (Exception ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Error loading settings: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }

            try
            {

                var sourceFile = repository.GetFirst<FileSource>(x => x.ProcessRunId == processRunId);
                var fileLines = repository.Get<FileLine>(x => x.FileSourceId == sourceFile.FileId);
                foreach(var line in fileLines)
                {
                    var headerOrDetail = line.Line.Substring(headerOrDetailPosition, 1);
                    switch (headerOrDetail)
                    {
                        case "0":
                            header = line.Line;
                            break;
                        case "1":
                            writeNewDealer(dealers.ToList(), line.Line, fileNameFromRVSDealerCodeStartPosition);
                            break;
                        case "2":
                            writeDetail(dealers.ToList(), line.Line);
                            break;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return true;
        }

        private bool getRvsFile(IRepository repository, ILogger logger, Int32 processRunId)
        {
            IEnumerable<Setting> settings;
            IEnumerable<Dealer> dealers;
            string rvsPathIn;
            string fileNameFromRVS;
            int headerOrDetailPosition;
            int fileNameFromRVSDealerCodeStartPosition;
            try
            {
                settings = repository.Get<Setting>();
                dealers = repository.Get<Dealer>(x => x.IsActive == true);
                localWorkingPath = settings.First(x => x.Key == Constants.I02LocalWorkingPath).Value;
                fileName = settings.First(x => x.Key == Constants.I02FileName).Value;
                rvsPathIn = settings.First(x => x.Key == Constants.RVSPathIn).Value;
                fileNameFromRVS = settings.First(x => x.Key == Constants.I02OrderReplenishmentFileNameFromRVS).Value;
                headerOrDetailPosition = int.Parse(settings.First(x => x.Key == Constants.I02FileNameFromRVSHeaderOrDetailPosition).Value);
                fileNameFromRVSDealerCodeStartPosition = int.Parse(settings.First(x => x.Key == Constants.I02FileNameFromRVSDealerCodeStartPosition).Value);
            }
            catch (DbUpdateException ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Error loading settings from DB: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            catch (Exception ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Error loading settings: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }

            var fullFileNameFromRvs = rvsPathIn + fileNameFromRVS;

            try
            {
                foreach (string f in Directory.EnumerateFiles(localWorkingPath, "*" + fileNameFromRVS + "*"))
                    File.Delete(f);
            }
            catch (DirectoryNotFoundException ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Local working directory not found: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            catch (Exception ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Error deleting files from local working directory: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            try
            {
                using (StreamReader sr = File.OpenText(fullFileNameFromRvs)) //Open Consolidated File from Germany that has been renamed above
                {
                    var fileSource = new FileSource { ProcessRunId = processRunId, Name = fileNameFromRVS, Path = rvsPathIn };
                    repository.Create<FileSource>(fileSource);
                    repository.Save();
                    string line = String.Empty; //Initialize the input string variable
                    while ((line = sr.ReadLine()) != null) //Read until end of file
                    {
                        repository.Create<FileLine>(new FileLine { FileSourceId = fileSource.FileId, Line = line, CreateDate = DateTime.Now });
                        repository.Save();
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - {0} not found: {1}", fullFileNameFromRvs, ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            catch(DbUpdateException ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed - Error saving file to DB: {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            catch (Exception ex)
            {
                logger.Log(repository, processRunId, string.Format("getRvsFile() Failed -  {0}", ex.Message), (int)Constants.LogType.ERROR, ex.Message, "", 0);
                return false;
            }
            return true;
        }

        //private FileDestination writeFile(IRepository repository)
        //{

        //}

        private void writeLine(string infoBytes, FileShare fileShare)
        {
            bool nf;
            if (fileShare == FileShare.None)
                nf = true;
            var fs = new FileStream(outputFileName, FileMode.Append, FileAccess.Write, fileShare);
            var info = new UTF8Encoding(true).GetBytes(infoBytes);
            fs.Write(info, 0, info.Length);
            fs.Write(newline, 0, newline.Length);
            fs.Close();
        }

        private void writeNewDealer(List<Dealer> dealers, string line, int fileNameFromRVSDealerCodeStartPosition)
        {
            var pos = dealers.FindIndex(a => a.Code == dealerCode);
            if (pos > -1) //if (DealerCode in dealers.List)
            {
                //If Not firstTime, then write Footer to old file
                if (!firstTime)
                    writeLine("9" + partCount, FileShare.ReadWrite);
            }
            dealerCode = line.Substring(fileNameFromRVSDealerCodeStartPosition, 4); //Set Current DealerCode

            pos = dealers.FindIndex(a => a.Code == dealerCode);
            if (pos > -1) //if (DealerCode in dealers.List)
            {
                firstTime = false; //for the next time
                partCount = 0; // 0 because there is an extra header line
                               //fs = new FileStream(@OutputFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                outputFileName = localWorkingPath + dealerCode + fileName;

                //fs = new FileStream(@OutputFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                //Insert Header = the same line starting with "1" in the source file 
                //Write sHeader here
                writeLine(header, FileShare.ReadWrite);

                //Write "1" header
                writeLine(line, FileShare.ReadWrite);
            }

            previousDealerCode = dealerCode;
        }

        private void writeDetail(List<Dealer> dealers, string line)
        {
            var pos = dealers.FindIndex(a => a.Code == dealerCode);

            if (pos > -1) //if (DealerCode in dealers.List)
            {
                writeLine(line, FileShare.None);
                partCount++;
            }

            previousDealerCode = dealerCode;
        }

        //public void OldRunProcess(IRepository repository, ILogger logger)
        //{
        //    var settings = repository.Get<Setting>();
        //    var dealers = repository.Get<Dealer>(x => x.IsActive == true);

        //    localWorkingPath = settings.First(x => x.Key == Constants.I02LocalWorkingPath).Value;
        //    fileName = settings.First(x => x.Key == Constants.I02FileName).Value;
        //    var rvsPathIn = settings.First(x => x.Key == Constants.RVSPathIn).Value;
        //    var fileNameFromRVS = settings.First(x => x.Key == Constants.I02OrderReplenishmentFileNameFromRVS).Value;
        //    var headerOrDetailPosition = int.Parse(settings.First(x => x.Key == Constants.I02FileNameFromRVSHeaderOrDetailPosition).Value);
        //    var fileNameFromRVSDealerCodeStartPosition = int.Parse(settings.First(x => x.Key == Constants.I02FileNameFromRVSDealerCodeStartPosition).Value);

        //    var fullFileNameFromRvs = rvsPathIn + fileNameFromRVS;

        //    foreach (string f in Directory.EnumerateFiles(localWorkingPath, "*" + fullFileNameFromRvs + "*"))
        //        File.Delete(f);

        //    using (StreamReader sr = File.OpenText(fullFileNameFromRvs)) //Open Consolidated File from Germany that has been renamed above
        //    {
        //        string line = String.Empty; //Initialize the input string variable
        //        while ((line = sr.ReadLine()) != null) //Read until end of file
        //        {
        //            var headerOrDetail = line.Substring(headerOrDetailPosition, 1);
        //            switch (headerOrDetail)
        //            {
        //                case "1":
        //                    doSomething1(dealers.ToList(), line, fileNameFromRVSDealerCodeStartPosition);
        //                    break;
        //                case "2":
        //                    doSomething2(dealers.ToList(), line);
        //                    break;
        //                case "0":
        //                    sHeader = line;
        //                    break;
        //            }
        //        }
        //        //Insert Footer
        //        var fs = new FileStream(outputFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        //        var info = new UTF8Encoding(true).GetBytes("9" + partCount);
        //        fs.Write(info, 0, info.Length);
        //        fs.Write(newline, 0, newline.Length);
        //        fs.Close();
        //    }

        //    File.Move(fullFileNameFromRvs, fullFileNameFromRvs + "." + DateTime.Now.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss"));
        //}

        //private void writeToFile(string infoBytes, FileShare fileShare)
        //{
        //    var fs = new FileStream(outputFileName, FileMode.Append, FileAccess.Write, fileShare);
        //    var info = new UTF8Encoding(true).GetBytes(infoBytes);
        //    fs.Write(info, 0, info.Length);
        //    fs.Write(newline, 0, newline.Length);
        //    fs.Close();
        //}

        //private void doSomething1(List<Dealer> dealers, string line, int fileNameFromRVSDealerCodeStartPosition)
        //{
        //    var pos = dealers.FindIndex(a => a.Code == dealerCode);
        //    if (pos > -1) //if (DealerCode in dealers.List)
        //    {
        //        //If Not firstTime, then write Footer to old file
        //        if (!firstTime)
        //            writeToFile("9" + partCount, FileShare.ReadWrite);
        //    }
        //    dealerCode = line.Substring(fileNameFromRVSDealerCodeStartPosition, 4); //Set Current DealerCode

        //    pos = dealers.FindIndex(a => a.Code == dealerCode);
        //    if (pos > -1) //if (DealerCode in dealers.List)
        //    {
        //        firstTime = false; //for the next time
        //        partCount = 0; // 0 because there is an extra header line
        //                       //fs = new FileStream(@OutputFileName, FileMode.Create, FileAccess.Write, FileShare.None);
        //        outputFileName = localWorkingPath + dealerCode + fileName;

        //        //fs = new FileStream(@OutputFileName, FileMode.Create, FileAccess.Write, FileShare.None);
        //        //Insert Header = the same line starting with "1" in the source file 
        //        //Write sHeader here
        //        writeToFile(sHeader, FileShare.ReadWrite);

        //        //Write "1" header
        //        writeToFile(line, FileShare.ReadWrite);
        //    }

        //    previousDealerCode = dealerCode;
        //}

        //private void doSomething2(List<Dealer> dealers, string line)
        //{
        //    var pos = dealers.FindIndex(a => a.Code == dealerCode);

        //    if (pos > -1) //if (DealerCode in dealers.List)
        //    {
        //        writeToFile(line, FileShare.None);
        //        partCount++;
        //    }

        //    previousDealerCode = dealerCode;
        //}
    }
}
