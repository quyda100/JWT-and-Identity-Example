using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface INewService
    {
        public List<NewDTO> GetNews();
        public List<NewDTO> GetNewestPosts();
        public List<NewDTO> GetViewPosts();
        public NewDTO GetNew(int id);
        public void AddNew(NewRequest model);
        public void UpdateNew(int id, NewUpdateRequest model);
        public void DeleteNew(int id);
    }
}
