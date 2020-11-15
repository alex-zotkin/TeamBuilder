$("#symbols_in_area").text($("textarea").val().length + "/" + 50);

$("textarea").on("input", () => {
    let max = 50;
    if ($("textarea").val().length > max) {
        $("textarea").val($("textarea").val().substr(0, max));
    }
    $("#symbols_in_area").text($("textarea").val().length + "/" + max);
});





if ($("select").val() == 3) {
    $("input[name=group]").val(666).hide();
    $(".checkselect").hide();
    $("textarea").val("Администратор").hide();
    $("#symbols_in_area").hide();
}


$("select").on("change", () => {
    if ($("select").val() == 1 || $("select").val() == 2) {
        $("input[name=group]").val("").show();
        $(".checkselect").val("").show();
        $("textarea").val("").show();
        $("#symbols_in_area").val("").show();
    } else {
        $("input[name=group]").val(666).hide();
        $(".checkselect").hide();
        $("textarea").val("Администратор").hide();
        $("#symbols_in_area").hide();
    }
});


/*new Vue({
    el: '.profile',
    data: {
        description: "",
        symbols: 0
    },
    watch: {
        symbols: {
            handler() {
                if (this.symbols > 50) {
                    this.symbols = 50;
                    this.description = this.description.substr(0, 50);
                }
            }
        }
    },
    methods: {
        DescriptionInput() {
            this.symbols = this.description.length;
        }
    },
});*/