using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecrutimentApp
{
    public static class HtmlExtensions
    {
        public static IHtmlContent LabelForWithEmptyLine<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder();
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression));
            contentBuilder.AppendHtml(new HtmlString("<br/>"));
            return contentBuilder;
        }
    }
}