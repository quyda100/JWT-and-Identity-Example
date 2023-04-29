using auth.Model;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IImportService
    {
        public List<Import> getImports();
        public List<ImportDetail> getImportDetail(int id);

        public void addImport(ImportRequest model);
    }
}
