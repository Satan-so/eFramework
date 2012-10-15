using System;

namespace GHY.EF.Admin
{
    public partial class error : System.Web.UI.Page
    {
        protected string errorMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Server.GetLastError() != null)
            {
                this.errorMessage = Server.GetLastError().Message;
            }
        }
    }
}