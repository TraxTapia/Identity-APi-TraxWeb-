

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Trax.Framework.Generic.Error;
using Trax.Framework.Generic.Logger;
using Trax.Framework.identity;
using Trax.Framework.identity.Filters;
using Trax.Framework.identity.RepositoryIdentity;
using Trax.Models.Generic.Identity;
using Trax.Models.Generic.Identity.Request;
using Trax.Models.Generic.Identity.Response;
using Trax.Models.Generic.Mail;
using Trax.Models.Generic.OperationResult;
using Trax.Models.Identity.Identity;
using Trax.Framework.Generic.Email;

namespace Api.TraxWeb.Controllers
{
    [RoutePrefix("Api/SettingUser")]
    public class SettingUserController : ApiController
    {
        private ApplicationUserManager _userManager;
        private Logger _Logger;


        public SettingUserController()
        {
            this._Logger = new Logger(System.Web.Hosting.HostingEnvironment.MapPath("~/" + Properties.Settings.Default.LogPath));

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpPost]
        [Route("RegisterUser")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> RegisterUserAsync(RegisterUserRequestDTO Register)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (!ModelState.IsValid || !Validator.IsEmail(Register.Email))
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    var _Errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
                    _Errors.ForEach(x => { if (x.Exception == null) _OperationResult.AddException(new Exception(x.ErrorMessage)); else _OperationResult.AddException(x.Exception); });
                    return _OperationResult;
                }
                IdentityRepository _RepositoryIdentity = new IdentityRepository();
                string _Password = _RepositoryIdentity.GeneratePassword(8);
                string _UserName = Register.Name.Trim() + " " + Register.FirstSurname.Trim() + " " + Register.SecondSurname.Trim();
                var _Result = await this.UserManager.CreateAsync(new ApplicationUser
                {
                    FullUserName = _UserName,
                    UserName = Register.Email.Trim(),
                    Email = Register.Email.Trim(),
                    EmailConfirmed = false,
                    CodeProvider = Register.CodeProvider,
                    NormalizedEmail = Register.Email.Trim(),
                    NormalizedUserName = Register.Email.Trim()
                }, _Password);
                if (!_Result.Succeeded)
                {
                    this._Logger.LogText("AccountController : RegisterUser : " + string.Join(", ", _Result.Errors));
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                    _OperationResult.AddException(new Exception(string.Join(",", _Result.Errors)));
                    return _OperationResult;
                }
                //MailConfiguration _MailConfiguration = new MailConfiguration();
                //this.SetMailConfiguration(ref _MailConfiguration);
                //Email _Email = new Email(_MailConfiguration);
                //string _Body = _RepositoryIdentity.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath("~/" + Properties.Settings.Default.TemplateCreateUser));
                //_Body = _Email.AcentuaString(_Body);
                //_Body = _Body.Replace("{CaseTag:Nombre}", _UserName); _Body = _Body.Replace("{CaseTag:CodLogin}", Register.Email); _Body = _Body.Replace("{CaseTag:ClaveAcceso}", _Password);
                //await _Email.SendAsync(new IdentityMessage()
                //{
                //    Body = _Body,
                //    Destination = Register.Email,
                //    Subject = "Credencial de Accesso"
                //});
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        [HttpPost]
        [Route("AssignRoleToUser")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> AssignRoleToUserAsync(RoleUserRequestDTO RoleUserRequest)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                IdentityRepository _RepositoryIdentity = new IdentityRepository(this._Logger);
                _OperationResult = await _RepositoryIdentity.AssignRoleToUserAsync(RoleUserRequest);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }

        [HttpPost]
        [Route("RemoveRoleUser")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> RemoveRoleUserAsync(RoleUserRequestDTO RoleUserRequest)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                IdentityRepository _RepositoryIdentity = new IdentityRepository(this._Logger);
                _OperationResult = await _RepositoryIdentity.RemoveRoleByUserAsync(RoleUserRequest);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        [HttpPost]
        [Route("FindUserByName")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<FindUserByNameResponseDTO> FindUserByNameAsync([FromBody] string UserName)
        {
            FindUserByNameResponseDTO _ResponseFindUserByName = new FindUserByNameResponseDTO();
            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    _ResponseFindUserByName.Result.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _ResponseFindUserByName.Result.AddException(new Exception("UserName inválido."));
                    return _ResponseFindUserByName;
                }
                IdentityRepository _RepositoryIdentity = new IdentityRepository(this._Logger);
                _ResponseFindUserByName = await _RepositoryIdentity.FindUserByEmailAsync(UserName);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _ResponseFindUserByName.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _ResponseFindUserByName.Result.AddException(ex);
            }
            return _ResponseFindUserByName;
        }

