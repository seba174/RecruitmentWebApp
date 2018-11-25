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
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(2);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression));
            contentBuilder.AppendHtml(new HtmlString("<br/>"));
            return contentBuilder;
        }

        public static IHtmlContent LabelEmptyLineDisplayFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(2);
            contentBuilder.AppendHtml(htmlHelper.LabelForWithEmptyLine(expression));
            contentBuilder.AppendHtml(htmlHelper.DisplayFor(expression, new { htmlAttributes = new { @class = "form-control" } }));
            return contentBuilder;
        }

        public static IHtmlContent LabelEditorValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(3);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression));
            contentBuilder.AppendHtml(htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } }));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }

        public static IHtmlContent LabelTextAreaValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, string placeholder = "")
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(3);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression));
            contentBuilder.AppendHtml(htmlHelper.TextAreaFor(expression, new { @class = "form-control", placeholder }));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }

        public static IHtmlContent LabelCalendarEditorValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(8);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group\">"));
            contentBuilder.AppendHtml(htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } }));
            contentBuilder.AppendHtml(new HtmlString("<span class=\"input-group-addon\">"));
            contentBuilder.AppendHtml(new HtmlString("<span class=\"glyphicon glyphicon-calendar\"></span>"));
            contentBuilder.AppendHtml(new HtmlString("</span>"));
            contentBuilder.AppendHtml(new HtmlString("</div>"));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }

        public static IHtmlContent LabelMoneyEditorValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, string placeholder = "")
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(7);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group\">"));
            contentBuilder.AppendHtml(new HtmlString("<span class=\"input-group-addon\">PLN</span>"));
            contentBuilder.AppendHtml(htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control", placeholder } }));
            contentBuilder.AppendHtml(new HtmlString("<span class=\"input-group-addon\">.00</span>"));
            contentBuilder.AppendHtml(new HtmlString("</div>"));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }
    }
}