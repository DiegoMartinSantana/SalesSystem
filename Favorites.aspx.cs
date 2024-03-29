﻿using AccessBd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain;
using Security;

namespace SalesSystem
{
    public partial class Favourites : System.Web.UI.Page
    {

        public List<Article> listArt = new List<Article>();
        public bool first = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Validation.Login(Session["user"]))
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["idDel"] != null)
                {
                    FavoritesAccess favoritesAccess = new FavoritesAccess();
                    Users user = (Users)Session["user"];
                    int idDelete = int.Parse(Request.QueryString["idDel"].ToString());
                    favoritesAccess.Delete(user.Id, idDelete);

                }
                ArticleAccess accessArt = new ArticleAccess();
                FavoritesAccess accessFav = new FavoritesAccess();

                try

                {
                    Users user = new Users();
                    user = (Users)Session["user"];

                    List<Favorites> listFav = accessFav.listFav(user.Id);

                    foreach (var itemFav in listFav)
                    {
                        Article article = new Article();

                        article = accessArt.returnById(itemFav.IdArticle);

                        listArt.Add(article);

                    }

                }
                catch (Exception ex)
                {
                    Session.Add("error", ex);
                    Response.Redirect("error.aspx", false);
                }



            }
        }
    }
}