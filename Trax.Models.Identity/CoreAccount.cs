//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Trax.Models.Identity
//{
//    public class CoreAccount
//    {
//        private Logger _Logger;
//        private string _ConnectionString;
//        private string _ApplicationName = "MAC.PROVIDERS";

//        public CoreAccount(MAC.Framework.Generic.Logger.Logger Logger) => this._Logger = Logger;

//        public MAC.Models.Generic.OperationResult.OperationResult RegisterUser(
//          RegisterUserRequestDTO Register,
//          string EndPoint,
//          string UserName,
//          string Password)
//        {
//            MAC.Models.Generic.OperationResult.OperationResult operationResult = new MAC.Models.Generic.OperationResult.OperationResult();
//            try
//            {
//                if (string.IsNullOrEmpty(Register.CodeProvider) || string.IsNullOrEmpty(Register.Email))
//                {
//                    operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.BAD_REQUEST);
//                    operationResult.AddException(new Exception("Los paramétros de entrada son requeridos."));
//                    return operationResult;
//                }
//                List<string> providerEmailsByCode = new RepositoryAccount().FindProviderEmailsByCode(int.Parse(Register.CodeProvider));
//                if (providerEmailsByCode == null || providerEmailsByCode.Count <= 0)
//                {
//                    operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.NOT_FOUND);
//                    operationResult.AddException(new Exception("No se encontro el Proveedor ingresado."));
//                    return operationResult;
//                }
//                if (!providerEmailsByCode.Contains(Register.Email))
//                {
//                    operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.NOT_FOUND);
//                    operationResult.AddException(new Exception("El correo proporcionado no se encuentra registrado como contacto, favor de validar."));
//                    return operationResult;
//                }
//                ClientIdentityAPI clientIdentityApi = new ClientIdentityAPI(this._Logger);
//                clientIdentityApi.SetUrlEndPoint(EndPoint);
//                if (!(operationResult = clientIdentityApi.GetToken(UserName, Password)).IsOK() || !(operationResult = clientIdentityApi.RegisterUser(Register)).IsOK())
//                    return operationResult;
//                RoleUserRequestDTO Request = new RoleUserRequestDTO()
//                {
//                    Email = Register.Email,
//                    Application = this._ApplicationName,
//                    Roles = new List<string>()
//          {
//            EnumRoles.RoleFarmacia.ToString()
//          }
//                };
//                operationResult = clientIdentityApi.AddRoleToUser(Request);
//            }
//            catch (Exception ex)
//            {
//                this._Logger.LogError(ex);
//                operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
//                operationResult.AddException(ex);
//            }
//            return operationResult;
//        }

//        public MAC.Models.Generic.OperationResult.OperationResult ResetPassword(
//          ResetPasswordViewModel model,
//          string UrlEndPoint,
//          string UserName,
//          string Password)
//        {
//            MAC.Models.Generic.OperationResult.OperationResult operationResult = new MAC.Models.Generic.OperationResult.OperationResult();
//            if (!string.IsNullOrEmpty(model.CodeProviderReset))
//            {
//                if (!string.IsNullOrEmpty(model.EmailReset))
//                {
//                    try
//                    {
//                        List<string> providerEmailsByCode = new RepositoryAccount().FindProviderEmailsByCode(int.Parse(model.CodeProviderReset));
//                        if (providerEmailsByCode == null || providerEmailsByCode.Count <= 0)
//                        {
//                            operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.NOT_FOUND);
//                            operationResult.AddException(new Exception("El número de Proveedor es incorrecto."));
//                            return operationResult;
//                        }
//                        if (!providerEmailsByCode.Contains(model.EmailReset))
//                        {
//                            operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.NOT_FOUND);
//                            operationResult.AddException(new Exception("El correo electrónico proporcinado no se encuntra asociado a alguna cuenta válida, favor de validar."));
//                            return operationResult;
//                        }
//                        ClientIdentityAPI clientIdentityApi = new ClientIdentityAPI(this._Logger);
//                        clientIdentityApi.SetUrlEndPoint(UrlEndPoint);
//                        if (!(operationResult = clientIdentityApi.GetToken(UserName, Password)).IsOK())
//                            return operationResult;
//                        operationResult = clientIdentityApi.ResetPassword(model.EmailReset);
//                    }
//                    catch (Exception ex)
//                    {
//                        this._Logger.LogError(ex);
//                        operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
//                        operationResult.AddException(ex);
//                    }
//                    return operationResult;
//                }
//            }
//            operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.BAD_REQUEST);
//            operationResult.AddException(new Exception("Hacen falta parametros para completar la operaión."));
//            return operationResult;
//        }

