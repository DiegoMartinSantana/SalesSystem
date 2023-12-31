﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using Access;
using AccessBd;
using System.Security.Cryptography.X509Certificates;
namespace SalesSystem
{
    public partial class _Default : Page
    {
        //the list has to be a property of the page to be accessed from the aspx
        public List<Article> list { get; set; }
        public int IdArticle { get; set; }
        public void refreshList()
        {
            ArticleAccess access = new ArticleAccess();
            list = access.listArticle();
            Session.Add("listarticle", list);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                refreshList();
            }


            if (Request.QueryString["id"] != null)
            {
                FavoritesAccess fAccess = new FavoritesAccess();
                Users user = (Users)Session["user"];
                int idart = int.Parse(Request.QueryString["id"].ToString());
                fAccess.AddFavorite(user.Id, idart);

            }
        }

        protected void chkfilter_CheckedChanged(object sender, EventArgs e)
        {
            txtsearch.Enabled = !chkfilter.Checked;

            ArticleAccess access = new ArticleAccess();
            list = access.listArticle();
        }
        protected void txtsearch_TextChanged(object sender, EventArgs e)
        {
            list = (List<Article>)Session["listarticle"];
            list = list.FindAll(x => x.Name.ToUpper().Contains(txtsearch.Text.ToUpper()) ||
            x.Description.ToUpper().Contains(txtsearch.Text.ToUpper()));


        }

        protected void ddlby_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlcritery.Items.Clear();

            if (ddlby.SelectedItem.ToString() == "Price")
            {
                ddlcritery.Items.Add("Less than : ");
                ddlcritery.Items.Add("Equals to : ");
                ddlcritery.Items.Add("More than : ");

            }
            else
            {
                ddlcritery.Items.Add("Starts with : ");
                ddlcritery.Items.Add("Ends with : ");
                ddlcritery.Items.Add("Contains : ");

            }

        }

        protected void btnsearchAdvanced_Click(object sender, EventArgs e)
        {
            try
            {
                ArticleAccess access = new ArticleAccess();
                list = access.filter(ddlby.SelectedItem.ToString(), ddlcritery.SelectedItem.ToString(), txtFilterAdvanced.Text);


            }
            catch (Exception ex)
            {

                throw ex;
            }



        }


       
    }
}