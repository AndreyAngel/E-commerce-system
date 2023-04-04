using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace OrderAPI.Helpers;

/// <summary>
/// Class used for auto-documentation of enums
/// </summary>
public class EnumTypesSchemaFilter : ISchemaFilter
{
    private readonly XDocument _xmlComments;

    /// <summary>
    /// Initializes a new instance of <see cref="EnumTypesSchemaFilter"/>.
    /// </summary>
    /// <param name="xmlPath"></param>
    public EnumTypesSchemaFilter(string xmlPath)
    {
        if (File.Exists(xmlPath))
        {
            _xmlComments = XDocument.Load(xmlPath);
        }
    }

    /// <inheritdoc/>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (_xmlComments == null) return;

        if (schema.Enum != null && schema.Enum.Count > 0 &&
            context.Type != null && context.Type.IsEnum)
        {
            schema.Description += "<p>Members:</p><ul>";

            var fullTypeName = context.Type.FullName;

            foreach (var enumMemberName in Enum.GetValues(context.Type))
            {
                var enumMemberValue = Convert.ToInt64(enumMemberName);

                var fullEnumMemberName = $"F:{fullTypeName}.{enumMemberName}";

                var enumMemberComments = _xmlComments.Descendants("member")
                    .FirstOrDefault(m => m.Attribute("name").Value.Equals
                    (fullEnumMemberName, StringComparison.OrdinalIgnoreCase));

                if (enumMemberComments == null) continue;

                var summary = enumMemberComments.Descendants("summary").FirstOrDefault();

                if (summary == null) continue;

                schema.Description += $"<li><i>{enumMemberValue}</i> - {summary.Value.Trim()}</li>";
            }

            schema.Description += "</ul>";
        }
    }
}
