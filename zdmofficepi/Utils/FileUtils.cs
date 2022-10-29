﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using zdmofficepi.DataAccess;
using zdmofficepi.Models;

namespace zdmofficepi.Utils
{
    public class FileUtils
    {
        UnitofWork unitOfWork;
        private const string FTP_URL = "ftp://zdmapi.armsteknoloji.com";
        private const string FTP_USERNAME = "u0584616";
        private const string FTP_PASSWORD = "5^k30nbC";
        private const string FTP_FOLDERNAME = "ZDMOffice";
        private readonly ApplicationDBContext _context;

        public FileUtils(ApplicationDBContext context)
        {
            _context = context;
            unitOfWork = new UnitofWork(_context);
        }

        public bool UploadFile(FileModel model)
        {
            try
            {
                if (!FtpDirectoryExists(model.Filefolder))
                {
                    Makefolder(model.Filefolder);
                }
                string URL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/{model.File.FileName}";
                var request = (FtpWebRequest)WebRequest.Create(URL);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                byte[] buffer = new byte[1024];
                var stream = model.File.OpenReadStream();
                byte[] fileContents;
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    fileContents = ms.ToArray();
                }
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public byte[] GetFile(FileModel model)
        {
            string URL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/{model.Filename}";
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                byte[] fileData = request.DownloadData(URL);
                var stream = new MemoryStream(fileData);

                IFormFile file = new FormFile(stream, 0, fileData.Length, model.Filename, model.Filename)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = model.Filetype
                };
                return fileData;
            }
        }

        public bool DeleteFile(FileModel model)
        {
            try
            {
                string URL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/{model.Filename}";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(URL);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    response.Close();
                    string FOLDERURL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/";
                    FtpWebRequest folderrequest = (FtpWebRequest)WebRequest.Create(URL);
                    folderrequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                    folderrequest.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                    using (FtpWebResponse folderresponse = (FtpWebResponse)request.GetResponse())
                    {
                        folderresponse.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool FtpDirectoryExists(string directory)
        {
            try
            {
                List<string> directroys = new List<string>();
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{FTP_URL}/{FTP_FOLDERNAME}/{directory}");
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();
                reader.Close();
                response.Close();
                directroys = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool Makefolder(string folder)
        {
            bool iscreated = false;
            try
            {
                WebRequest request = WebRequest.Create($"{FTP_URL}/{FTP_FOLDERNAME}/{folder}");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    iscreated = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return iscreated;

        }
    }
}