new Vue({
    el: ".login",
    data: {
        login: false,
    },
    methods: {
        Redir() {
            this.login = true;
            window.location.replace('/auth');
        }
    }
});