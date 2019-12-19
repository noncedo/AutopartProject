﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class I02UService : II02UService
    {
        public void RunProcess(IRepository repository, IFTP ftp, ILogger logger)
        {
            try
            {
                Boolean runStatus = false;
                String localWorkingPathSubDir = String.Format("{0:yyyyMMddHmmss}", DateTime.Now);

                var process = repository.GetFirst<Process>(x => x.Name == "I02U");

                var processRun = new ProcessRun { ProcessId = process.ProcessId, Start = DateTime.Now };

                repository.Create<ProcessRun>(processRun);
                repository.Save();

                logger.Log(repository, processRun.ProcessRunId, "Update Order Replenishment Process Start");
                logger.Log(repository, processRun.ProcessRunId, "Fetching FTP file(s)");

                if (GetFTPFiles(repository, logger, ftp, processRun.ProcessRunId, localWorkingPathSubDir) == true)
                {
                    logger.Log(repository, processRun.ProcessRunId, "Loading FTP file(s) into SQL Server");
                    if (LoadFTPFilesIntoDB(repository, logger, processRun.ProcessRunId, localWorkingPathSubDir) == true)
                    {

                        logger.Log(repository, processRun.ProcessRunId, "Creating consolidated file");
                        if (CreateConsolidatedI02UFile(repository, logger, processRun.ProcessRunId) == true)
                        {
                            logger.Log(repository, processRun.ProcessRunId, "Update Order Replenishment Process completed successfully");
                            runStatus = true;
                        }
                    }
                }

                processRun.End = DateTime.Now;
                processRun.RunStatus = runStatus;

                repository.Update<ProcessRun>(processRun);
                repository.Save();
            }
            catch (Exception e)
            {
                logger.Log(repository, 0, "RunProcess() failed", 3, e.Message);
            }
        }

        private Boolean GetFTPFiles(IRepository repository, ILogger logger, IFTP ftp, Int32 processRunId, String localWorkingPathSubDir)
        {
            Boolean filesExist = false;

            try
            {
                var dealerGroups = repository.Get<DealerGroup>();
                var settings = repository.Get<Setting>();

                var ftpHost = settings.First(x => x.Key == Constants.FtpHost).Value;
                var ftpUsername = settings.First(x => x.Key == Constants.FtpUsername).Value;
                var ftpPassword = settings.First(x => x.Key == Constants.FtpPassword).Value;

                var sourcePath = settings.First(x => x.Key == Constants.DMSourcePath).Value;
                var destinationPath = settings.First(x => x.Key == Constants.DMDestinationPath).Value;
                var sourceFilename = settings.First(x => x.Key == Constants.I02UOrderReplenishmentFileNameFromDealer).Value;
                var destinationFilename = settings.First(x => x.Key == Constants.DMDestinationFilename).Value;
                var localWorkingPath = settings.First(x => x.Key == Constants.DMLocalWorkingPath).Value;

                foreach (var dealerGroup in dealerGroups)
                {
                    foreach (var dealer in dealerGroup.Dealers)
                    {
                        String filename = String.Format("{0}", sourceFilename);
                        String remoteFile = String.Format("{0}/{1}/N{2}/{3}", sourcePath, dealerGroup.Code, dealer.Code, filename);
                        String remoteFilePath = String.Format("{0}/{1}/N{2}", sourcePath, dealerGroup.Code, dealer.Code);
                        String localFile = String.Format(@"{0}{1}\{2}{3}", localWorkingPath, localWorkingPathSubDir, dealer.Code, filename);

                        System.IO.Directory.CreateDirectory(String.Format(@"{0}{1}", localWorkingPath, localWorkingPathSubDir));

                        if (ftp.Get(ftpHost, ftpUsername, ftpPassword, remoteFile, localFile) == true)
                        {
                            filesExist = true;

                            logger.Log(repository, processRunId, "Fetching " + remoteFile);

                            String remoteFileRename = String.Format("{0}.{1:yyyyMMdd}.{2:Hmmss}", filename, DateTime.Now, DateTime.Now);
                            //ftp.Rename(ftpHost, ftpUsername, ftpPassword, remoteFile, remoteFileRename);

                            var file = new DataAccess.FileSource { Path = remoteFilePath, Name = String.Format("{0}{1}", dealer.Code, filename), ProcessRunId = processRunId };
                            repository.Create<DataAccess.FileSource>(file);
                            repository.Save();
                        }
                        else if (ftp.Get(ftpHost, ftpUsername, ftpPassword, remoteFile + ".txt", localFile) == true)
                        {
                            filesExist = true;

                            logger.Log(repository, processRunId, "Fetching " + remoteFile);

                            String remoteFileRename = String.Format("{0}.{1:yyyyMMdd}.{2:Hmmss}", filename, DateTime.Now, DateTime.Now);
                            //ftp.Rename(ftpHost, ftpUsername, ftpPassword, remoteFile, remoteFileRename);

                            var file = new DataAccess.FileSource { Path = remoteFilePath, Name = String.Format("{0}{1}", dealer.Code, filename), ProcessRunId = processRunId };
                            repository.Create<DataAccess.FileSource>(file);
                            repository.Save();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(repository, processRunId, "GetFTPFiles() failed", 3, e.Message);
                return false;
            }

            if (filesExist == false)
            {
                logger.Log(repository, processRunId, "No FTP Files to Process");
            }

            return filesExist;
        }

        private Boolean LoadFTPFilesIntoDB(IRepository repository, ILogger logger, Int32 processRunId, String localWorkingPathSubDir)
        {
            try
            {
                var process = repository.GetFirst<Process>(x => x.Name == "I02U");
                var settings = repository.Get<Setting>();
                var localWorkingPath = settings.First(x => x.Key == Constants.DMLocalWorkingPath).Value;
                var fileSources = repository.Get<FileSource>(x => x.ProcessRunId == processRunId);

                foreach (var fileSource in fileSources)
                {
                    var fileLines = repository.Get<FileLine>(x => x.FileSourceId == fileSource.FileId);

                    logger.Log(repository, processRunId, "Loading File " + fileSource.Name);

                    using (StreamReader reader = new StreamReader(localWorkingPath + localWorkingPathSubDir + "\\" + fileSource.Name))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var fileLine = new FileLine { FileSourceId = fileSource.FileId, Line = line, CreateDate = DateTime.Now };
                            repository.Create<FileLine>(fileLine);
                            repository.Save();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(repository, processRunId, "LoadFTPFilesIntoDB() failed", 3, e.Message);
                return false;
            }
            return true;
        }

        private Boolean CreateConsolidatedI02UFile(IRepository repository, ILogger logger, Int32 processRunId)
        {
            try
            {
                var settings = repository.Get<Setting>();
                StreamWriter w = null;

                var rvsPathOut = settings.First(x => x.Key == Constants.RVSPathOut).Value;
                var mvsPath = settings.First(x => x.Key == Constants.MVSPath).Value;
                var mvsExt = settings.First(x => x.Key == Constants.MVSExt).Value;
                var destinationPath = settings.First(x => x.Key == Constants.DMDestinationPath).Value;
                var destinationFilename = settings.First(x => x.Key == Constants.I02UOrderReplenishmentFileNameToRVS).Value;

                var fileSources = repository.Get<FileSource>(x => x.ProcessRunId == processRunId);

                var fileDestination = new DataAccess.FileDestination { Path = destinationPath, Name = destinationFilename, ProcessRunId = processRunId };
                repository.Create<DataAccess.FileDestination>(fileDestination);
                repository.Save();

                try
                {
                    File.Move(rvsPathOut + destinationFilename, rvsPathOut + destinationFilename + "." + DateTime.Now.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss")); //Backup old file, overwrite = true
                    File.Delete(rvsPathOut + destinationFilename);
                }
                catch { }

                using (StreamWriter header = File.AppendText(destinationPath + destinationFilename))
                {
                    header.WriteLine("0 Header");
                }

                foreach (var fileSource in fileSources)
                {
                    var fileLines = repository.Get<FileLine>(x => x.FileSourceId == fileSource.FileId);

                    foreach (var fileLine in fileLines)
                    {
                        try
                        {
                            //Consolidate by appending to main file
                            using (w = System.IO.File.AppendText(destinationPath + destinationFilename))
                            {
                                if (fileLine.Line.Substring(0, 1) != "0" && fileLine.Line.Substring(0, 1) != "9")
                                {
                                    w.WriteLine(fileLine.Line);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            logger.Log(repository, processRunId, "CreateConsolidatedI02UFile() failed ", 3, ex.Message);
                            Console.WriteLine(ex.Message);
                        }

                        fileLine.FileDestinationId = fileDestination.FileId;
                    }
                }

                w.Close();

                using (StreamWriter footer = File.AppendText(destinationPath + destinationFilename))
                {
                    footer.WriteLine("9 Footer");
                }

                try
                {
                    File.Move(mvsPath + destinationFilename + mvsExt, mvsPath + destinationFilename + mvsExt + "." + DateTime.Now.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss"));
                }
                catch { }

                File.Copy(destinationPath + destinationFilename, mvsPath + destinationFilename + mvsExt);
            }
            catch (Exception e)
            {
                logger.Log(repository, processRunId, "CreateConsolidatedI0U2File() failed ", 3, e.Message);
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}
