using auth.Model;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IImportService
    {
        public List<Import> GetImports();
        public List<ImportDetail> GetImportDetail(int id);
        public void AddImport(ImportRequest model);
        public void ImportByCSV(ImportFileRequest model);
    }
}
