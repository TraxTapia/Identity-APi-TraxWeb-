
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Trax.Framework.Generic.Logger;
using Trax.Framework.identity;
using Trax.Framework.identity.Filters;
using Trax.Framework.identity.RepositoryIdentity;
using Trax.Models.Generic.Identity.Request;
using Trax.Models.Generic.Identity.Response;
using Trax.Models.Generic.OperationResult;

namespace Api.TraxWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/SettingRole")]
    public class SettingRoleController : ApiController
    {
        private Logger _Logger;

        public SettingRoleController()
        {
            this._Logger = new Logger(System.Web.Hosting.HostingEnvironment.MapPath("~/" + Properties.Settings.Default.LogPath));
        }

        [HttpPost]
        [Route("GetTreeViewRoles")]

        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<TreeViewResponseDTO> GetTreeViewRolesAsync()
        {
            var _Response = new TreeViewResponseDTO();
            try
            {
                var _Repository = new IdentityRepository();
                _Response = await _Repository.GetTreeViewAsync();
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }

        [HttpPost]
        [Route("SaveTreeViewRoles")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> SaveTreeViewRolesAsync(TreeViewRequestDTO TreeView)
        {
            var _Response = new OperationResult();
            try
            {
                IdentityRepository _Repository = new IdentityRepository();
                _Response = await _Repository.SaveChangesTreeViewAsync(TreeView);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.AddException(ex);
            }
            return _Response;
        }

        [HttpPost]
        [Route("GetTreeViewRolesByUser")]

        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<TreeViewResponseDTO> GetTreeViewRolesByUserAsync([FromBody] string Email)
        {
            var _Response = new TreeViewResponseDTO();
            try
            {
                if (string.IsNullOrEmpty(Email) || !Validator.IsEmail(Email))
                {
                    _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _Response.Result.AddException(new Exception("El Email proporcionado no es una dirección válida."));
                    return _Response;
                }
                var _Repository = new IdentityRepository(this._Logger);
                _Response = await _Repository.GetTreeViewRolesByUserAsync(Email);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }

        [HttpPost]
        [Route("SaveTreeViewRolesByUser")]

        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> SaveTreeViewRolesByUserAsync(SaveChangesRolesByUserRequestDTO _Request)
        {
            var _Response = new OperationResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    _Response.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    var _Errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
                    _Errors.ForEach(x => { _Response.AddException(new Exception(x.ErrorMessage)); });
                    return _Response;
                }
                var _Repository = new IdentityRepository();
                _Response = await _Repository.SaveTreeViewRolesByUserAsync(_Request);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.AddException(ex);
            }
            return _Response;
        }

        [HttpPost]
        [Route("GetApplications")]

        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<GetApplicationsResponseDTO> GetApplicationsAsync()
        {
            GetApplicationsResponseDTO _Response = new GetApplicationsResponseDTO();
            try
            {
                var _Repository = new IdentityRepository(this._Logger);
                _Response = await _Repository.GetApplicationsAsync();
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }

        [HttpPost]
        [Route("AddRoles")]

        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> AddRolesAsync(AddRolesRequestDTO _Request)
        {
            var _Response = new OperationResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    _Response.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    var _Errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
                    _Errors.ForEach(x => { _Response.AddException(new Exception(x.ErrorMessage)); });
                    return _Response;
                }
                var _Repository = new IdentityRepository(this._Logger);
                _Response = await _Repository.AddRolesAsync(_Request);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.AddException(ex);
            }
            return _Response;
        }
    }
}