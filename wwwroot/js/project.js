Vue.component("info", {
    data: function () {
      return {
        
      }
    },
    props:{
        infoposition: Object,
        infodata:Object
    },
    computed: {
        style(){
            return`left: ${this.infoposition.x}; top: ${this.infoposition.y};`;
        }
    },
    template: 
    `<div class="info" :style=style>
        <ul>
            <li><p>{{infodata.FirstName}} {{infodata.LastName}}</p></li>
            <li v-if="infodata.Course != 3">{{infodata.Course}} курс, {{infodata.Group}} группа</li>
            <li v-else>Администратор,<br> {{infodata.Group}} группа</li>
            <li v-if="infodata.Course != 3">С#, C++, Pascal</li>

            <li v-if="infodata.Course != 3">{{infodata.Description}}</li>
        </ul>
    </div>`
});

Vue.component("delete_alert",{
    data: function () {
        return {
          
        }
      },
      template: 
      `<div class="alert">
           <div class="alert_text">
                <h3>Удаление проектной деятельности</h3>
                <p>Вы собираетесь ПОЛНОСТЬЮ удалить все данные о ПД.
                    <br>Это действие НЕВОЗМОЖНО отменить. Вы уверены?
                </p>
                <div class="alert_buttons">
                    <div class="button button_red">Удалить</div>
                    <div class="button button_grey" v-on:click="closeAlert">Отмена</div>
                </div>
           </div>

      </div>
      `,
      methods:{
          closeAlert(){
            this.$emit('deletealert', false);
          }
      }
});

Vue.component("add_admin", {
    data: () => {
        return {
            adminName: '',
            notAdmins: [],
            admins: [],
        }
    },
    props: {
        notadminsproject: Array,
        adminsproject: Array,
    },
    template:
        `
    <div class="alert">
    <div class="admin_box">
        <h3>Добавление администратора</h3>
        <!--<input v-model="adminName" type="text" placeholder="Введите имя администратора" style="width : 80%">-->
        <div class="blocks">
            <div class="not_add_admin_list">
                <ul >
                    <li><h4>Доступные администраторы:</h4></li>
                    <li v-for="admin in this.notadminsproject"><img :src="admin.Photo50"> <p>{{admin.FirstName}} {{admin.LastName}}</p> <button class="button button_green" v-on:click="emitAdd(admin.VkId)">+</button></li>
                </ul>
            </div>

            <div class="add_admin_list">
                <ul>
                    <li><h4>Администраторы проекта:</h4></li>
                    <li v-for="admin in this.adminsproject"><img :src="admin.Photo50"> <p>{{admin.FirstName}} {{admin.LastName}}</p> <button class="button button_red" v-on:click="$emit('deleteadmin',admin.VkId)">-</button></li>
                </ul>
            </div>
        </div>
        <div class="add_admin_buttons">
            <button class="button button_green" v-on:click="$emit('close')">Сохранить</button>
        </div>
    </div>
    </div>
    `,
    /*created() {
        this.notAdmins = this.notadminsproject;
        this.admins = this.adminsproject;
    },
    watch: {
        adminName(newName) {
            this.notAdmins = this.notadminsproject;
            this.notAdmins = this.notadminsproject.filter(function (admin) {
                str = admin.name + " " + admin.lastName;
                return str.toLowerCase().includes(newName.toLowerCase());
            });
        },

    },*/
    methods: {
        emitAdd(VkId) {
            this.adminName = "";
            this.$emit('addadmin', VkId);
            this.notAdmins = this.notadminsproject;
            this.admins = this.adminsproject;
        },
    }
  
});






