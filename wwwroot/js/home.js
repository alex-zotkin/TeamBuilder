Vue.component('team', {
    data: function () {
        return {
            count_teams: "",
            course1: "",
            course2: ""
        }
    },
    props: ['id'],
    template: `<div class="team_inputs">
                <span class= "team_input">
                    <label for="">Кол-во команд</label>
                    <input required type="number" min="0" autofocus v-on:input="inputInComponent(id)" v-model="count_teams">
                 </span>
                  * &nbsp; &nbsp; (
                <span class="team_input">
                    <label for="">Первокурсники</label>
                    <input required type="number" min="0" v-model="course1" v-on:input="inputInComponent(id)">
                 </span>
                        +
                 <span class="team_input">
                    <label for="">Второкурсники</label>
                    <input required type="number" min="0" v-model="course2" v-on:input="inputInComponent(id)">
                    </span> )
                 <span class="delete_team" v-on:click="deleteInComponent(id)">❌</span>
               </div>`,
    methods: {
        inputInComponent(id) {
            count_teams = this.count_teams;
            course1 = this.course1;
            course2 = this.course2;
            this.$emit("changedata", {
                id,
                count_teams,
                course1,
                course2
            })
        },

        deleteInComponent(id) {
            this.$emit("deletedata", id);
        }
    }
});


Vue.component('kriterii', {
    data: function () {
        return {
            krit_name: "",
            points: ""
        }
    },
    props: ['id', 'number', 'name'],
    template: `<tr>
                  <td>{{number+1}}</td>
                  <td><input required style="width:400px" type="text" v-model="krit_name" v-on:input="inputData(id)" placeholder="Введите название этапа" /></td>
                  <td><input required type="number" v-model="points" v-on:input="inputData(id)" min="1" style="width:100px" placeholder="Баллы"/></td>
                  <td class="delete_team" v-on:click="deleteKritInComponent(id)">❌</td>
               </tr>`,
    methods: {
        deleteKritInComponent(id) {
            this.$emit("deletekritfromcomponent", id);
        },
        inputData(id) {
            var krit_name = this.krit_name;
            var points = this.points;
            this.$emit("changeinkritsevent", { id, krit_name, points})
        }
    }
});

new Vue({
    el: "#app",
    data: {
        team_inputs: [
            { id: 0, count_teams: 0, course1: 0, course2: 0 },
        ],
        krits: [
            { id: 0, krit_name: "", points: "" },
        ],
        students: 0,
        show: false,
        pointsSum: 0,
        submitButton: true,
    },
    watch: {
        team_inputs: {
            deep: true,
            handler() {
                var res = 0;
                for (var i = 0; i < this.team_inputs.length; i++) {
                    res += Number(this.team_inputs[i].count_teams) * (Number(this.team_inputs[i].course1) + Number(this.team_inputs[i].course2));
                }
                this.students = res;
            }
        },
        krits: {
            deep: true,
            handler() {
                var sum = 0;
                for (var i = 0; i < this.krits.length; i++){
                    sum += Number(this.krits[i].points);
                }
                this.pointsSum = sum;
            }
        }
    },
    methods: {
        addTeam() {

            var id = this.team_inputs[this.team_inputs.length - 1].id + 1;
            this.team_inputs.push({ id: id, count_teams: "", course1: "", course2: "" });
        },
        change(data) {
            for (var i = 0; i < this.team_inputs.length; i++) {
                if (this.team_inputs[i].id != data.id) {
                    continue;
                }
                this.team_inputs[i].count_teams = data.count_teams;
                this.team_inputs[i].course1 = data.course1;
                this.team_inputs[i].course2 = data.course2;
            }
        },

        deleteteam(id) {
            if (this.team_inputs.length != 1) {
                for (var i = 0; i < this.team_inputs.length; i++) {
                    if (this.team_inputs[i].id == id) {
                        this.team_inputs.splice(i, 1);
                        break;
                    }
                }
            }
        },


        deletekrit(id) {
            if (this.krits.length != 1) {
                for (var i = 0; i < this.krits.length; i++) {
                    if (this.krits[i].id == id) {
                        this.krits.splice(i, 1);
                        break;
                    }
                }
            }
        },

        addKrit() {
            var id = this.krits[this.krits.length - 1].id + 1;
            this.krits.push({ id: id, krit_name: "", points: ""});
        },

        changekrit(data) {
            for (var i = 0; i < this.krits.length; i++) {
                if (this.krits[i].id != data.id) {
                    continue;
                }
                this.krits[i].krit_name = data.krit_name;
                this.krits[i].points = data.points;
            }
        },
        formSubmit() {
            this.submitButton = false;
            let json = JSON.stringify({
                teams: this.team_inputs,
                marks: this.krits
            });
            var time = performance.now();
            $.ajax({
                url: '/addProject',
                method: 'post',
                dataType: 'text',
                data: {
                    teams: JSON.stringify(this.team_inputs),
                    marks: JSON.stringify(this.krits)
                },

                success: function (data) {
                    window.location.href = "/project/" + data;
                }
            });

        }
    }
});
