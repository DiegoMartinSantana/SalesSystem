﻿using Access;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccessBd
{
    public class ArticleAccess
    {

        public void deleteArticle(int id)
        {
            BdAccess access = new BdAccess();
            try
            {
                access.setConsultation("delete Articulos where Id=@id");
                access.setParameter("@id", id);
                access.executeAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                access.close();
            }



        }
        public void editArticle(Article art)
        {
            BdAccess access = new BdAccess();

            try
            {
              
                access.setConsultation("Update  ARTICULOS set Codigo = @cod, Nombre = @nom , Descripcion = @desc, IdMarca=@idmar,IdCategoria = @idcat, ImagenUrl = @imgurl,Precio = @pre WHERE Id=@id");
                access.setParameter("@cod", art.CodArticle);
                access.setParameter("@nom", art.Name);
                access.setParameter("@desc", art.Description);
                access.setParameter("@idmar", art.brand.Id);
                access.setParameter("@idcat", art.category.Id);
                if (string.IsNullOrEmpty(art.UrlImg))
                {
                    access.setParameter("@imgurl", DBNull.Value);
                }
                else
                {
                    access.setParameter("@imgurl", art.UrlImg);

                }
                access.setParameter("@pre", art.Price);
                access.setParameter("@id", art.Id);
                access.executeAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                access.close();
            }



        }
        public Article returnById(int id)
        {

            BdAccess data = new BdAccess();
            //I bring everything and then I use what I want
            try
            {


                data.setConsultation("select  A.Codigo, Nombre, A.Descripcion artDesc, M.Descripcion DescMarca,C.Descripcion Catdesc,ImagenUrl,Precio , A.Id Idart, IdCategoria,IdMarca from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id=A.IdMarca and C.Id= A.IdCategoria   and A.Id = @Idarticulo");
                data.setParameter("@Idarticulo", id);
                data.executeRead();
                Article a = new Article();

                while (data.reader.Read())
                {
                    a.Id = id;
                    a.Name = (string)data.reader["Nombre"];
                    a.Price = (decimal)data.reader["Precio"];

                    a.category = new Category();
                    a.brand = new Brand();
                    a.brand.Id = (int)data.reader["IdMarca"];
                    a.brand.Description = (string)data.reader["DescMarca"];

                    a.category.Id = (int)data.reader["IdCategoria"];

                    a.category.Description = (string)data.reader["CatDesc"];

                    a.CodArticle = (string)data.reader["Codigo"];
                    a.Description = (string)data.reader["artDesc"];
                    if (!(data.reader["ImagenUrl"] is DBNull))
                    {
                        a.UrlImg = (string)data.reader["ImagenUrl"];
                    }
                }
                return a;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                data.close();
            }
        }
        public List<Article> listArticle(string id = "")
        {
            BdAccess data = new BdAccess();
            List<Article> list = new List<Article>();
            //I bring everything and then I use what I want
            try
            {
                string querylist = " select  A.Codigo, Nombre, A.Descripcion artDesc, M.Descripcion DescMarca,C.Descripcion Catdesc,ImagenUrl,Precio , A.Id Idart, IdCategoria,IdMarca from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id=A.IdMarca and C.Id= A.IdCategoria";
                if (id != "")
                {
                    querylist += " and A.id = " + id;
                }
                data.setConsultation(querylist);

                data.executeRead();

                while (data.reader.Read())
                {
                    Article a = new Article();
                    a.Id = (int)data.reader["Idart"];
                    a.Name = (string)data.reader["Nombre"];
                    a.Price = (decimal)data.reader["Precio"];

                    a.category = new Category();
                    a.brand = new Brand();
                    a.brand.Id = (int)data.reader["IdMarca"];
                    a.brand.Description = (string)data.reader["DescMarca"];

                    a.category.Id = (int)data.reader["IdCategoria"];

                    a.category.Description = (string)data.reader["CatDesc"];

                    a.CodArticle = (string)data.reader["Codigo"];
                    a.Description = (string)data.reader["artDesc"];
                    if (!(data.reader["ImagenUrl"] is DBNull))
                    {
                        a.UrlImg = (string)data.reader["ImagenUrl"];
                    }

                    list.Add(a);

                }
                data.close();

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                data.close();
            }
        }

        public List<Article> filter(string by, string critery, string filter)
        {
            List<Article> list = new List<Article>();
            BdAccess access = new BdAccess();

            try
            {
                string query = "select A.Id Idart, A.Codigo, Nombre, A.Descripcion artDesc, IdMarca, IdCategoria, ImagenURL ,Precio,C.Descripcion catDesc ,M.Descripcion marcaDesc From ARTICULOS A, MARCAS M, CATEGORIAS C where A.idMarca = M.id and C.Id = A.IdCategoria and ";

                if (by == "Price")
                {
                    switch (critery)
                    {

                        case "Less than : ":
                            query += "Precio < " + filter;
                            break;
                        case "Equals to : ":
                            query += "Precio = " + filter;

                            break;
                        case "More than : ":
                            query += "Precio > " + filter;

                            break;

                    }
                }

                if (by == "Brand")
                {
                    switch (critery)
                    {
                        case "Starts with : ":
                            query += "M.Descripcion like '" + filter + "%' ";

                            break;
                        case "Ends with : ":
                            query += "M.Descripcion like '%" + filter + "'";

                            break;
                        case "Contains : ":
                            query += "M.Descripcion like '%" + filter + "%'";


                            break;

                    }
                }
                if (by == "Category")
                {
                    switch (critery)
                    {
                        case "Starts with : ":
                            query += "C.Descripcion like '" + filter + "%'";

                            break;
                        case "Ends with : ":
                            query += "C.Descripcion like '%" + filter + "'";

                            break;
                        case "Contains : ":
                            query += "C.Descripcion like '%" + filter + "%'";

                            break;

                    }
                }

                access.setConsultation(query);
                access.executeRead();

                while (access.reader.Read())
                {
                    Article a = new Article();


                    a.Id = (int)access.reader["Idart"];
                    a.Name = (string)access.reader["Nombre"];
                    a.Price = (decimal)access.reader["Precio"];

                    a.category = new Category();
                    a.brand = new Brand();
                    a.brand.Id = (int)access.reader["IdMarca"];
                    a.brand.Description = (string)access.reader["marcaDesc"];

                    a.category.Id = (int)access.reader["IdCategoria"];

                    a.category.Description = (string)access.reader["catDesc"];

                    a.CodArticle = (string)access.reader["Codigo"];
                    a.Description = (string)access.reader["artDesc"];
                    if (!(access.reader["ImagenURL"] is DBNull))
                    {
                        a.UrlImg = (string)access.reader["ImagenURL"];
                    }

                    list.Add(a);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                access.close();
            }




        }

        public void addArticle(Article art)
        {

            BdAccess access = new BdAccess();
            try
            {
                access.setConsultation("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria,ImagenUrl,Precio) values (@cod,@nom,@desc,@idmar,@idcat,@imgurl,@pre)");
                access.setParameter("@cod", art.CodArticle);
                access.setParameter("@nom", art.Name);
                access.setParameter("@desc", art.Description);
                access.setParameter("@idmar", art.brand.Id);
                access.setParameter("@idcat", art.category.Id);
                if (string.IsNullOrEmpty(art.UrlImg))
                {
                    access.setParameter("@imgurl", DBNull.Value);
                }
                else
                {
                    access.setParameter("@imgurl", art.UrlImg);

                }
                access.setParameter("@pre", art.Price);


                access.executeAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                access.close();
            }

        }
    }
}
