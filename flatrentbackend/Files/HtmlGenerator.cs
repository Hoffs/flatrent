using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FlatRent.Files
{
    public static class HtmlGenerator
    {
        public static async Task<string> GetAgreementHtml(AgreementPatchData patchData)
        {
            var fs = await File.ReadAllTextAsync(Path.Join("Files", "AgreementTemplate.html"), Encoding.UTF8).ConfigureAwait(false);
            return PatchData(fs, patchData);
        }

        private static string PatchData<T>(string fileString, T data)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                fileString = fileString.Replace($"{{{{{property.Name}}}}}", property.GetValue(data).ToString());
            }

            return fileString;
        }
    }
}