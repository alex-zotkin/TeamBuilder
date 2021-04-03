new Vue({
    el: '#app',
    data: {
        data: [],
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
                    this.data = JSON.parse(data);
                    console.log(this.data);
                }
            })
        },

        activeInput(UserId){
            if (this.data.IsUserJury && this.data.CurrentUser.UserId == UserId) {
                return true;
            }
            return false;
        },

        setMark(TeamId, target, Stage) {
            let Mark = $(target).val();
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
        }
    }
});