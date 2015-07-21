using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;

namespace SharpUpdate
{
    internal class SharpUpdateXml
    {
        private Version version;
        private Uri uri;
        private string fileName;
        private string md5;
        private string description;
        private string lunchArgs;

        internal Version Version
        {
            get { return this.version; }
        }

        internal Uri Uri
        {
            get { return this.uri; }
        }

        internal string  FileName
        {
            get { return this.fileName; }
        }

        internal string MD5
        {
            get { return this.md5; }
        }

        internal string Description
        {
            get { return this.description; }
        }

        internal string LunchArgs
        {
            get { return this.lunchArgs; }
        }

        internal SharpUpdateXml (Version version,Uri uri,string fileName,string md5,string description,string lunchArgs)
        {
            this.version = version;
            this.uri = uri;
            this.fileName = fileName;
            this.md5 = md5;
            this.description = description;
            this.lunchArgs = lunchArgs;

        }

        internal bool IsNewerThan(Version version)
        {
            return this.version > version;
        }

        internal static bool ExistOnServer(Uri location)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest) WebRequest.Create(location.AbsoluteUri);
                HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
                resp.Close();

                return resp.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static SharpUpdateXml Parse(Uri location, string appID)
        {
            Version version = null;
            string url = "", fileName = "", md5 = "", description = "", launchArgs = "";

            try
            {
                XDocument doc = XDocument.Load(location.AbsoluteUri);
                XElement el = doc.Element("update");

                if(el == null) return null;

                version = Version.Parse(el.Element("version").Value);
                url = el.Element("url").Value;
                fileName = el.Element("fileName").Value;
                md5 = el.Element("md5").Value;
                description = el.Element("description").Value;
                launchArgs = el.Element("launchArgs").Value;

                return new SharpUpdateXml(version, new Uri(url), fileName, md5, description, launchArgs);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }                            
}                                