new Vue({
    el: "#app",
    data: {
        data: [],

        infoBool: false,
        infoposition: {x:"", y:""},
        infoData: {},

        editMode:false,

        deleteAlert: false,

        addAdminBox: false,
        alladmins: [],

        news_text: "",
        news_textarea_desabled: false,
        /*alladmins: [
            { name: "Александр", lastName: "Зоткин", VkId: 123456, isAdmin: true },
            { name: "Яна", lastName: "Михална", VkId: 564987, isAdmin: false },
            { name: "Ирина", lastName: "Кто?", VkId: 109316, isAdmin: false },
            { name: "Алина", lastName: "Рамазанова", VkId: 1235456, isAdmin: false },
            { name: "Анна", lastName: "Татаринова", VkId: 56987, isAdmin: false },
            { name: "Анастасия", lastName: "Михалкова", VkId: 1093, isAdmin: false }
        ],*/
        adminsproject: [],
        notadminsproject: [],

        admins_pos: 0,
        admins_pos_change: false,
        news_pos: 300,
        news_pos_change: false,
        project_width: 600,
    },
    created: function () {
        this.loadData();


        let Path = location.pathname.split("/");
        let ProjectId = Path[Path.length - 1];
        let cookieNews = ProjectId + "news";
        let cookieAdmins = ProjectId + "admins";
        if ($.cookie(cookieAdmins) == "close") {
            this.admins_pos_change = true;
            this.admins_pos = -235;
            this.project_width -= 235;
        }

        if ($.cookie(cookieNews) == "close") {
            this.news_pos_change = true;
            this.news_pos -= 235 - this.admins_pos;
            this.project_width -= 235;
        } else {
            this.news_pos += this.admins_pos;
        }
        /*for (let i = 0; i < this.alladmins.length; i += 1) {
            if (this.alladmins[i].isAdmin) {
                this.adminsproject.push(this.alladmins[i]);
            } else {
                this.notadminsproject.push(this.alladmins[i]);
            }
        };*/
    },
    methods:{
        ShowInfo($event, user) {
            this.infoData = user;
            let maxHeight = window.innerHeight;
            if($event.pageY >= maxHeight*0.7){
                this.infoposition.x = $event.pageX-265 + "px";
                this.infoposition.y = $event.pageY-240 + "px";
            }
            else{
                this.infoposition.x = $event.pageX-265 + "px";
                this.infoposition.y = $event.pageY + "px";
            }
            
            this.infoBool = true;          
        },

        StartEdit(){
            this.editMode = true;
        },

        EndEdit(){
            //Сохранение нового имени проектной деятельности
            let newProjectName = document.getElementById("projectName").value;
            console.log(newProjectName);
            if (this.data.Project.Name != newProjectName) {
                let Path = location.pathname.split("/");
                let ProjectId = Path[Path.length - 1];
                $.ajax({
                    type: "POST",
                    url: "/changeProjectName/" + ProjectId + "/" + newProjectName,
                    success: (data) => {
                        this.loadData();
                    }
                });
            }



            //Завершение редактирования
            this.editMode = false;
        },

        deletealert(){
            this.deleteAlert = false;
        },

        loadData() {
            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                url: "/allInfoAboutProject/" + ProjectId,
                success: (data) => {
                    this.data = JSON.parse(data);
                    this.notadminsproject = this.data.AllAdmins;
                    this.adminsproject = this.data.ProjectAdmins;
                    console.log(this.data);
                }
            });
        },

        addadmin(VkId) {
            let index = 0;
            for (let i = 0; i < this.notadminsproject.length; i += 1) {
                if (this.notadminsproject[i].VkId == Number(VkId)) {
                    index = i;
                    break;
                }
            }
            this.adminsproject.push(this.notadminsproject[index]);
            this.notadminsproject.splice(index, 1);

            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                url: "/addInProjectAdmin/" + ProjectId + "/" + VkId,
                success: (data) => {
                    //this.loadData();
                }
            });

        },

        deleteadmin(VkId) {
            if ((this.adminsproject.length > 1)) {
                let index = 0;
                for (let i = 0; i < this.adminsproject.length; i += 1) {
                    if (this.adminsproject[i].VkId == Number(VkId)) {
                        index = i;
                        break;
                    }
                }
                this.notadminsproject.push(this.adminsproject[index]);
                this.adminsproject.splice(index, 1);

                let Path = location.pathname.split("/");
                let ProjectId = Path[Path.length - 1];
                $.ajax({
                    type: "POST",
                    url: "/deleteFromProjectAdmin/" + ProjectId + "/" + VkId,
                    success: (data) => {
                        //this.loadData();
                    }
                });
            }
        },

        addNew() {
            let text = this.news_text;
            this.news_textarea_desabled = true; 
            this.news_text = "Загрузка...";
            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                url: "/addInProjectNews/" + ProjectId + "/" + this.data.CurrentUser.VkId + "/" + text,
                success: (data) => {
                    this.news_text = "";
                    this.loadData();
                    this.news_textarea_desabled = false;
                }
            });
        },

        deleteNew(NewId) {
            $.ajax({
                type: "POST",
                url: "/deleteInProjectNews/"  + NewId,
                success: (data) => {
                    this.loadData();
                }
            });
        },

        box_resize(box) {
            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 1];
            
            let cookieNews = ProjectId + "news";
            let cookieAdmins =  ProjectId + "admins";

            if (box == "admin") {
                if (this.admins_pos_change) {
                    this.admins_pos += 235;
                    this.news_pos += 235;
                    this.project_width += 235;
                    this.admins_pos_change = false;
                    $.removeCookie(cookieAdmins);
                }
                else {
                    this.admins_pos -= 235;
                    this.news_pos -= 235;
                    this.project_width -= 235;
                    this.admins_pos_change = true;
                    $.cookie(cookieAdmins, "close");
                }
            }

            else if (box == "news") {
                if (this.news_pos_change) {
                    this.news_pos += 235;
                    this.project_width += 235;
                    this.news_pos_change = false;
                    $.removeCookie(cookieNews);
                }
                else {
                    this.news_pos -= 235;
                    this.project_width -= 235;
                    this.news_pos_change = true;
                    $.cookie(cookieNews, "close");
                }
            }
        }

    },
        
});