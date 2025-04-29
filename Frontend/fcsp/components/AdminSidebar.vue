<template>
  <div class="sidebar">
    <div class="sidebar-header">
      <h2 class="cursor-pointer" @click="$router.push('/admin')" style="cursor: pointer;">Admin Dashboard</h2>
    </div>
    <div class="sidebar-menu">
      <router-link to="/admin/manageOrder" class="menu-item">
        <ShoppingCartOutlined />
        <span>Order Management</span>
      </router-link>
      <router-link to="/admin/manageProduct" class="menu-item">
        <AppstoreOutlined />
        <span>Product Management</span>
      </router-link>
      <router-link to="/admin/manageAccounts" class="menu-item">
        <UserOutlined />
        <span>Account Management</span>
      </router-link>
      <router-link to="/admin/manageCommentRatings" class="menu-item">
        <StarOutlined />
        <span>Review Management</span>
      </router-link>
      <router-link to="/admin/manageShipping" class="menu-item">
        <CarOutlined />
        <span>Shipping Management</span>
      </router-link>
      <router-link to="/admin/manageShoeDesigns" class="menu-item">
        <ExperimentOutlined />
        <span>Shoe Design Management</span>
      </router-link>
      <router-link to="/admin/manageVoucher" class="menu-item">
        <GiftOutlined />
        <span>Voucher Management</span>
      </router-link>
      <div class="menu-item" @click="handleLogout">
        <LogoutOutlined />
        <span>Logout</span>
      </div>
    </div>
  </div>
</template>

<script>
import {
  ShoppingCartOutlined,
  AppstoreOutlined,
  UserOutlined,
  StarOutlined,
  CarOutlined,
  ExperimentOutlined,
  GiftOutlined,
  SettingOutlined,
  LogoutOutlined
} from '@ant-design/icons-vue';

export default {
  name: 'AdminSidebar',
  components: {
    ShoppingCartOutlined,
    AppstoreOutlined,
    UserOutlined,
    StarOutlined,
    CarOutlined,
    ExperimentOutlined,
    GiftOutlined,
    SettingOutlined,
    LogoutOutlined
  },
  methods: {
    handleLogout() {
      // Clear all localStorage data
      localStorage.clear()
      
      // Redirect to home page
      this.$router.push('/')
    }
  }
}
</script>

<style scoped>
.sidebar {
  width: 250px;
  height: 100vh;
  background-color: #001529;
  color: white;
  padding: 20px 0;
  position: fixed;
  left: 0;
  top: 0;
}

.sidebar-header {
  padding: 0 20px;
  margin-bottom: 20px;
}

.sidebar-header h2 {
  color: white;
  font-size: 1.5rem;
  margin: 0;
}

.sidebar-menu {
  display: flex;
  flex-direction: column;
}

.menu-item {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  color: rgba(255, 255, 255, 0.65);
  text-decoration: none;
  transition: all 0.3s;
}

.menu-item:hover {
  color: white;
  background-color: #1890ff;
}

.menu-item.router-link-active {
  color: white;
  background-color: #1890ff;
}

.menu-item span {
  margin-left: 10px;
}

.menu-item .anticon {
  font-size: 18px;
}
</style> 