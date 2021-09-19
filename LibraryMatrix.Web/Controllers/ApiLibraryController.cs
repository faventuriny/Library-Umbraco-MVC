using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace LibraryMatrix.Web.Controllers
{
    public class ApiLibraryController : UmbracoApiController
    {
        //http://localhost:63702/umbraco/api/ApiLibrary/GetAllBooks
        [HttpGet]
        public IHttpActionResult  GetAllBooks()
        {
            var list = GetBooks();
            return Json(list);   
        }

        //http://localhost:63702/umbraco/api/ApiLibrary/GetBooksAndNewspappers
        [HttpGet]
        public IHttpActionResult GetBooksAndNewspappers()
        {
            var books = GetBooks();
            var newspappers = GetNewspappers();

            var list = new List<List<Dictionary<string, string>>> { };
            list.Add(books);
            list.Add(newspappers);

            return Json(list);
        }

        // Get books from umbraco
        private List<Dictionary<string, string>> GetBooks()
        {
            var books = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("books").FirstOrDefault();
            var booksChildren = books.Children;

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>> { };
            foreach (var book in booksChildren)
            {
                var obj = new Dictionary<string, string>();

                obj.Add("Title", book.Name.ToString());
                obj.Add("FullTitle", book.Value("title").ToString());
                obj.Add("CatalogNumber", book.Value("catalogNumber").ToString());
                obj.Add("Author", book.Value("author").ToString());
                obj.Add("Description", book.Value("description").ToString());
                obj.Add("PublishedDate", book.Value("publishedDate").ToString());
                obj.Add("ImgUrl", "http://localhost:63702" + book.GetCropUrl("coverPicture", "bookCropper"));

                list.Add(obj);
            }
            return list;
        }
        // Get newspapper from umbraco
        private List<Dictionary<string, string>> GetNewspappers()
        {
            var newspappers = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("newspapers").FirstOrDefault();
            var newspappersChildren = newspappers.Children;

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>> { };
            foreach (var newspapper in newspappersChildren)
            {
                var obj = new Dictionary<string, string>();

                obj.Add("Title", newspapper.Value("title").ToString());
                obj.Add("CatalogNumber", newspapper.Value("catalogNumber").ToString());
                obj.Add("Author", newspapper.Value("author").ToString());
                obj.Add("Description", newspapper.Value("description").ToString());
                obj.Add("NewspaperDate", newspapper.Value("newspaperDate").ToString());
                obj.Add("ExternalLink", newspapper.Value("externalLink").ToString());

                list.Add(obj);
            }

            return list;
        }
    }
}