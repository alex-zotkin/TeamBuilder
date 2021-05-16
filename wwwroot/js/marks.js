new Vue({
    el: '#app',
    data: {
        data: [],
        sortDirection: "",
    },
    created: function () {
        this.loadData();
    },
    methods: {
        loadData() {
            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 2];
            $.ajax({
                type: "POST",
                url: "/allInfoAboutMarks/" + ProjectId,
                success: (data) => {
                    let n = data.split(";")
                    if (n[0] == "NOJURY") {
                        this.data = {};
                        this.data.Jury = "NOJURY";
                        this.data.Project = {};
                        this.data.Project.ProjectId = n[1];
                    }
                    else {
                        this.data = JSON.parse(data);
                    }
                   
                }
            })
        },

        activeInput(UserId){
            if (this.data.IsUserJury && this.data.CurrentUser.UserId == UserId) {
                return true;
            }
            return false;
        },

        setMark(TeamId, Mark, Stage) {
            $.ajax({
                type: "POST",
                data: {
                    "UserId": this.data.CurrentUser.UserId,
                    "TeamId": TeamId,
                    "Stage": Stage,
                    "Mark": Mark
                },
                url: "/setMark",
                success: (data) => {
                    this.loadData();
                }
            })
        },

        marksOpen(set) {
            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 2];
            $.ajax({
                type: "POST",
                data: {
                    "ProjectId": ProjectId,
                    "Set": set
                },
                url: "/openMarks",
                success: (data) => {
                    this.loadData();
                }
            })
        },

        download() {
            let Path = location.pathname.split("/");
            let ProjectId = Path[Path.length - 2];
            $.ajax({
                type: "POST",
                data: {
                    "ProjectId": ProjectId,
                },
                url: "/download",
                success: () => {
                }
            })
        },

        sortTable() {
            if (this.sortDirection == "▲") {
                //сортировка по убыванию
                this.data.Teams.sort(function (prev, next) {
                    return (prev.Summary > next.Summary) ? -1 : 1;
                });
                console.log(this.data.Teams);
            } else if (this.sortDirection == "▼") {
                //сортировка по возрастанию
                this.data.Teams.sort(function (prev, next) {
                    return (prev.Summary < next.Summary) ? -1 : 1;
                });
            }
        },

        changeSortDirection() {
            if (this.sortDirection == "" || this.sortDirection == "▼")
                this.sortDirection = "▲";
            else if (this.sortDirection == "▲")
                this.sortDirection = "▼";
            this.sortTable();
        }
    }
});