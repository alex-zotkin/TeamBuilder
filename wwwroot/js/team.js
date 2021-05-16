new Vue({
    el: '#app',
    data: {
        data: "",


        inputTitle: false,
        inputProjectType: false,
        inputDescription: false,
        inputCount1: false,
        inputCount2: false,


        isUserTeamLead: true,


        imgSrc: "",

        chatLoading: false,
        textChat: "",


        linksEdit: false,
        links: "",

    },
    created: function () {
        this.loadData();
        this.interval = setInterval(this.loadData, 500);
    },
    methods: {
        loadData() {
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                url: "/allInfoAboutTeam/" + TeamId,
                success: (data) => {
                    this.data = JSON.parse(data);

                    for (let i = 0; i < this.data.Files.length; i += 1) {
                        let n = this.data.Files[i].Name.split("-");
                        let s = "";

                        for (let j = 1; j < n.length; j += 1) {
                            s += n[j];
                        }
                        this.data.Files[i].ShortName = s;
                    }

                    this.links = this.data.Links;
                    //console.log(this.data);
                }
            })
        },

        showInput(inputName) {
 
            if (this.data.isUserTeamLead || this.data.isUserAdmin) {
                this.sendChanges();
                switch (inputName) {
                    case "inputTitle":
                        this.inputTitle = true;
                        break;
                    case "inputProjectType":
                        this.inputProjectType = true;
                        break;
                    case "inputDescription":
                        this.inputDescription = true;
                        break;
                    case "inputCount1":
                        this.inputCount1 = this.data.isUserAdmin;
                        break;
                    case "inputCount2":
                        this.inputCount2 = this.data.isUserAdmin;
                        break;
                }
                $("[name = " + inputName + "]:first").focus();
            }
        },

        sendChanges(inputName) {
            let data = $("[name = " + inputName + "]").val();
            if (inputName != undefined) {
                let Path = location.pathname.split("/");
                let TeamId = Path[Path.length - 1];

                $.ajax({
                    type: "POST",
                    url: "/changeInfoTeam/" + TeamId + "/" + inputName + "/" + data,
                    success: () => {
                        this.loadData();
                    }
                })
            }

            this.inputTitle = false;
            this.inputProjectType = false;
            this.inputDescription = false;
            this.inputCount1 = false;
            this.inputCount2 = false;
        },

        setTeamLead(UserId) {
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                url: "/setteamlead",
                data: {
                    "UserId": UserId,
                    "TeamId": TeamId
                },
                success: () => {
                    this.loadData();
                }
            })
        },

        changeImg() {
            if (this.imgSrc != "") {
                let Path = location.pathname.split("/");
                let TeamId = Path[Path.length - 1];
            
                $.ajax({
                    type: "POST",
                    data: {
                        "ImgSrc": this.imgSrc,
                        "TeamId": TeamId,
                    },
                    url: "/changeTeamImg",
                    success: (res) => {
                        //console.log(res);
                        this.loadData();
                    }
                });
            }
        },

        sendMes() {
            if (this.textChat != "") {
                this.chatLoading = true;
                let Path = location.pathname.split("/");
                let TeamId = Path[Path.length - 1];

                $.ajax({
                    type: "POST",
                    data: {
                        "TeamId": TeamId,
                        "UserId": this.data.CurrentUser.UserId,
                        "Text": this.textChat
                    },
                    url: "/sendMes",
                    success: () => {
                        this.chatLoading = false;
                        this.textChat = "";
                        this.loadData();
                    }
                });
            }
        },

        delMes(ChatId) {
            this.chatLoading = true;
            $.ajax({
                type: "POST",
                data: {
                    "ChatId": ChatId
                },
                url: "/delMes",
                success: () => {
                    this.chatLoading = false;
                    this.loadData();
                }
            });
        },

        startEditLinks() {
            this.linksEdit = true;
            clearInterval(this.interval);
        },

        addLink() {
            if (this.links.length == 0) {
                this.links.push(
                    {
                        Id: 1,
                        Name: "",
                        Value: "",
                    }
                );
                return;
            }
            let id = this.links[this.links.length - 1].Id + 1;
            this.links.push(
                {
                    Id: id,
                    Name: "",
                    Value: "",
                }
            );
        },

        delLink(id) {
            for (let i = 0; i < this.links.length; i += 1) {
                if (this.links[i].Id == id) {
                    this.links.splice(i, 1);
                    break;
                }
            }
        },

        delFile(id) {
            this.linksEdit = false;
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                data: {
                    "Id": id,
                },
                url: "/delFile",
                success: (res) => {
                    //console.log(res);
                    this.loadData();
                }
            });
        },

        saveLinks() {
            this.linksEdit = false;
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                data: {
                    "Links": this.links,
                    "TeamId": TeamId
                },
                url: "/saveLinks",
                success: (res) => {
                    this.interval = setInterval(this.loadData, 500);
                    this.loadData();
                }
            });
        },

        joinTeam() {
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1];
            if (this.data.CurrentUser.Course == 3) {
                alert("Администраторы не могу вступать в команды студентов. Поменяйте свою роль в профиле и повторите попытку");
            } else {
                let Path = location.pathname.split("/");
                let ProjectId = Path[Path.length - 1];
                $.ajax({
                    type: "POST",
                    url: "/joinTeam/" + this.data.CurrentUser.VkId + "/" + TeamId,
                    success: (data) => {
                        this.loadData();
                    }
                });
            }
        },

        exitTeam() {
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1];
            $.ajax({
                type: "POST",
                url: "/exitTeam/" + this.data.CurrentUser.VkId + "/" + TeamId,
                success: (data) => {
                    this.loadData();
                }
            });
        },


        deleteApplication() {
            let Path = location.pathname.split("/");
            let TeamId = Path[Path.length - 1]
            $.ajax({
                type: "POST",
                url: "/notifications/deleteApplication/" + TeamId + "/" + this.data.CurrentUser.UserId,
            })
                .always(data => {
                    this.loadData();
                });
        },
    }
}
);