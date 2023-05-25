using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IImportService
    {
        public List<ImportDTO> GetImports();
        public List<ImportProductDTO> GetImportDetail(int id);
        public void AddImport(ImportRequest model);
        public void ImportByCSV(ImportFileRequest model);
    }
}
