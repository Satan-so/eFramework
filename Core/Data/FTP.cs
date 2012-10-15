using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GHY.EF.Core.Data
{
    /// <summary>
    /// FTP操作工具类。
    /// 务必使用using体保证资源销毁。
    /// </summary>
    public class FTP : IDisposable
    {
        #region 私有字段
        bool logined = false;

        string mes, reply;
        int bytes, retValue;
        Socket clientSocket;

        static int BLOCK_SIZE = 1024;
        Byte[] buffer = new Byte[BLOCK_SIZE];
        Encoding ASCII = Encoding.ASCII;
        #endregion

        #region 私有方法
        void ReadReply()
        {
            mes = string.Empty;
            reply = ReadLine();
            retValue = Int32.Parse(reply.Substring(0, 3));
        }

        private void Cleanup()
        {
            if (this.clientSocket != null)
            {
                this.clientSocket.Close();
                this.clientSocket = null;
            }

            this.logined = false;
        }

        private string ReadLine()
        {
            while (true)
            {
                this.bytes = this.clientSocket.Receive(this.buffer, this.buffer.Length, 0);
                this.mes += Encoding.Default.GetString(this.buffer, 0, this.bytes);

                if (this.bytes < this.buffer.Length)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            if (mes.Length > 2)
            {
                mes = mess[mess.Length - 2];
            }
            else
            {
                mes = mess[0];
            }

            if (!mes.Substring(3, 1).Equals(" "))
            {
                return ReadLine();
            }

            return mes;
        }

        private void SendCommand(String command)
        {
            Byte[] cmdBytes = Encoding.Default.GetBytes((command + "\r\n").ToCharArray());
            this.clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
            this.ReadReply();
        }

        private Socket CreateDataSocket()
        {
            this.SendCommand("PASV");

            if (this.retValue != 227)
            {
                throw new IOException(this.reply.Substring(4));
            }

            int index1 = this.reply.IndexOf('(');
            int index2 = this.reply.IndexOf(')');
            string ipData = this.reply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];

            int len = ipData.Length;
            int partCount = 0;
            string buf = string.Empty;

            for (int i = 0; i < len && partCount <= 6; i++)
            {

                char ch = Char.Parse(ipData.Substring(i, 1));

                if (Char.IsDigit(ch))
                {
                    buf += ch;
                }
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV reply: " + this.reply);
                }

                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = string.Empty;
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV reply: " + this.reply);
                    }
                }
            }

            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];

            int port = (parts[4] << 8) + parts[5];

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(ipAddress).AddressList[0], port);

            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("Can't connect to remote server");
            }

            return s;
        }
        #endregion

        #region 保护方法
        /// <summary>
        /// 释放FTP资源。
        /// </summary>
        /// <param name="disposing">是否清理托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.clientSocket != null)
            {
                this.SendCommand("QUIT");
            }

            this.Cleanup();
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 获取或设置FTP服务器地址。
        /// </summary>
        public string RemoteHost { get; set; }

        /// <summary>
        /// 获取或设置FTP服务器端口号。
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// 获取或设置FTP服务器路径。
        /// </summary>
        public string RemotePath { get; set; }

        /// <summary>
        /// 获取或设置FTP用户名。
        /// </summary>
        public string RemoteUser { get; set; }

        /// <summary>
        /// 获取或设置FTP密码。
        /// </summary>
        public string RemotePassword { get; set; }
        #endregion

        #region 公共方法
        /// <summary>
        /// FTP工具类的构造函数。
        /// </summary>
        public FTP()
        {
            //为属性设置默认值。
            this.RemoteHost = "localhost";
            this.RemotePort = 21;
            this.RemotePath = ".";
            this.RemoteUser = "anonymous";
            this.RemotePassword = "hello1234";
        }

        /// <summary>
        /// 返回目录中的文件列表。
        /// </summary>
        /// <returns>文件列表。</returns>
        public string[] GetFileList()
        {
            return this.GetFileList(string.Empty);
        }

        /// <summary>
        /// 返回目录中的文件列表。
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>文件列表。</returns>
        public string[] GetFileList(string mask)
        {
            if (!this.logined)
            {
                this.Login();
            }

            Socket cSocket = this.CreateDataSocket();

            if (string.IsNullOrEmpty(mask))
            {
                this.SendCommand("LIST ");
            }
            else
            {
                this.SendCommand("NLST " + mask);
            }

            if (!(this.retValue == 150 || this.retValue == 125 || this.retValue == 226))
            {
                throw new IOException(this.reply.Substring(4));
            }
            this.mes = string.Empty;

            while (true)
            {
                int bytes = cSocket.Receive(this.buffer, this.buffer.Length, 0);
                this.mes += Encoding.Default.GetString(this.buffer, 0, this.bytes);

                if (this.bytes < this.buffer.Length)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = this.mes.Split(seperator);

            cSocket.Close();
            return mess;
        }

        /// <summary>
        /// 获取文件的大小。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns>文件的大小。</returns>
        public long GetFileSize(string fileName)
        {

            if (!this.logined)
            {
                this.Login();
            }

            this.SendCommand("SIZE " + fileName);
            long size = 0;

            if (this.retValue == 213)
            {
                size = Int64.Parse(this.reply.Substring(4));
            }
            else
            {
                throw new IOException(this.reply.Substring(4));
            }

            return size;

        }

        /// <summary>
        /// 登录FTP服务器。
        /// </summary>
        public void Login()
        {
            this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(Dns.GetHostEntry(this.RemoteHost).AddressList[0], this.RemotePort);

            this.clientSocket.Connect(ep);

            this.ReadReply();

            if (this.retValue != 220)
            {
                this.Dispose();
                throw new IOException(this.reply.Substring(4));
            }

            this.SendCommand("USER " + this.RemoteUser);

            if (!(this.retValue == 331 || this.retValue == 230))
            {
                this.Cleanup();
                throw new IOException(this.reply.Substring(4));
            }

            if (this.retValue != 230)
            {
                this.SendCommand("PASS " + this.RemotePassword);

                if (!(this.retValue == 230 || this.retValue == 202))
                {
                    this.Cleanup();
                    throw new IOException(this.reply.Substring(4));
                }
            }

            this.logined = true;
            this.chdir(this.RemotePath);
        }

        /// <summary>
        /// 设置编码模式。
        /// </summary>
        /// <param name="mode">二进制模式还是ASCII模式。</param>
        public void SetBinaryMode(bool mode)
        {
            if (mode)
            {
                this.SendCommand("TYPE I");
            }
            else
            {
                this.SendCommand("TYPE A");
            }
            if (this.retValue != 200)
            {
                throw new IOException(this.reply.Substring(4));
            }
        }

        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="remFileName">服务器文件名。</param>
        public void Download(string remFileName)
        {
            this.Download(remFileName, string.Empty, false);
        }

        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="remFileName">服务器文件名。</param>
        /// <param name="resume"></param>
        public void Download(string remFileName, Boolean resume)
        {
            this.Download(remFileName, string.Empty, resume);
        }

        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="remFileName">服务器文件名。</param>
        /// <param name="locFileName">本地文件名。</param>
        public void Download(string remFileName, string locFileName)
        {
            this.Download(remFileName, locFileName, false);
        }

        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="remFileName">服务器文件名。</param>
        /// <param name="locFileName">本地文件名。</param>
        /// <param name="resume"></param>
        public void Download(string remFileName, string locFileName, bool resume)
        {
            if (!this.logined)
            {
                this.Login();
            }

            this.SetBinaryMode(true);

            if (string.IsNullOrEmpty(locFileName))
            {
                locFileName = remFileName;
            }

            if (!File.Exists(locFileName))
            {
                Stream st = File.Create(locFileName);
                st.Close();
            }

            Socket cSocket;

            using (FileStream output = new FileStream(locFileName, FileMode.Open))
            {
                cSocket = CreateDataSocket();
                long offset = 0;

                if (resume)
                {
                    offset = output.Length;

                    if (offset > 0)
                    {
                        this.SendCommand("REST " + offset);

                        if (this.retValue != 350)
                        {
                            offset = 0;
                        }
                    }

                    if (offset > 0)
                    {
                        long npos = output.Seek(offset, SeekOrigin.Begin);
                    }
                }

                this.SendCommand("RETR " + remFileName);

                if (!(retValue == 150 || retValue == 125))
                {
                    throw new IOException(this.reply.Substring(4));
                }

                while (true)
                {
                    this.bytes = cSocket.Receive(this.buffer, buffer.Length, 0);
                    output.Write(this.buffer, 0, this.bytes);

                    if (this.bytes <= 0)
                    {
                        break;
                    }
                }
            }

            if (cSocket.Connected)
            {
                cSocket.Close();
            }

            this.ReadReply();

            if (!(this.retValue == 226 || this.retValue == 250))
            {
                throw new IOException(this.reply.Substring(4));
            }

        }

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        public void Upload(string fileName)
        {
            this.Upload(fileName, false);
        }

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <param name="resume"></param>
        public void Upload(string fileName, bool resume)
        {
            if (!this.logined)
            {
                this.Login();
            }

            Socket cSocket = CreateDataSocket();
            long offset = 0;

            if (resume)
            {
                try
                {
                    this.SetBinaryMode(true);
                    offset = GetFileSize(fileName);
                }
                catch (Exception)
                {
                    offset = 0;
                }
            }

            if (offset > 0)
            {
                this.SendCommand("REST " + offset);

                if (this.retValue != 350)
                {
                    offset = 0;
                }
            }

            this.SendCommand("STOR " + Path.GetFileName(fileName));

            if (!(this.retValue == 125 || this.retValue == 150))
            {
                throw new IOException(this.reply.Substring(4));
            }

            using (FileStream input = new FileStream(fileName, FileMode.Open))
            {

                if (offset != 0)
                {
                    input.Seek(offset, SeekOrigin.Begin);
                }

                while ((this.bytes = input.Read(this.buffer, 0, this.buffer.Length)) > 0)
                {
                    cSocket.Send(this.buffer, this.bytes, 0);
                }
            }

            if (cSocket.Connected)
            {
                cSocket.Close();
            }

            this.ReadReply();

            if (!(this.retValue == 226 || this.retValue == 250))
            {
                throw new IOException(this.reply.Substring(4));
            }
        }

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        public void DeleteRemoteFile(string fileName)
        {
            if (!this.logined)
            {
                this.Login();
            }

            this.SendCommand("DELE " + fileName);

            if (this.retValue != 250)
            {
                throw new IOException(this.reply.Substring(4));
            }

        }

        /// <summary>
        /// 重命名文件。
        /// </summary>
        /// <param name="oldFileName">旧文件名。</param>
        /// <param name="newFileName">新文件名。</param>
        public void RenameRemoteFile(string oldFileName, string newFileName)
        {
            if (!this.logined)
            {
                this.Login();
            }

            this.SendCommand("RNFR " + oldFileName);

            if (this.retValue != 350)
            {
                throw new IOException(reply.Substring(4));
            }

            this.SendCommand("RNTO " + newFileName);

            if (this.retValue != 250)
            {
                throw new IOException(this.reply.Substring(4));
            }
        }

        /// <summary>
        /// 新建文件夹。
        /// </summary>
        /// <param name="dirName">文件夹名。</param>
        public void MkDir(string dirName)
        {
            if (!this.logined)
            {
                this.Login();
            }

            this.SendCommand("MKD " + dirName);

            if (this.retValue != 250)
            {
                throw new IOException(this.reply.Substring(4));
            }
        }

        /// <summary>
        /// 删除文件夹。
        /// </summary>
        /// <param name="dirName">文件夹名。</param>
        public void RmDir(string dirName)
        {
            if (!this.logined)
            {
                this.Login();
            }

            this.SendCommand("RMD " + dirName);

            if (this.retValue != 250)
            {
                throw new IOException(this.reply.Substring(4));
            }

        }

        /// <summary>
        /// 改变当前目录。
        /// </summary>
        /// <param name="dirName">文件夹名。</param>
        public void chdir(string dirName)
        {
            if (dirName.Equals("."))
            {
                return;
            }

            if (!this.logined)
            {
                this.Login();
            }

            this.SendCommand("CWD " + dirName);

            if (this.retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

            this.RemotePath = dirName;

        }

        /// <summary>
        /// 释放FTP资源。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 释放FTP资源。
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// 析构器。
        /// </summary>
        ~FTP()
        {
            this.Dispose(false);
        }
        #endregion
    }
}