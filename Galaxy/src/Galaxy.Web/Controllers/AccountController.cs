using Abp.Web.Models;
using Galaxy.Users;
using Galaxy.Users.Dto;
using Galaxy.Web.Configuration;
using Galaxy.Web.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Galaxy.Web.Controllers
{
    public class AccountController : GalaxyControllerBase
    {
        #region 对象初始化和依赖注入

        private readonly UserAppService appService;
        private readonly QQLoginSettings qq;
        private readonly WeiboLoginSettings weibo;
        private readonly WechatLoginSettings wechat;
        public AccountController(UserAppService _appService, IOptions<QQLoginSettings> _qq, IOptions<WeiboLoginSettings> _weibo, IOptions<WechatLoginSettings> _wechat)
        {
            appService = _appService;
            qq = _qq.Value;
            weibo = _weibo.Value;
        }

        #endregion

        #region 登录

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<JsonResult> Login(LoginViewModel loginModel)
        {
            int loginStatus = appService.GetLoginStatus(loginModel.UsernameOrEmailAddress, loginModel.Password);
            if (loginStatus == 0)
            {
                //_signInManager.SignInAsync(loginResult.Identity, loginModel.RememberMe);
                //Identity身份验证， 根据Authentcation生成Token，设置Cookie
                /*
                var user = _userService.Login(userName, password);
                user.AuthenticationType = CookieAuthenticationDefaults.AuthenticationScheme;
                var identity = new ClaimsIdentity(user);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserID));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                */
                //缓存用户名
                ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, loginModel.UsernameOrEmailAddress) }, CookieAuthenticationDefaults.AuthenticationScheme));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties { IsPersistent = loginModel.RememberMe });
                //HttpContext.Session.SetString("UserName", AbpSession.GetUserName());
                //HttpContext.Session.SetString("UserName", loginModel.UsernameOrEmailAddress);
            }
            return Json(new AjaxResponse { Result = loginStatus });
        }

        /// <summary>
        /// 第三方QQ登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult LoginQQ()
        {
            /* 从配置文件获取对应的值
             * 1、Program类中添加对应的配置文件引用ConfigureAppConfiguration(option => { option.AddJsonFile("appsettings.json", false, false);})
             * 2、需要新建一个对应配置文件中的Key的类，类名需要和配置文件的名字一致
             * 3、Startup类中添加 public IConfiguration Configuration { get; }并在构造函数中初始化
             * 4、Startup类ConfigureServices方法中添加 services.AddOptions();和 services.Configure<对应配置文件的类>(Configuration.GetSection("对应配置文件的类"));
             * 5、Controllers中添加对应类的实例，并在构造函数中完成依赖注入
             * 6、直接使用类名.属性就可以获取对应配置文件的值
             */
            string state = new Random(100000).Next(99, 99999).ToString();//随机数
            HttpContext.Session.SetString("QQState", state);
            string appID = qq.QQAppID;
            string qqAuthorizeURL = qq.QQAuthorizeURL;
            string callback = qq.QQCallBack;
            string authenticationUrl = $"{qqAuthorizeURL}?client_id={appID}&response_type=code&redirect_uri={callback}&state={state}";//QQ互联地址
            return new RedirectResult(authenticationUrl);
        }

        [AllowAnonymous]
        public IActionResult LoginWeibo()
        {
            string state = new Random(100000).Next(99, 99999).ToString();//随机数
            HttpContext.Session.SetString("WeiboState", state);
            string appID = weibo.WeiboAppID;
            string qqAuthorizeURL = weibo.WeiboAuthorizeURL;
            string callback = weibo.WeiboCallBack;
            string authenticationUrl = $"{qqAuthorizeURL}?client_id={appID}&redirect_uri={callback}&state={state}";//要转跳到微博验证的地址
            return new RedirectResult(authenticationUrl);
        }

        [AllowAnonymous]
        public IActionResult LoginWechat()
        {
            string state = new Random(100000).Next(99, 99999).ToString();//随机数
            HttpContext.Session.SetString("WechatState", state);
            string appID = wechat.WechatAppID;
            string qqAuthorizeURL = wechat.WechatAuthorizeURL;
            string callback = wechat.WechatCallBack;
            string authenticationUrl = $"{qqAuthorizeURL}?client_id={appID}&redirect_uri={callback}&state={state}";//要转跳到微博验证的地址
            return new RedirectResult(authenticationUrl);
        }

        #endregion

        #region 注销

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return RedirectToRoute(new { controller = "Home", action = "Index" });
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region 注册

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="registermodel"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<JsonResult> Register(RegisterUserDto entity)
        {
            //数据写进数据库后，执行Identity身份验证， 根据Authentcation生成Token，设置Cookie，直接跳转到首页
            return Json(new AjaxResponse { Result = "" });
        }

        #endregion

        #region 其他

        /// <summary>
        /// 验证用户名是否唯一
        /// </summary>
        public virtual JsonResult CheckUnique(string name)
        {
            bool result = true;
            List<RegisterUserDto> Users = appService.GetRegisterUsers();
            foreach (RegisterUserDto user in Users)
            {
                if (user.UserName.Equals(name))
                {
                    result = false;
                    break;
                }
            }
            Dictionary<string, bool> dic = new Dictionary<string, bool>
            {
                { "valid", result }
            };
            //转化为Json输出
            //返回数据
            return Json(new AjaxResponse { Result = JsonConvert.SerializeObject(dic) });
        }

        #endregion
    }
}