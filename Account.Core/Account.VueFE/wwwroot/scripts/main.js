/// <reference path="vue.js" />
/// <reference path="vue-router.js" />
/// <reference path="manifests.js" />

const router = new VueRouter({
    routes: [
        { name: "manifest", path: "/manifest", component: Manifest },
        { name: "daily", path: "/daily", component: Daily },
        { name: "monthly", path: "/monthly", component: Monthly },
        { name: "yearly", path: "/yearly", component: Yearly }
    ]
});

bus = new Vue();
window.onload = function () {
    VM = new Vue({
        el: "#main",
        router: router,
        created: function () {
            router.push("manifest");
        }
    });
}