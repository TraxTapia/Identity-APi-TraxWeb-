using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trax.Framework.Generic.Error;
using Trax.Framework.Generic.Logger;
using Trax.Models.Generic.Identity;
using Trax.Models.Generic.Identity.Request;
using Trax.Models.Generic.Identity.Response;
using Trax.Models.Generic.OperationResult;
using System.Data.Entity;
using Trax.Models.Generic.Identity.Enum;
using Trax.Models.Identity.DataBaseFirst;
using Trax.Models.Identity;
using Trax.Models.Identity.Identity;

namespace Trax.Framework.identity.RepositoryIdentity
{
   public  class IdentityRepository
    {
        private Logger _Logger;

        public IdentityRepository(Logger Logger)
        {
            this._Logger = Logger;
        }
        public IdentityRepository()
        { }
        public async Task<OperationResult> AssignRoleToUserAsync(RoleUserRequestDTO RoleUserRequest)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (!(RoleUserRequest.Roles != null && RoleUserRequest.Roles.Count > 0))
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _OperationResult.AddException(new Exception("No se encontraron los roles a asignar."));
                    return _OperationResult;
                }
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityAppDbContext));
                    var _User = await _UserManager.FindByEmailAsync(RoleUserRequest.Email);
                    if (_User == null)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("Usuario no encontrado."));
                        return _OperationResult;
                    }
                    else if (await _UserManager.IsLockedOutAsync(_User.Id))
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NON_AUTHORITATIVE_INFORMATION);
                        _OperationResult.AddException(new Exception("El usuario se encuentra bloqueado, favor de contactar al Administrador del Sistema."));
                        return _OperationResult;
                    }
                    IdentityResult _IdentityResult = new IdentityResult();
                    List<string> _Roles = _IdentityAppDbContext.Aplications.Include(x => x.Roles).Where(x => x.Visible && x.Name == RoleUserRequest.Application).SelectMany(x => x.Roles).Select(x => x.Name).ToList();
                    if (!(_Roles != null && _Roles.Count > 0))
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("No se encontraron roles para la aplicación indicada."));
                        return _OperationResult;
                    }
                    _Roles = _Roles.Where(x => RoleUserRequest.Roles.Any(y => y.Equals(x))).ToList();
                    if (_Roles != null && _Roles.Count > 0)
                    {
                        if (!(_IdentityResult = await _UserManager.AddToRolesAsync(_User.Id, _Roles.ToArray())).Succeeded)
                            throw new Exception(string.Join(", ", _IdentityResult.Errors));
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : AssignRoleToUser : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<OperationResult> RemoveRoleByUserAsync(RoleUserRequestDTO RoleUserRequest)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (!(RoleUserRequest.Roles != null && RoleUserRequest.Roles.Count > 0))
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _OperationResult.AddException(new Exception("No se encontraron los roles a asignar."));
                    return _OperationResult;
                }
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityAppDbContext));
                    var _User = await _UserManager.FindByEmailAsync(RoleUserRequest.Email);
                    if (_User == null)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("Usuario no encontrado."));
                        return _OperationResult;
                    }
                    IdentityResult _IdentityResult = new IdentityResult();
                    List<string> _Roles = _IdentityAppDbContext.Aplications.Include(x => x.Roles).Where(x => x.Visible && x.Name == RoleUserRequest.Application).SelectMany(x => x.Roles).Select(x => x.Name).ToList();
                    if (!(_Roles != null && _Roles.Count > 0))
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("No se encontraron roles para la aplicación indicada."));
                        return _OperationResult;
                    }
                    _Roles = _Roles.Where(x => RoleUserRequest.Roles.Any(y => y.Equals(x))).ToList();
                    if (!(_Roles != null && _Roles.Count > 0))
                    {
                        if (!(_IdentityResult = await _UserManager.RemoveFromRolesAsync(_User.Id, _Roles.ToArray())).Succeeded)
                            throw new Exception(string.Join(", ", _IdentityResult.Errors));
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : RemoveRoleByUserName : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<FindUserByNameResponseDTO> FindUserByEmailAsync(string Email)
        {
            FindUserByNameResponseDTO _Response = new FindUserByNameResponseDTO();
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(_IdentityAppDbContext));
                    var _User = await _UserManager.FindByEmailAsync(Email);
                    if (_User == null)
                    {
                        _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _Response.Result.AddException(new Exception("No se encontro el Usuario " + Email + "."));
                        return _Response;
                    }
                    var _Names = _User.FullUserName.Split(' ').ToList();
                    if (_Names.Count < 3)
                    {
                        _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.CONFLICT);
                        _Response.Result.AddException(new Exception("El nombre de usuario se encuentra incompleto."));
                        return _Response;
                    }
                    string _FirstSurname = _Names[_Names.Count - 1];
                    string _SecondSurname = _Names[_Names.Count - 2];
                    _Names.Remove(_FirstSurname);
                    _Names.Remove(_SecondSurname);
                    _Response.User = new UserDTO()
                    {
                        Email = _User.UserName,
                        Name = string.Join(" ", _Names),
                        FirstSurname = _FirstSurname,
                        SecondSurname = _SecondSurname,
                        Enable = !(_User.LockoutEndDateUtc != null && _User.LockoutEndDateUtc > DateTime.Now),
                        CodeProvider = string.IsNullOrEmpty(_User.CodeProvider) ? string.Empty : _User.CodeProvider
                    };
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : FindUserByUserName : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }
        public async Task<GetAllUsersResponseDTO> GetAllUserAsync()
        {
            GetAllUsersResponseDTO _Response = new GetAllUsersResponseDTO();
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser, string>(new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(_IdentityAppDbContext));
                    var _Users = await _UserManager.Users.ToListAsync();
                    _Response.Users = _Users.OrderBy(x => x.FullUserName)
                        .Where(x => x.FullUserName.Split(' ').Count() >= 3)
                        .Select(x =>
                        {
                            List<string> _Names = x.FullUserName.Split(' ').ToList();
                            string _FirstSurname = _Names[_Names.Count - 1];
                            string _SecondSurname = _Names[_Names.Count - 2];
                            _Names.Remove(_FirstSurname);
                            _Names.Remove(_SecondSurname);
                            return new UserDTO()
                            {
                                Name = string.Join(" ", _Names),
                                FirstSurname = _FirstSurname,
                                SecondSurname = _SecondSurname,
                                Email = x.Email,
                                Enable = !(x.LockoutEndDateUtc != null && x.LockoutEndDateUtc > DateTime.Now),
                                CodeProvider = string.IsNullOrEmpty(x.CodeProvider) ? string.Empty : x.CodeProvider
                            };
                        }).ToList();
                    if (!(_Response.Users != null && _Response.Users.Count > 0))
                    {
                        _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _Response.Result.AddException(new Exception("No se encontraron usuarios."));
                        return _Response;
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : GetAllUser : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }
        public async Task<OperationResult> LockUserAsync(string Email)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityAppDbContext));
                    var _User = await _UserManager.FindByNameAsync(Email);
                    if (_User == null)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("Usuario no encontrado."));
                        return _OperationResult;
                    }
                    else if (await _UserManager.IsLockedOutAsync(_User.Id))
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_ACCEPTABLE);
                        _OperationResult.AddException(new Exception("El usuario ya se encuantra bloqueado."));
                        return _OperationResult;
                    }
                    await _UserManager.SetLockoutEnabledAsync(_User.Id, true);
                    await _UserManager.SetLockoutEndDateAsync(_User.Id, DateTime.Today.AddYears(200));
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : LockUser : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<OperationResult> ChangePasswordAsync(string UserId, ChangePasswordRequestDTO ChangePasswordBindingModel)
        {
            OperationResult _OperationResult = new OperationResult();
            if (string.IsNullOrEmpty(UserId))
            {
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                _OperationResult.AddException(new Exception("El parámetro UserId no puede ser null o vacio."));
                return _OperationResult;
            }
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityAppDbContext));
                    IdentityResult _IdentityResult = await _UserManager.ChangePasswordAsync(UserId, ChangePasswordBindingModel.OldPassword, ChangePasswordBindingModel.NewPassword);
                    if (!_IdentityResult.Succeeded)
                        throw new Exception(string.Join(", ", _IdentityResult.Errors));
                    _IdentityResult = await _UserManager.UpdateSecurityStampAsync(UserId);
                    if (!_IdentityResult.Succeeded)
                        throw new Exception(string.Join(", ", _IdentityResult.Errors));
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : ChangePassword : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public OperationResult ResetPassword()
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityAppDbContext));

                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : ResetPassword : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<OperationResult> AddRoleAsync(RoleDTO Role)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _Application = await _IdentityAppDbContext.Aplications
                        .Include(x => x.Roles.Select(y => y.RoleContentAuthorization))
                        .Where(x => x.Visible).FirstOrDefaultAsync(x => x.Name == Role.Name);
                    if (_Application == null)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("No existe la aplicación a la que intenta agregar el role."));
                        return _OperationResult;
                    }
                    var _ApplicationRole = _Application.Roles.FirstOrDefault(x => x.Name == Role.Name);
                    if (_ApplicationRole == null)
                    {

                        _ApplicationRole = new ApplicationRole()
                        {
                            Application = _Application,
                            Name = Role.Name,
                            RoleContentAuthorization = Role.ContentAuthorize?.Select(x => new Models.Identity.Base.RoleContentAuthorization()
                            {
                                Action = x.Action,
                                Module = x.Module,
                                Visible = true
                            }).ToList()
                        };
                        var _RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(_IdentityAppDbContext));
                        var _IdentityResult = await _RoleManager.CreateAsync(_ApplicationRole);
                        if (!_IdentityResult.Succeeded)
                            throw new Exception(string.Join(", ", _IdentityResult.Errors));
                    }
                    else
                    {
                        var _RoleContentAuthorization = Role.ContentAuthorize.Select(x => new Models.Identity.Base.RoleContentAuthorization()
                        {
                            Role = _ApplicationRole,
                            Action = x.Action,
                            Module = x.Module,
                            Visible = true
                        }).ToList();
                        _IdentityAppDbContext.RoleContentAuthorization.AddRange(_RoleContentAuthorization);
                        _IdentityAppDbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.LogText("RepositoryIdentity : AddRole : " + Error.InnerExceptionMessage(ex) + " " + Error.InnerExceptionStackTrace(ex));
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public OperationResult GetUserByEmail(string Email, ref ApplicationUser User)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                using (IdentityAppDbContext _IdentityAppDbContext = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_IdentityAppDbContext));
                    var _User = _UserManager.FindByEmail(Email);
                    if (_User == null)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("Usuario no encontrado."));
                        return _OperationResult;
                    }
                    else if (_User.LockoutEndDateUtc != null && _User.LockoutEndDateUtc > DateTime.Now)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NON_AUTHORITATIVE_INFORMATION);
                        _OperationResult.AddException(new Exception("El usuario se encuentra bloqueado, favor de contactar al Administrador del Sistema."));
                        return _OperationResult;
                    }
                    User = _User;
                }
            }
            catch (Exception ex)
            {
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<AspNetUsers> FindFullDataUserByEmailAsync(string Email)
        {
            using (IdentityAccoutns _Context = new IdentityAccoutns())
            {
                return await _Context.AspNetUsers.Include(x => x.AspNetRoles.Select(y => y.RoleContentAuthorization)).FirstOrDefaultAsync(x => x.Email.ToLower() == Email.ToLower());
            }
        }

        public async Task<bool> IsUserInRoleAsync(string Email, string Application, string Role)
        {
            using (IdentityAccoutns _Context = new IdentityAccoutns())
            {
                var _Users = await _Context.AspNetUsers.Include(x => x.AspNetRoles.Select(y => y.Aplications)).FirstOrDefaultAsync(x => x.Email.ToLower() == Email.ToLower());
                if (_Users == null)
                    return false;
                return _Users.AspNetRoles.Any(x => x.Aplications.Visible && x.Aplications.Name == Application && x.Name == Role);
            }
        }

        public async Task<ListAllRolesDTO> ListAllRolesAsync()
        {
            var _Response = new ListAllRolesDTO();
            try
            {
                using (IdentityAccoutns _Context = new IdentityAccoutns())
                {
                    var _IdentityRoles = await _Context.Aplications.Include(x => x.AspNetRoles.Select(y => y.RoleContentAuthorization))
                        .Where(x => x.Visible).ToListAsync();
                    _Response.Applications = _IdentityRoles.Select(x => new ApplicationResponseDTO()
                    {
                        Name = x.Name,
                        Roles = x.AspNetRoles.Select(y => new RoleDTO()
                        {
                            Name = y.Name,
                            ContentAuthorize = y.RoleContentAuthorization.Where(w => w.Visible).Select(w => new ContentAuthorizeDTO()
                            {
                                Action = w.Action,
                                Module = w.Module
                            }).ToList()
                        }).ToList()
                    }).ToList();
                    if (!(_Response.Applications != null && _Response.Applications.Count > 0))
                    {
                        _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _Response.Result.AddException(new Exception("No se encontraron Roles."));
                        return _Response;
                    }
                }
            }
            catch (Exception ex)
            {
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
                this._Logger.LogError(ex);
            }
            return _Response;
        }

        public string ReadFile(string PathFile)
        {
            string _Body = string.Empty;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(PathFile))
            {
                _Body = sr.ReadToEnd();
            }
            return _Body;
        }

        public string GeneratePassword(int StringLength)
        {
            Random rnd = new Random();
            string possibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int size = possibles.Length;
            string pass = "";
            for (int i = 0; i < StringLength; i++)
            {
                pass += possibles[rnd.Next(size)];
            }
            return pass;
        }

        public async Task<TreeViewResponseDTO> GetTreeViewAsync()
        {
            var _Response = new TreeViewResponseDTO();
            try
            {
                using (IdentityAccoutns _Context = new IdentityAccoutns())
                {
                    var _Applications = await _Context.Aplications.Include(x => x.AspNetRoles.Select(y => y.RoleContentAuthorization))
                        .Where(x => x.Visible).ToListAsync();
                    _Response.Items = _Applications.Select(x => new TreeViewItemDTO()
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name.Replace("MAC.", ""),
                        Checked = x.Visible,
                        Collapsed = true,
                        Children = x?.AspNetRoles.Select(y => new TreeViewItemDTO()
                        {
                            Value = y.Id,
                            Text = y.Name,
                            Checked = y.Visible,
                            Children = y?.RoleContentAuthorization.GroupBy(w => w.Module).Select(w => new TreeViewItemDTO()
                            {
                                Text = w.Key,
                                Value = w.Key,
                                Checked = true,
                                Children = w.Select(z => new TreeViewItemDTO()
                                {
                                    Text = z.Action,
                                    Value = z.Id.ToString(),
                                    Checked = z.Visible
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }
        public async Task<OperationResult> SaveChangesTreeViewAsync(TreeViewRequestDTO Request)
        {
            OperationResult _Response = new OperationResult();
            try
            {
                var _Applications = Request.Items.Select(x => new Models.Identity.DataBaseFirst.Aplications()
                {
                    Id = int.Parse(x.Value.Trim()),
                    Visible = x.Checked
                }).ToList();
                var _Roles = Request.Items.SelectMany(x => x?.Children).Select(x => new AspNetRoles()
                {
                    Id = x.Value.Trim(),
                    Visible = x.Checked
                }).ToList();
                var _RoleContent = Request.Items.SelectMany(x => x?.Children).SelectMany(x => x?.Children)
                    .SelectMany(x => x?.Children)
                    .Select(x => new Models.Identity.DataBaseFirst.RoleContentAuthorization()
                    {
                        Id = int.Parse(x.Value.Trim()),
                        Visible = x.Checked
                    }).ToList();
                using (IdentityAccoutns _Context = new IdentityAccoutns())
                {
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Applications.ForEach(x =>
                    {
                        _Context.Aplications.Attach(x);
                        _Context.Entry(x).Property(y => y.Visible).IsModified = true;
                    });
                    _Roles.ForEach(x =>
                    {
                        _Context.AspNetRoles.Attach(x);
                        _Context.Entry(x).Property(y => y.Visible).IsModified = true;
                    });
                    _RoleContent.ForEach(x =>
                    {
                        _Context.RoleContentAuthorization.Attach(x);
                        _Context.Entry(x).Property(y => y.Visible).IsModified = true;
                    });
                    await _Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.AddException(ex);
            }
            return _Response;
        }
        public async Task<List<TreeViewItemDTO>> GetTreeViewRolesAsync()
        {
            using (IdentityAccoutns _Context = new IdentityAccoutns())
            {
                var _Applications = await _Context.Aplications.Include(x => x.AspNetRoles.Select(y => y.RoleContentAuthorization))
                    .Where(x => x.Visible).ToListAsync();
                return _Applications.Select(x => new TreeViewItemDTO()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Checked = x.Visible,
                    Children = x?.AspNetRoles.Select(y => new TreeViewItemDTO()
                    {
                        Value = y.Id,
                        Text = y.Name,
                        Checked = true,
                        Children = y?.RoleContentAuthorization.GroupBy(w => w.Module).Select(w => new TreeViewItemDTO()
                        {
                            Text = w.Key,
                            Value = w.Key,
                            Checked = true,
                            Children = w.Select(z => new TreeViewItemDTO()
                            {
                                Text = z.Action,
                                Value = z.Id.ToString(),
                                Checked = z.Visible
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();
            }
        }
        public async Task<TreeViewResponseDTO> GetTreeViewRolesByUserAsync(string Email)
        {
            var _Response = new TreeViewResponseDTO();
            try
            {
                using (IdentityAccoutns _Context = new IdentityAccoutns())
                {
                    var _User = await _Context.AspNetUsers.Include(x => x.AspNetRoles).FirstOrDefaultAsync(x => x.Email == Email);
                    if (_User == null)
                    {
                        _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _Response.Result.AddException(new Exception("No se encontro el usuario " + Email + "."));
                        return _Response;
                    }
                    var _RolesId = _User?.AspNetRoles.Select(x => x.Id).ToList();
                    var _Applications = await _Context.Aplications.Include(x => x.AspNetRoles.Select(y => y.RoleContentAuthorization))
                        .Where(x => x.Visible).ToListAsync();
                    _Response.Items = _Applications.Select(x => new TreeViewItemDTO()
                    {
                        Text = x.Name.Replace("MAC.", ""),
                        Value = x.Id.ToString(),
                        Checked = true,
                        Children = x.AspNetRoles.Where(y => y.Visible).Select(y => new TreeViewItemDTO()
                        {
                            Text = y.Name,
                            Value = y.Id,
                            Checked = false,
                            Children = y?.RoleContentAuthorization.Where(z => z.Visible).GroupBy(z => z.Module)
                            .ToList().Select(z => new TreeViewItemDTO()
                            {
                                Text = z.Key,
                                Value = z.Key,
                                Disabled = true,
                                Checked = false,
                                Children = z.Select(w => new TreeViewItemDTO()
                                {
                                    Text = w.Action,
                                    Value = w.Id.ToString(),
                                    Checked = false,
                                    Disabled = true
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList();
                    _Response.Items.SelectMany(x => x?.Children).ToList().ForEach(x =>
                    {
                        x.Checked = _RolesId.Any(y => y == x.Value);
                        if (x.Children != null && x.Children.Count > 0)
                        {
                            x.Children.SelectMany(y => y?.Children)
                            .ToList().ForEach(y =>
                            {
                                y.Checked = x.Checked;
                            });
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }
        public async Task<OperationResult> SaveTreeViewRolesByUserAsync(SaveChangesRolesByUserRequestDTO Request)
        {
            var _Response = new OperationResult();
            try
            {
                var _RolesId = Request.Items;
                using (IdentityAccoutns _Context = new IdentityAccoutns())
                {
                    var _User = await _Context.AspNetUsers.Include(x => x.AspNetRoles).FirstOrDefaultAsync(x => x.Email == Request.Email);
                    if (_User == null)
                    {
                        _Response.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _Response.AddException(new Exception("No se encontro al usuario " + Request.Email + "."));
                        return _Response;
                    }
                    var _UserRoles = _User.AspNetRoles.Select(x => x.Id).ToList();
                    var _MissingRoles = _RolesId.Where(x => !_UserRoles.Contains(x)).ToList();
                    var _DeleteRoles = _UserRoles.Where(x => !_RolesId.Contains(x)).ToList();
                    if (!((_MissingRoles != null && _MissingRoles.Count > 0) || (_DeleteRoles != null && _DeleteRoles.Count > 0)))
                        return _Response;
                    _User.AspNetRoles = _User.AspNetRoles.Where(x => !_DeleteRoles.Contains(x.Id)).ToList();
                    var _Roles = _Context.AspNetRoles.Where(x => x.Aplications.Visible && x.Visible && _MissingRoles.Contains(x.Id)).ToList();
                    if (_Roles != null && _Roles.Count > 0)
                    {
                        _Roles.ForEach(x => { _User.AspNetRoles.Add(x); });
                    }
                    _Context.Entry(_User).State = EntityState.Modified;
                    _Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.AddException(ex);
            }
            return _Response;
        }
        public OperationResult Login(string UserName, string Password)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                using (IdentityAppDbContext _Context = new IdentityAppDbContext())
                {
                    var _UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_Context));
                    var _ApplicationUser = _UserManager.FindByEmail(UserName);
                    if (_ApplicationUser == null)
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                        _OperationResult.AddException(new Exception("No se encontro el usuario: " + UserName));
                        return _OperationResult;
                    }
                    else if (!_UserManager.CheckPassword(_ApplicationUser, Password))
                    {
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                        _OperationResult.AddException(new Exception("Contraseña incorrecta."));
                        return _OperationResult;
                    }
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<GetApplicationsResponseDTO> GetApplicationsAsync()
        {
            GetApplicationsResponseDTO _Response = new GetApplicationsResponseDTO();
            try
            {
                using (IdentityAccoutns _Context = new IdentityAccoutns())
                {
                    _Response.Applications = await _Context.Aplications
                        .Include(x => x.AspNetRoles.Select(y => y.RoleContentAuthorization))
                        .Where(x => x.Visible).Select(x => new ApplicationResponseDTO()
                        {
                            Name = x.Name.Replace("MAC.", ""),
                            Roles = x.AspNetRoles.Where(y => y.Visible).Select(y => new RoleDTO()
                            {
                                Name = y.Name,
                                ContentAuthorize = y.RoleContentAuthorization.Where(z => z.Visible).Select(z => new ContentAuthorizeDTO()
                                {
                                    Action = z.Action,
                                    Module = z.Module,
                                }).ToList()
                            }).ToList()
                        }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _Response.Result.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _Response.Result.AddException(ex);
            }
            return _Response;
        }
        public async Task<OperationResult> AddRolesAsync(AddRolesRequestDTO Request)
        {
            var _OperationResult = new OperationResult();
            try
            {
                switch (Request.ActionRole)
                {
                    case EnumActionRole.AddApplication:
                        return await this.AddApplicationAsync(Request?.Application?.AppName?.Trim().ToUpper());
                    case EnumActionRole.AddRole:
                        return await this.AddRoleAsync(Request?.Application?.AppName?.Trim().ToUpper(), Request?.Application?.RoleName?.Trim());
                    case EnumActionRole.AddContentAuthorization:
                        return await this.AddContentAuthorizationAsync(Request?.Application?.AppName?.Trim().ToUpper(), Request?.Application?.RoleName?.Trim(),
                            Request?.Application?.ContentAuthorize);
                    default:
                        _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                        _OperationResult.AddException(new Exception("La opcion es inválida, favor de validar."));
                        break;
                }
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<Aplications> FindApplicationByNameAsync(string Name)
        {
            using (IdentityAccoutns _Context = new IdentityAccoutns())
            {
                return await _Context.Aplications.Include(x => x.AspNetRoles).FirstOrDefaultAsync(x => x.Name == Name);
            }
        }
        public async Task<OperationResult> AddApplicationAsync(string Name)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _OperationResult.AddException(new Exception("El nombre de la aplicación es requerido."));
                    return _OperationResult;
                }
                var _Application = await this.FindApplicationByNameAsync("MAC." + Name);
                if (_Application != null)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.CONFLICT);
                    _OperationResult.AddException(new Exception("Ya existe una Aplicación con el nombre insertado."));
                    return _OperationResult;
                }
                _Application = new Models.Identity.DataBaseFirst.Aplications()
                {
                    Name = "MAC." + Name,
                    Visible = true
                };
                await this.AddAsync(_Application);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<OperationResult> AddRoleAsync(string AppName, string RoleName)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (string.IsNullOrEmpty(AppName) || string.IsNullOrEmpty(RoleName))
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _OperationResult.AddException(new Exception("El nombre de la aplicación y del role es requerido."));
                    return _OperationResult;
                }
                var _Application = await this.FindApplicationByNameAsync("MAC." + AppName);
                if (_Application == null)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                    _OperationResult.AddException(new Exception("No se encontro la aplicación " + AppName + "."));
                    return _OperationResult;
                }
                else if (_Application.AspNetRoles.FirstOrDefault(x => x.Name == RoleName) != null)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.CONFLICT);
                    _OperationResult.AddException(new Exception("El role " + RoleName + " ya existe, intente nuevamente con un nombre diferente."));
                    return _OperationResult;
                }
                var _Role = new AspNetRoles()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleName,
                    Visible = true,
                    Application_Id = _Application.Id
                };
                await this.AddAsync(_Role);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task<OperationResult> AddContentAuthorizationAsync(string AppName, string RoleName, List<ContentAuthorizeDTO> ContentAuthorization)
        {
            OperationResult _OperationResult = new OperationResult();
            try
            {
                if (string.IsNullOrEmpty(AppName) || string.IsNullOrEmpty(RoleName) || !(ContentAuthorization != null && ContentAuthorization.Count > 0))
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.BAD_REQUEST);
                    _OperationResult.AddException(new Exception("El nombre de la aplicación, de los roles y los contenidos autorizados."));
                    return _OperationResult;
                }
                var _Application = await this.FindApplicationByNameAsync("MAC." + AppName);
                if (_Application == null)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                    _OperationResult.AddException(new Exception("No se encontro la aplicación " + AppName + "."));
                    return _OperationResult;
                }
                var _Role = _Application.AspNetRoles.FirstOrDefault(x => x.Name == RoleName);
                if (_Role == null)
                {
                    _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.NOT_FOUND);
                    _OperationResult.AddException(new Exception("No se encontro el role " + RoleName + "."));
                    return _OperationResult;
                }
                var _ContentAuthorization = ContentAuthorization.Select(x => new Models.Identity.DataBaseFirst.RoleContentAuthorization()
                {
                    Action = x?.Action.Trim(),
                    Module = x?.Module.Trim(),
                    Visible = true,
                    Role_Id = _Role.Id
                }).ToList();
                await this.AddRangeAsync(_ContentAuthorization);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex);
                _OperationResult.SetStatusCode(OperationResult.StatusCodesEnum.INTERNAL_SERVER_ERROR);
                _OperationResult.AddException(ex);
            }
            return _OperationResult;
        }
        public async Task AddAsync<T>(T Register) where T : class
        {
            using (IdentityAccoutns _Context = new IdentityAccoutns())
            {
                _Context.Set<T>().Add(Register);
                await _Context.SaveChangesAsync();
            }
        }
        public async Task AddRangeAsync<T>(List<T> Range) where T : class
        {
            using (IdentityAccoutns _Context = new IdentityAccoutns())
            {
                _Context.Set<T>().AddRange(Range);
                await _Context.SaveChangesAsync();
            }
        }
    }
}
