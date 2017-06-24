// Write your Javascript code.

window.onload = function () {
    VM = new Vue({
        el: "#main",
        methods: {
            menuSelected: function (key, keyPath) {
                console.log(key);
                console.log(keyPath);
            }
        }
    });
}