        [HttpPost]
        [Route("ListAllUsers")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<GetAllUsersResponseDTO> ListAllUsersAsync()
        {
            IdentityRepository _RepositoryIdentity = new IdentityRepository(this._Logger);
            GetAllUsersResponseDTO _GetAllUsersResponse = await _RepositoryIdentity.GetAllUserAsync();
            return _GetAllUsersResponse;
        }

        [HttpPost]
        [Route("UpdateUser")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> UpdateUserAsync(UpdateUserRequestDTO User)
        {
            OperationResult _Response = new OperationResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    _Response.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    var _Errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
                    _Errors.ForEach(x => { _Response.AddException(new Exception(x.ErrorMessage)); });
                    return _Response;
                }
                var _User = await this.UserManager.FindByEmailAsync(User.Email);
                if (_User == null)
                {
                    _Response.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                    _Response.AddException(new Exception("No se encontro el usuario " + User.Email + "."));
                    return _Response;
                }
                _User.FullUserName = User.Name + " " + User.FirstSurname + " " + User.SecondSurname;
                _User.CodeProvider = User.CodeProvider;
                if (User.Enable)
                    _User.LockoutEndDateUtc = null;
                else
                    _User.LockoutEndDateUtc = DateTime.Now.AddYears(200);
                var _Result = await this.UserManager.UpdateAsync(_User);
                if (!_Result.Succeeded)
                    throw new Exception(string.Join(", ", _Result.Errors));
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
        [UserInRole(Application = "TRAX.IDENTITY")]
        [Route("ResetPassword")]
        public async Task<OperationResult> ResetPasswordAsync([FromBody] string Email)
        {
            OperationResult _OperationResult = new OperationResult();
            if (string.IsNullOrEmpty(Email))
            {
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                _OperationResult.AddException(new Exception("Parámetros inválidos."));
                return _OperationResult;
            }
            try
            {
                IdentityRepository _RepositoryIdentity = new IdentityRepository();
                ApplicationUser _ApplicationUser = new ApplicationUser();
                if (!(_OperationResult = _RepositoryIdentity.GetUserByEmail(Email, ref _ApplicationUser)).IsOK())
                    return _OperationResult;
                string _TokenResetPassword = await this.UserManager.GeneratePasswordResetTokenAsync(_ApplicationUser.Id);
                MailConfiguration _MailConfiguration = new MailConfiguration();
                this.SetMailConfiguration(ref _MailConfiguration);
                Email _Email = new Email(_MailConfiguration);
                string _Body = _RepositoryIdentity.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath("~/" + Properties.Settings.Default.TemplateResetPassword));
                _Body = _Email.AcentuaString(_Body);
                _Body = _Body.Replace("{CaseTag:Nombre}", _ApplicationUser.FullUserName);
                _Body = _Body.Replace("{SecureToken}", Url.Link("ResetPassword", new { UserId = _ApplicationUser.Id, Token = _TokenResetPassword }));
                await _Email.SendAsync(new IdentityMessage()
                {
                    Body = _Body,
                    Destination = Email,
                    Subject = "Restablecimiento de Contraseña"
                });
                _OperationResult.Message = "Se envió un correo a la dirección Email asociada al usuario, para proseguir con el proceso es necesario ingrese a liga proporcionada en el correo.";
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }

        [HttpGet]
        [Route("ResetPassword", Name = "ResetPassword")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPasswordAsync(string UserId, string Token)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            {
                return RedirectToRoute("Default",
                    new
                    {
                        controller = "Home",
                        action = "ResetPassword",
                        Message = "Parámetros inválidos.",
                        Code = 400
                    });
            }
            try
            {
                IdentityRepository _RepositoryIdentity = new IdentityRepository(this._Logger);
                string _Password = _RepositoryIdentity.GeneratePassword(8);
                IdentityResult _IdentityResult = new IdentityResult();
                if (!(_IdentityResult = await this.UserManager.ResetPasswordAsync(UserId, Token, _Password)).Succeeded)
                    return RedirectToRoute("Default",
                        new
                        {
                            controller = "Home",
                            action = "ResetPassword",
                            Message = string.Join(",", _IdentityResult.Errors),
                            Code = 404
                        });
                await this.UserManager.UpdateSecurityStampAsync(UserId);
                ApplicationUser _ApplicationUser = await this.UserManager.FindByIdAsync(UserId);
                MailConfiguration _MailConfiguration = new MailConfiguration();
                this.SetMailConfiguration(ref _MailConfiguration);
                Email _Email = new Email(_MailConfiguration);
                string _Body = _RepositoryIdentity.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath("~/" + Properties.Settings.Default.TemplateResetPasswordConfirm));
                _Body = _Email.AcentuaString(_Body);
                _Body = _Body.Replace("{CaseTag:Nombre}", _ApplicationUser.FullUserName); _Body = _Body.Replace("{CaseTag:CodLogin}", _ApplicationUser.Email); _Body = _Body.Replace("{CaseTag:ClaveAcceso}", _Password);
                OperationResult _OperationResult = new OperationResult();
                await _Email.SendAsync(new IdentityMessage()
                {
                    Body = _Body,
                    Destination = _ApplicationUser.Email,
                    Subject = "Restablecimiento de Contraseña"
                });
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                return RedirectToRoute("Default",
                new
                {
                    controller = "Home",
                    action = "ResetPassword",
                    Message = Error.InnerExceptionMessage(ex) + "\r\n" + ex.StackTrace,
                    Code = 500
                });
            }
            return RedirectToRoute("Default",
                 new
                 {
                     controller = "Home",
                     action = "ResetPassword",
                     Message = "Se restablecio con éxito su contraseña. Se envio un correo con su nueva contraseña.",
                     Code = "200"
                 });
        }

        [HttpPost]
        [Route("ChangePassword")]
        [UserInRole(Application = "TRAX.IDENTITY")]
        public async Task<OperationResult> ChangePasswordAsync(ChangePasswordRequestDTO ChangePasswordRequest)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _OperationResult.AddException(new Exception("Parámetros de entrada inválidos."));
                    return _OperationResult;
                }
                ApplicationUser _ApplicationUser = new ApplicationUser();
                if ((_ApplicationUser = await this.UserManager.FindByEmailAsync(ChangePasswordRequest.Email)) == null)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                    _OperationResult.AddException(new Exception("Usuario no encontrado."));
                    return _OperationResult;
                }
                IdentityRepository _RepositoryIdentity = new IdentityRepository(this._Logger);
                if (!(_OperationResult = await _RepositoryIdentity.ChangePasswordAsync(_ApplicationUser.Id, ChangePasswordRequest)).IsOK())
                    return _OperationResult;
                MailConfiguration _MailConfiguration = new MailConfiguration();
                this.SetMailConfiguration(ref _MailConfiguration);
                Email _Email = new Email(_MailConfiguration);
                string _Body = _RepositoryIdentity.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath("~/" + Properties.Settings.Default.TemplateChangePassword));
                _Body = _Email.AcentuaString(_Body);
                _Body = _Body.Replace("{CaseTag:Nombre}", _ApplicationUser.FullUserName);
                await _Email.SendAsync(new IdentityMessage()
                {
                    Body = _Body,
                    Destination = _ApplicationUser.Email,
                    Subject = "Cambio de contraseña"
                });
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }

        private void SetMailConfiguration(ref MailConfiguration MailConfiguration)
        {
            MailConfiguration = new MailConfiguration()
            {
                //SMTPEnableSSL = Properties.Settings.Default.SMTPEnableSSL,
                //SMTPHost = Properties.Settings.Default.SMTPHost,
                //SMTPFromMail = Properties.Settings.Default.SMTPFrom,
                //SMTPUserName = Properties.Settings.Default.SMTPUserName,
                //SMTPPassword = Properties.Settings.Default.SMTPPassword,
                //SMTPPort = Properties.Settings.Default.SMTPPort,
                //SMTPTimeOut = Properties.Settings.Default.SMTPTimeOut
            };
        }

    }
}