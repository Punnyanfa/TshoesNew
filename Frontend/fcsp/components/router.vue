import { createRouter, createWebHistory } from 'vue-router';
import HomePage from '@/views/HomePage.vue';  // Trang chủ, nơi chứa card sản phẩm
import DetailPage from '@/views/detailPage.vue';  // Trang chi tiết sản phẩm

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage,
  },
  {
    path: '/product/:id',
    name: 'ProductDetail',
    component: DetailPage,
    props: true, // Để truyền props cho DetailPage
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
