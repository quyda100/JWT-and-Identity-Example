using auth.Model;

namespace auth.Interfaces
{
    public interface IImportService
    {
        public Task<IEnumerable<Import>> getImports();
        public Task<IEnumerable<Import>> getImportDetail(int id);

        public void addImport(Import model);
        public void updateImport(int id, Import model);
        public void deleteImport(int id);
    }
}
