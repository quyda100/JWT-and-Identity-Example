using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface INewService
    {
        public List<NewDTO> GetNews();
        public New GetNew(int id);
        public void AddNew(NewRequest model);
        public void UpdateNew(int id, NewDTO model);
        public void DeleteNew(int id);
    }
}