//        public MAC.Models.Generic.OperationResult.OperationResult ChangePassword(
//          ChangePasswordRequestDTO change,
//          string UrlEndPoint,
//          string UserName,
//          string Password)
//        {
//            MAC.Models.Generic.OperationResult.OperationResult operationResult = new MAC.Models.Generic.OperationResult.OperationResult();
//            if (!string.IsNullOrEmpty(change.OldPassword))
//            {
//                if (!string.IsNullOrEmpty(change.NewPassword))
//                {
//                    try
//                    {
//                        ClientIdentityAPI clientIdentityApi = new ClientIdentityAPI(this._Logger);
//                        clientIdentityApi.SetUrlEndPoint(UrlEndPoint);
//                        if (!(operationResult = clientIdentityApi.GetToken(UserName, Password)).IsOK())
//                            return operationResult;
//                        operationResult = clientIdentityApi.ChangePassword(change);
//                    }
//                    catch (Exception ex)
//                    {
//                        this._Logger.LogError(ex);
//                        operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
//                        operationResult.AddException(ex);
//                    }
//                    return operationResult;
//                }
//            }
//            operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.BAD_REQUEST);
//            operationResult.AddException(new Exception("Hacen falta parametros para completar la operaión."));
//            return operationResult;
//        }

//        public MAC.Models.Generic.OperationResult.OperationResult SendMail(
//          EmailRequestDTO Request,
//          string UrlEndPoint,
//          string UserName,
//          string Password)
//        {
//            MAC.Models.Generic.OperationResult.OperationResult operationResult = new MAC.Models.Generic.OperationResult.OperationResult();
//            try
//            {
//                ClientIdentityAPI clientIdentityApi = new ClientIdentityAPI(this._Logger);
//                clientIdentityApi.SetUrlEndPoint(UrlEndPoint);
//                if (!(operationResult = clientIdentityApi.GetToken(UserName, Password)).IsOK())
//                    return operationResult;
//                operationResult = clientIdentityApi.SendMail(Request);
//            }
//            catch (Exception ex)
//            {
//                this._Logger.LogError(ex);
//                operationResult.SetStatusCode(MAC.Models.Generic.OperationResult.OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
//                operationResult.AddException(ex);
//            }
//            return operationResult;
//        }

//        public RegisterUserRequestDTO MapRegisterViewModelToRegisterRequest(
//          RegisterViewModel Request)
//        {
//            RegisterUserRequestDTO registerUserRequestDto = new RegisterUserRequestDTO();
//            registerUserRequestDto.CodeProvider = Request.CodeProviderRegister;
//            registerUserRequestDto.set_UserName(Request.UserNameRegister);
//            registerUserRequestDto.Email = Request.EmailRegister;
//            registerUserRequestDto.FirstSurname = Request.FirstSurname;
//            registerUserRequestDto.SecondSurname = Request.SecondSurname;
//            return registerUserRequestDto;
//        }

//        public ChangePasswordRequestDTO MapChangePasswordViewModelToRegisterRequest(
//          ChangePasswordViewModel Request)
//        {
//            return new ChangePasswordRequestDTO()
//            {
//                OldPassword = Request.OldPassword,
//                NewPassword = Request.NewPassword,
//                ConfirmPassword = Request.ConfirmatedPassword
//            };
//        }

//        public bool HasConnectionString() => !string.IsNullOrEmpty(this._ConnectionString);

//        private bool ExistUserByCodeProvider(string Email, string CodeProvider)
//        {
//            if (!this.HasConnectionString())
//                throw new Exception("No se asigno una coneccion a la BD.");
//            SQLServerDBConnection serverDbConnection = new SQLServerDBConnection((ILogger)this._Logger, this._ConnectionString);
//            try
//            {
//                serverDbConnection.Connect();
//                return serverDbConnection.GetDataTable("EXEC IdentityDB.dbo.sp_GetUserByUserNameAndCodeProvider '" + Email + "', '" + CodeProvider + "'").Rows.Count > 0;
//            }
//            finally
//            {
//                serverDbConnection.CloseConnection();
//            }
//        }

//        public void SetConnectionString(string ConnectionString) => this._ConnectionString = ConnectionString;
//    }
//}
