import HomePage from "@/views/HomePage.vue";
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    path: "/",
    name: "homePage",
    component: HomePage,
  },
];
const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
