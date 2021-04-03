new Vue({
    el: '.header',
    data: {
        statusclick: false,
        nonotif: true,
        notifications: {},
        notifCount: 0,
    },
    created: function(){
        this.loadNotifications();
        setInterval(this.loadNotifications, 5000);
    },
    methods: {
        loadNotifications(){
            $.ajax({
                type: "POST",
                url: "/notifications/all",
                success: (data) => {
                    this.notifications = JSON.parse(data);
                    this.notifCount = this.notifications.ApplicationsForTeamlead.length + this.notifications.ApplicationsForUser.length;

                    /*console.log(this.notifications);*/
                    //this.loadData();
                }
            });
        },

        deleteApplication(TeamId, UserId) {
            $.ajax({
                type: "POST",
                url: "/notifications/deleteApplication/" +  TeamId + "/" + UserId,
            })
                .always(data => {
                    this.loadNotifications();
                });
        },

        checkApplication(TeamId, UserId, Successed) {
            $.ajax({
                type: "POST",
                url: "/notifications/checkApplication/" + TeamId + "/" + UserId + "/" + Successed,
            })
                .always(data => {
                    this.loadNotifications();
                });
        },

        is_not_end_of_array() {
            return (this.num == requestmens.length);
        },
        any_news() {
            return true;/*----*/
        }
    }

});