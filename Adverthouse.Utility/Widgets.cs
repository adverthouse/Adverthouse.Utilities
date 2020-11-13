using Adverthouse.Common.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Adverthouse.Utility
{
    public static class Widgets
    {
        public static IHtmlContent WidgetTitle(this IHtmlHelper htmlHelper, string title)
        {
            var content = new HtmlContentBuilder()
                              .AppendHtml(String.Format("<strong>{0}</strong>", title));
            return content;
        }

        public static IHtmlContent WidgetBreadCrumb(this IHtmlHelper htmlHelper, string Title)
        {
            var content = new HtmlContentBuilder()
                                .AppendHtml("<div class=\"jumbotron\" data-pages=\"parallax\">")
                                .AppendHtml("    <div class=\"container-fluid container-fixed-lg sm-p-l-0 sm-p-r-0\">")
                                .AppendHtml("        <div class=\"inner\">")
                                .AppendHtml("            <ol class=\"breadcrumb\">")
                                .AppendHtml("                <li class=\"breadcrumb-item\"><a href=\"/AdminDashBoard\">Dashboard</a></li>")
                                .AppendHtml("                <li class=\"breadcrumb-item active\">" + Title + "</li>")
                                .AppendHtml("            </ol>")
                                .AppendHtml("        </div>")
                                .AppendHtml("    </div>")
                                .AppendHtml("</div>");
            return content;
        }
        public static IHtmlContent WidgetBreadCrumb(this IHtmlHelper htmlHelper, string action, string controller, string title, string title2)
        {
            var content = new HtmlContentBuilder()
                                .AppendHtml("<div class=\"jumbotron\" data-pages=\"parallax\">")
                                .AppendHtml("    <div class=\"container-fluid container-fixed-lg sm-p-l-0 sm-p-r-0\">")
                                .AppendHtml("        <div class=\"inner\">")
                                .AppendHtml("            <ol class=\"breadcrumb\">")
                                .AppendHtml("                <li class=\"breadcrumb-item\"><a href=\"/AdminDashBoard\">Dashboard</a></li>")
                                .AppendHtml("                <li class=\"breadcrumb-item\"><a href=\"/" + controller + "/" + action + "\">" + title + "</a></li>")
                                .AppendHtml("                <li class=\"breadcrumb-item active\">" + title2 + "</li>")
                                .AppendHtml("            </ol>")
                                .AppendHtml("        </div>")
                                .AppendHtml("    </div>")
                                .AppendHtml("</div>");
            return content;
        }

        public static IHtmlContent WidgetScreenTitle(this IHtmlHelper htmlHelper, string title, bool isCreate = false)
        {
            var content = new HtmlContentBuilder()
                              .AppendHtml(String.Format("<h4>{0} {1} screen</h4>", title, isCreate ? "create" : "update"));

            return content;
        }

        public static IHtmlContent WidgetResultDescription(this IHtmlHelper htmlHelper, string pattern, IPSFBase psfHelper)
        {
            var content = new HtmlContentBuilder()
                              .AppendHtml(String.Format(pattern, (psfHelper.CurrentPage - 1) * psfHelper.ItemPerPage + 1, psfHelper.CurrentPage * psfHelper.ItemPerPage, psfHelper.TotalItemCount));

            return content;
        }
        public static IHtmlContent WidgetCheckBox(this IHtmlHelper htmlHelper, int ID)
        {
            return WidgetCheckBox(htmlHelper, ID.ToString());
        }
        public static IHtmlContent WidgetCheckBox(this IHtmlHelper htmlHelper, string ID)
        {
            var content = new HtmlContentBuilder()
                              .AppendHtml(" <div class=\"checkbox check-danger text-center\">")
                              .AppendHtml("    <input type=\"checkbox\" value=" + ID + " id=" + String.Format("IDCheckBox_{0}", ID) + " class=\"IDCB\">")
                              .AppendHtml("    <label for=" + String.Format("IDCheckBox_{0}", ID) + " class=\"no-padding no-margin\"></label>")
                              .AppendHtml(" </div>");

            return content;
        }

        public static IHtmlContent WidgetCheckBoxAll(this IHtmlHelper htmlHelper)
        {
            var content = new HtmlContentBuilder()
                              .AppendHtml(" <div class=\"checkbox check-danger text-center\">")
                              .AppendHtml("    <input type=\"checkbox\" id=\"SelectAll\" class=\"CBSelectAll\">")
                              .AppendHtml("    <label for=\"SelectAll\" class=\"no-padding no-margin\"></label>")
                              .AppendHtml(" </div>");

            return content;
        }
        public static IHtmlContent WidgetHeaderLink(this IHtmlHelper htmlHelper, IPSFBase psf, string columnName, string displayName)
        {
            var content = new HtmlContentBuilder();
            content.AppendHtml("<a href=\"?&SortAscending=" + ((!psf.SortAscending).ToString()) + "&sortBy=" + (columnName) + (psf.Filter) + "\" class=\"bold\">" + displayName);
            if (psf.SortBy == columnName)
            {
                string sortDirection = (psf.SortAscending) ? "asc" : "desc";
                content.AppendHtml("        <img src=\"/CustomAssets/images/" + (sortDirection) + ".gif\" height=\"8\" width=\"8\" alt=\"\" class=\"AscDesc\" />");
            }
            content.AppendHtml("</a>");

            return content;
        }

        public static IHtmlContent WidgetAdminPageNumbers(this IHtmlHelper htmlHelper, IPSFBase psf)
        {
            return WidgetPageNumbers(htmlHelper, psf, "pagination", "page-item", "active", "page-link", "");
        }

        public static IHtmlContent WidgetPageNumbers(this IHtmlHelper htmlHelper, IPSFBase psf, string ulClass = "page-numbers", string liClass = "", string liClassActive = "active", string liaClass = "page-numbers", string liaCurrent = "current")
        {
            var content = new HtmlContentBuilder();
            string preUrl = "?ItemPerPage=" + psf.ItemPerPage;

            if (!String.IsNullOrEmpty(psf.SortBy)) { preUrl += "&sortBy=" + psf.SortBy; }
            if (!psf.SortAscending) { preUrl += "&ascending=false"; }
            if (!String.IsNullOrEmpty(psf.Filter)) { preUrl += psf.Filter; }
            preUrl += "&currentPage=";


            if (psf.CurrentPage > psf.TotalItemCount)
                psf.CurrentPage = 1;

            content.AppendHtml("<ul class=\"" + ulClass + "\">");

            if (psf.ItemPerPage < psf.TotalItemCount && psf.CurrentPage > 1)
            {
                content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + (psf.CurrentPage - 1).ToString()) + ">&#8249;&#8249;</a></li>");
                content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + preUrl + ">1</a></li>");
            }
            if ((psf.CurrentPage - 5) > 1)
            {
                if ((psf.CurrentPage - 5) > 1)
                {
                    content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + (psf.CurrentPage - 5).ToString()) + ">...</a></li>");
                }
                for (int i = (psf.CurrentPage - 4); i < psf.CurrentPage; i++)
                {
                    content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + i.ToString()) + ">" + i + "</a></li>");
                }
            }
            else
            {
                for (int i = 2; i < psf.CurrentPage; i++)
                {
                    content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + i.ToString()) + ">" + i + "</a></li>");
                }
            }

            content.AppendHtml("<li class=\"" + liClass + " " + liClassActive + "\"><a class=\"" + liaClass + " " + liaCurrent + "\" href=" + (preUrl + psf.CurrentPage) + ">" + psf.CurrentPage + "</a></li>");

            if ((psf.CurrentPage + 5) < psf.PageCount)
            {
                for (int i = psf.CurrentPage + 1; i < psf.CurrentPage + 5; i++)
                {
                    content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + i.ToString()) + ">" + i + "</a></li>");
                }
                if ((psf.CurrentPage + 5) < psf.PageCount)
                {
                    content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + (psf.CurrentPage + 5).ToString()) + ">...</a></li>");
                }
            }
            else
            {
                for (int i = psf.CurrentPage + 1; i < psf.PageCount; i++)
                {
                    content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + preUrl + i.ToString() + ">" + i + "</a></li>");
                }
            }
            if (psf.CurrentPage < psf.PageCount)
            {
                content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + preUrl + psf.PageCount.ToString() + ">" + psf.PageCount.ToString() + "</a></li>");
                content.AppendHtml("<li class=\"" + liClass + "\"><a class=\"" + liaClass + "\" href=" + (preUrl + (psf.CurrentPage + 1).ToString()) + ">&#8250;&#8250;</a></li>");
            }
            content.AppendHtml("</ul>");
            return content;
        }
    }
}