using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

public class FTP
{
    private string FTPServerLocation = string.Empty;
    private string FTPPort = string.Empty;
    private string UserID = string.Empty;
    private string Password = string.Empty;
    private FtpWebRequest FtpRequest;
    private FtpWebResponse FtpResponse;
    private Stream FtpStream;
    private int BufferSize = 2048;

    public FTP(string ftpServer, string ftpPort, string userID, string password)
    {
        this.FTPServerLocation = ftpServer;
        this.FTPPort = ftpPort;
        this.UserID = userID;
        this.Password = password;
    }

    private void Connect(string filePath)
    {
        FtpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(filePath));
        FtpRequest.Credentials = new NetworkCredential(UserID, Password);
        FtpRequest.UseBinary = true;
        FtpRequest.UsePassive = true;
        FtpRequest.KeepAlive = false;
    }

    public string[] ListDirectory(string directory)
    {
        List<string> fileList = new List<string>();
        try
        {
            Connect("ftp://" + FTPServerLocation + ":" + FTPPort + "/" + directory);

            FtpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
            FtpStream = FtpResponse.GetResponseStream();
            StreamReader ftpReader = new StreamReader(FtpStream);

            while (ftpReader.Peek() != -1)
            {
                fileList.Add(ftpReader.ReadLine());
            }

            ftpReader.Close();
            FtpStream.Close();
            FtpResponse.Close();
            FtpRequest = null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return fileList.ToArray();
    }

    public void DownloadFile(string remoteFile, string localFile)
    {
        try
        {
            Connect("ftp://" + FTPServerLocation + ":" + FTPPort + "/" + remoteFile);
            FtpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
            FtpStream = FtpResponse.GetResponseStream();

            /*
            if (localFile.LastIndexOf('/') == localFile.Length)
                localFile = localFile + Path.GetFileName(remoteFile);
            else
                localFile = localFile + "/" + Path.GetFileName(remoteFile);
            */

            FileStream fileStream = new FileStream(localFile, FileMode.Create);

            byte[] buffer = new byte[BufferSize];

            int readBytes = FtpStream.Read(buffer, 0, BufferSize);         

            while (readBytes > 0)
            {
                fileStream.Write(buffer, 0, readBytes);
                readBytes = FtpStream.Read(buffer, 0, BufferSize);
            }

            fileStream.Flush();
            fileStream.Close();
            FtpStream.Close();
            FtpResponse.Close();
            FtpRequest = null;
        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);
        }
    }

    public void UploadFile(string remoteFile, string localFile)
    {
        try
        {
            Connect("ftp://" + FTPServerLocation + ":" + FTPPort + "/" + remoteFile);

            FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
            FtpStream = FtpResponse.GetResponseStream();

            FileStream fileStream = new FileStream(localFile, FileMode.Create);

            byte[] buffer = new byte[BufferSize];

            int readBytes = fileStream.Read(buffer, 0, BufferSize);

            while (readBytes > 0)
            {
                FtpStream.Write(buffer, 0, readBytes);
                readBytes = fileStream.Read(buffer, 0, BufferSize);
            }

            FtpStream.Flush();
            FtpStream.Close();
            fileStream.Close();            
            FtpResponse.Close();
            FtpRequest = null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}

