#pragma checksum "C:\Users\PC Master\Desktop\Проект КОНСТРУКТОР КОМАНД\TeamBuilder\Views\Project\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "240a486a5dd307bc3038d44f8e2c25199dfb332f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Project_Index), @"mvc.1.0.view", @"/Views/Project/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\PC Master\Desktop\Проект КОНСТРУКТОР КОМАНД\TeamBuilder\Views\_ViewImports.cshtml"
using TeamBuilder;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\PC Master\Desktop\Проект КОНСТРУКТОР КОМАНД\TeamBuilder\Views\_ViewImports.cshtml"
using TeamBuilder.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"240a486a5dd307bc3038d44f8e2c25199dfb332f", @"/Views/Project/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"15b89ea05c964198ec05415b907ee84874fb63c3", @"/Views/_ViewImports.cshtml")]
    public class Views_Project_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/project.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/project.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "240a486a5dd307bc3038d44f8e2c25199dfb332f4194", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

<div id=""app"" v-cloak>
    <delete_alert v-if=""deleteAlert"" v-on:deletealert=""deletealert""></delete_alert>

    <add_admin v-if=""addAdminBox""
               :notadminsproject=""notadminsproject""
               :adminsproject=""adminsproject""
               v-on:addadmin=""addadmin""
               v-on:deleteadmin=""deleteadmin""
               v-on:close=""addAdminBox = false""></add_admin>


    <add_jury v-if=""addJuryBox""
              :notadminsproject=""notjuryproject""
              :adminsproject=""juryproject""
              v-on:addjury=""addjury""
              v-on:deleteadmin=""deletejury""
              v-on:close=""addJuryBox = false""></add_jury>

    <info v-if=""infoBool"" v-bind:infoposition=""infoposition"" v-bind:infodata=""infoData""></info>

    <div class=""project_actions_box"" :style=""'width: calc(100% - ' + project_width  + 'px );'"">
        <h3 v-if=""!editMode"">{{data.Project.Name}}</h3>
        <input id=""projectName"" v-else type=""text"" placeholder=""Название проектной деятельности""");
            WriteLiteral(" :value=\"data.Project.Name\">\r\n\r\n        <div class=\"admin_actions\" v-if=\"data.IsUserAdmin\">\r\n            <div class=\"button button_red\" v-if=\"!editMode\" v-on:click=\"deleteAlert=true\">Удалить ПД</div>\r\n");
            WriteLiteral(@"        </div>
    </div>



    <div class=""news_box"" :style=""'right:' + news_pos + 'px'"">
        <h5 class=""box_close_title"" v-if=""news_pos_change"">Новости</h5>
        <h5 style=""margin-left: -80px"">
            <span style=""margin-right: 60px"" class=""span_arrow"" v-on:click=""box_resize('news')"" v-if=""!news_pos_change"">▶</span>
            <span style=""margin-right: 60px"" class=""span_arrow"" v-on:click=""box_resize('news')"" v-else>◀</span>
            Новости
        </h5>
        <div class=""news"" v-if=""!news_pos_change"">
            <ul>
                <li v-for=""news in data.News"">
                    <span class=""delete_news"" v-on:click=""deleteNew(news.NewId)"" v-if=""data.IsUserAdmin"">❌</span>
                    <img :src=""news.Author.Photo50"" :title=""news.Author.FirstName + ' ' + news.Author.LastName"" style=""border-radius: 100px; width: 45px; margin-left: -18px;"" />
                    <p style=""margin-bottom: 10px;"">{{news.Text}}</p>
                    <br>
                    <spa");
            WriteLiteral(@"n class=""news_data"">{{news.Date}}</span>
                </li>
            </ul>
            <textarea v-if=""data.IsUserAdmin"" :disabled=""news_textarea_desabled"" rows=""7"" placeholder=""Текст новости (для отправки нажмите Enter)"" v-model=""news_text"" v-on:keyup.enter.preventDefault=""addNew()""></textarea>
        </div>

    </div>



    <div class=""user_list_box"" :style=""'right:' + admins_pos + 'px'"">
        <h5 class=""box_close_title"" v-if=""admins_pos_change"" style=""left:-55px; top: 160px;"">Администраторы</h5>
        <h5 style=""margin-left: -25px; margin-bottom: 20px;"">
            <span style=""margin-right: 10px"" class=""span_arrow"" v-on:click=""box_resize('admin')"" v-if=""!admins_pos_change"">▶</span>
            <span style=""margin-right: 10px"" class=""span_arrow"" v-on:click=""box_resize('admin')"" v-else>◀</span>
            Администраторы
        </h5>
        <ul v-if=""!admins_pos_change"">
            <li v-on:mousemove=""ShowInfo($event, admin)"" v-for=""admin in data.ProjectAdmins""
         ");
            WriteLiteral(@"       v-on:mouseleave=""infoBool = false"">
                <a target=""_blank"" :href=""'https://vk.com/id' + admin.VkId"">
                    <img :src=""admin.Photo50"">
                    <p>{{admin.FirstName}} {{admin.LastName}}</p>
                </a>
            </li>
            <li class=""button button_green add_admin_button"" v-on:click=""addAdminBox = true"" v-if=""data.IsUserAdmin"">
                Изменить
            </li>
        </ul>

        <h5 v-if=""!admins_pos_change"">Жюри</h5>
        <ul v-if=""!admins_pos_change"" style=""margin-bottom: 50px;"">
            <li v-on:mousemove=""ShowInfo($event, jury)"" v-for=""jury in data.ProjectJury""
                v-on:mouseleave=""infoBool = false"">
                <a target=""_blank"" :href=""'https://vk.com/id' + jury.VkId"">
                    <img :src=""jury.Photo50"">
                    <p>{{jury.FirstName}} {{jury.LastName}}</p>
                </a>
            </li>
            <li class=""button button_green add_admin_button"" v-on:click=""a");
            WriteLiteral(@"ddJuryBox = true"" v-if=""data.IsUserAdmin"">
                Изменить
            </li>
        </ul>
        <h5 v-if=""!admins_pos_change"">Пользователи, <br> вступившие в команды</h5>
        <ul v-if=""!admins_pos_change"">
            <li v-for=""user in data.Users"" v-on:mousemove=""ShowInfo($event, user)""
                                           v-on:mouseleave=""infoBool = false"">
                <a");
            BeginWriteAttribute("href", " href=\"", 4962, "\"", 4969, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                    <img :src=""user.Photo50"">
                    <p>{{user.FirstName}} {{user.LastName}}</p>
                </a>
            </li>
        </ul>
    </div>


    <div class=""project_list"" :style=""'width: calc(100% - ' + project_width  + 'px );'"">
        <!--<div class=""team team_disabled"" v-if=""editMode"" v-for=""n in 8"">
            <h3 style=""cursor:default;"">Команда {{n}}</h3>
            <p v-if=""editMode"">Пока ни один студент <br> не вступил в данную команду</p>
            <h2 v-if=""editMode"">3 Х 2</h2>
        </div>-->

        <div class=""team team_active"" v-for=""team in data.Teams"" :style=""{borderColor: (data.IsUserInTeam == team.TeamId) ? 'rgba(0,0,255,0.4)' : 'lightgray', background: (data.IsUserInTeam == team.TeamId) ? 'rgba(0,0,255,0.04)' : 'white'}"">
            <span class=""team_status"" style=""background-color: #ff9900"" v-if=""(typeof applications[team.TeamId] != 'undefined') && !applications[team.TeamId].Checked"">Ваша заявка обрабатывается &nbsp; <img style=");
            WriteLiteral(@"""width:20px;"" src=""https://i.gifer.com/XVo6.gif"" /></span>
            <span class=""team_status"" style=""background-color: rgba(20, 146, 8, 0.952)"" v-else-if=""data.IsUserInTeam != team.TeamId"">Запись открыта</span>
            <span class=""team_status"" style=""background-color: rgba(0,0,255,0.8)"" v-else-if=""data.IsUserInTeam == team.TeamId"">Вы состоите в данной команде</span>
            <span class=""team_delete"" v-if=""editMode"">❌</span>
            <div class=""team_img"" :style="" 'background-image: url(' + team.Img +  ')'""></div>

            <div class=""team_content"">
                <div class=""team_block"">
                    <span>Проект</span>
                    <h3 class=""team_title"" v-if=""!editMode"">{{team.Title}}</h3>
                </div>
                <input type=""text"" :value=""team.Title"" style=""width: 80%; margin-top: -30px; margin-bottom: 15px;"" v-if=""editMode"" />

                <div class=""team_block"">
                    <span>Тип проекта</span>
                    <p class=""");
            WriteLiteral(@"team_description"">
                        {{team.Type}}
                    </p>
                </div>

                <div class=""team_block"">
                    <span>Описание</span>
                    <p class=""team_description"">
                        {{team.Description}}
                    </p>
                </div>

                <div class=""team_block"">
                    <span>Требования</span>
                    <p class=""team_count"" v-if=""!editMode"">{{team.MaxCount1}} перв. Х {{team.MaxCount2}} втор.</p>
                    <p class=""team_count"" v-if=""editMode"">
                        <input type=""number"" min=""1"" :value=""team.MaxCount1"" style=""width: 35px; padding: 2px;"" /> перв.
                        Х <input type=""number"" min=""1"" :value=""team.MaxCount2"" style=""width: 35px; padding: 2px;"" /> втор.
                    </p>
                </div>

                <div class=""team_block"">
                    <span>Участники  &nbsp; <span :style=""{color: (team.Count");
            WriteLiteral(@"1 == team.MaxCount1) ? 'red' : 'green'}"">{{team.Count1}}/{{team.MaxCount1}}</span> &nbsp; <span :style=""{color: (team.Count2 == team.MaxCount2) ? 'red' : 'green'}"">{{team.Count2}}/{{team.MaxCount2}}</span></span>
                    <div class=""team_users"">
                        <p v-if=""data.TeamUsers[team.TeamId].length == 0"">Никто еще не вступил в эту команду...</p>
                        <a v-for=""u in data.TeamUsers[team.TeamId]"" :href=""'https://vk.com/id' + u[2]"" :style=""{backgroundImage: 'url(' + u[3] + ')'}""></a>
                    </div>
                </div>

                <div class=""team_buttons"">
                    <button class=""button button_green"" v-if=""!(data.IsUserAdmin || data.IsUserJury || data.IsUserInTeam != -1) && !data.UserApplicationTeamsId.includes(team.TeamId)"" v-on:click=""joinTeam(team.TeamId)"">Вступить</button>
                    <button class=""button button_orange"" v-else-if=""(typeof applications[team.TeamId] != 'undefined') && !applications[team.TeamId].Checked");
            WriteLiteral(@""" v-on:click=""deleteApplication(team.TeamId, data.CurrentUser.UserId)"">Отменить заявку</button>
                    <button class=""button button_red"" v-else-if=""(typeof applications[team.TeamId] != 'undefined') && applications[team.TeamId].Checked && !applications[team.TeamId].Successed"" v-on:click=""deleteApplication(team.TeamId, data.CurrentUser.UserId)"">Ваша заявка отклонена</button>
                    <button class=""button button_red"" v-else-if=""data.IsUserInTeam == team.TeamId"" v-on:click=""exitTeam(team.TeamId)"">Выйти</button>
                    <a :href=""'/project/' + team.ProjectId + '/team/' + team.TeamId"" class=""button button_grey"">Подробнее</a>
                    <!--'project/' + team.ProjectId + '/team/' + team.TeamId-->
                </div>
            </div>
        </div>

        <!--<div class=""team team_disabled"" v-if=""editMode"">
            <input type=""text"" placeholder=""Название команды"" value=""Команда"" style=""text-align: center;"">

            <div class=""editSizeInput"">
");
            WriteLiteral(@"                <div class=""label"">
                    <label>1 курс</label>
                    <input type=""number"" value=""3"">
                </div>
                <h2>Х</h2>
                <div class=""label"">
                    <label>2 курс</label>
                    <input type=""number"" value=""2"">
                </div>
            </div>
        </div>-->




        <div class=""team add_team"" v-if=""editMode"">
            <h1>+</h1>
            <p>Добавить команды</p>
        </div>

    </div>

</div>

");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "240a486a5dd307bc3038d44f8e2c25199dfb332f16523", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
