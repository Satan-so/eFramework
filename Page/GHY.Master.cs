using System;
using System.Web.UI;
using GHY.EF.Core.Info;

namespace GHY.EF.Page
{
    /// <summary>
    /// GHY页面模板。
    /// </summary>
    public partial class GHY : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                BindRepeater(this);
            }
        }

        void BindRepeater(Control control)
        {
            if (control is InfoRepeater)
            {
                InfoRepeater repeater = control as InfoRepeater;

                if (repeater.Count <= 0)
                {
                    repeater.DataBind();
                }

                return;
            }

            foreach (Control childControl in control.Controls)
            {
                this.BindRepeater(childControl);
            }
        }
    }
}