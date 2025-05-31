export default defineNuxtRouteMiddleware((to) => {
    let role = '';
    if (process.client) {
      role = (localStorage.getItem('role') || '').toLowerCase();
    }
  
    // Các trang public không cần phân quyền
    const publicPages = [
      '/loginPage',
      '/registerPage',
      '/forgotPassword',
      '/verifyOTP',
      '/verifyOTPreset',
      '/not-authorized'
      
    ];
    if (publicPages.includes(to.path)) {
      return;
    }
  
    // Chỉ manufacturer được vào /Manufacturer
    // if (to.path.startsWith('/manufacturer') && role !== 'manufacturer') {
    //   return navigateTo('/not-authorized');
    // }
    // Chỉ admin được vào /admin
    if (to.path.startsWith('/admin') && role !== 'admin') {
      return navigateTo('/not-authorized');
    }
    
    // Các trang còn lại chỉ cho designer, customer hoặc chưa login
    if (
      !to.path.startsWith('/admin') &&
      !to.path.startsWith('/manufacturer') &&
      !['designer', 'customer', ''].includes(role) &&
      to.path !== '/not-authorized'
    ) {
      return navigateTo('/not-authorized');
    }
  
    console.log('Middleware:', { path: to.path, role });
  });