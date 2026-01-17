using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface IFBORepository
    {
        List<FBO> GetAllFBOs();
        FBO? GetFBOById(int id);
        FBO? GetFBOByICAO(string icao);
        void AddFBO(FBO fbo);
        void UpdateFBO(FBO fbo);
        void DeleteFBO(int id);
    }
}
