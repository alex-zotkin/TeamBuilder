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
        $("#symbols_in_area").text("0/50").show();
    } else {
        if (prompt("Введите кодовое слово; для теста, кодовое слово ADMINWORD") == 'ADMINWORD') {
            $("input[name=group]").val(666).hide();
            $(".checkselect").hide();
            $("textarea").val("Администратор").hide();
            $("#symbols_in_area").hide();
        } else {
            $("select").val(1);
        }
    }
});


/*new Vue({
    el: '.profile',
    data: {
        textarea: "",
        symbols: 0
    },
    methods: {
        changeSymbols() {
            if (this.textarea.length > 50) {
                this.textarea = this.textarea.splice(0, 50);
            }
            this.symbols = this.textarea.length;
        }
    },
});*/