using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Email.Core.Interfaces;

namespace Email.Core.Renderers
{
    public class ReplaceRenderer : ITemplateRenderer
    {
        public string Parse<T>(string template, T model, bool isHtml = true)
        {
            foreach (var pi in model.GetType().GetRuntimeProperties())
            {
                template = template.Replace($"##{pi.Name}##", pi.GetValue(model, null).ToString());
            }

            return template;
        }

        public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Parse(template, model, isHtml));
        }
    }
}