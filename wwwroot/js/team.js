new Vue({
    el: '#app',
    data: {
        data: "",


        inputTitle: false,
        inputProjectType: false,
        inputDescription: false,


        isUserTeamLead: true,


        imgSrc: "",

        chatLoading: false,
        textChat: "",


        linksEdit: false,
        links: "",

    },
    created: function () {
        this.loadData();
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
                    this.links = this.data.Links;
                    console.log(this.data);
                }
            })
        },

        showInput(inputName) {
            if (this.data.isUserTeamLead) {
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
                        console.log(res);
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
                    console.log(res);
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
                    console.log(res);
                    this.loadData();
                }
            });
        },

        /*SetNewImg($event) {
            $event.preventDefault();
            let link = $("#img_link").val();
            let file = $("#img_file");
            console.log(link);
            console.log(file);
            let formData = new FormData($("#change_img_block")[0]);
            console.log(formData);
            if (link != "") {
                /*let Path = location.pathname.split("/");
                let TeamId = Path[Path.length - 1];

                $.ajax({
                    type: "POST",
                    url: "/changeInfoTeam/" + TeamId + "/" + inputName + "/" + data,
                    success: () => {
                        this.loadData();
                    }
                })
            }*/
    }
}
);