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
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression, new { @class= "font-weight-bold" }));
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
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression, new { @class = "font-weight-bold"}));
            contentBuilder.AppendHtml(htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } }));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }

        public static IHtmlContent LabelTextAreaValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, string placeholder = "")
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(3);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression, new { @class = "font-weight-bold" }));
            contentBuilder.AppendHtml(htmlHelper.TextAreaFor(expression, new { @class = "form-control", placeholder }));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }

        public static IHtmlContent LabelCalendarEditorValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(9);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression, new { @class = "font-weight-bold" }));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group\">"));
            contentBuilder.AppendHtml(htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } }));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group-append\">"));
            contentBuilder.AppendHtml(new HtmlString("<span class=\"input-group-text\">"));
            contentBuilder.AppendHtml(new HtmlString("<i class=\"fas fa-calendar-alt\" aria-hidden=\"true\"></i>"));
            contentBuilder.AppendHtml(new HtmlString("</span></div>"));
            contentBuilder.AppendHtml(new HtmlString("</div>"));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }

        public static IHtmlContent LabelMoneyEditorValidationMessageFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, string placeholder = "")
        {
            HtmlContentBuilder contentBuilder = new HtmlContentBuilder(7);
            contentBuilder.AppendHtml(htmlHelper.LabelFor(expression, new { @class = "font-weight-bold" }));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group\">"));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group-prepend\"><span class=\"input-group-text\">PLN</span></div>"));
            contentBuilder.AppendHtml(htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control", placeholder } }));
            contentBuilder.AppendHtml(new HtmlString("<div class=\"input-group-append\"><span class=\"input-group-text\">.00</span></div>"));
            contentBuilder.AppendHtml(new HtmlString("</div>"));
            contentBuilder.AppendHtml(htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return contentBuilder;
        }
    }
}