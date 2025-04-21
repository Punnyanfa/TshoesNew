<!-- Header Component - Flexbox Optimized Sneaker Theme -->
<template>
  <header class="sneaker-header" :class="{ 'scrolled': isScrolled }">
    <div class="header-container">
      <!-- Logo Section -->
      <router-link to="/homePage" class="logo-wrapper">
        <img 
          src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=338&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7" 
          alt="Sneaker Logo" 
          class="logo-img" 
          @mouseover="animateLogo"
          @mouseleave="resetLogo" 
        />
        <span class="brand-text">SneakerVibe</span>
      </router-link>

      <!-- Navigation -->
      <nav class="navbar-nav" :class="{ 'nav-open': isNavOpen }">
        <!-- Regular nav items -->
        <template v-for="item in navItems" :key="item.path">
          <!-- Regular nav link -->
          <router-link 
            v-if="item.path !== '/customPage'" 
            :to="item.path" 
            class="nav-link"
            @click="toggleNav"
          >
            <a-icon :type="item.icon" /> {{ item.label }}
          </router-link>
          
          <!-- Customize with dropdown -->
          <div v-else class="custom-dropdown">
            <div class="nav-link" style="cursor: pointer;">
              <a-icon :type="item.icon" /> {{ item.label }}
              <DownOutlined style="margin-left: 5px; font-size: 12px;" />
            </div>
            <div class="dropdown-content">
              <router-link to="/customPage" class="dropdown-item" @click="toggleNav">
                <ShoppingOutlined style="margin-right: 8px;" /> Customize Product
              </router-link>
              <router-link to="/mycustomPage" class="dropdown-item" @click="toggleNav">
                <UserOutlined style="margin-right: 8px;" /> My Customize
              </router-link>
            </div>
          </div>
        </template>
      </nav>

     <!-- User Actions -->
<div class="user-actions">
  <!-- Favorite Button -->
  <router-link to="/favoritePage" class="sneaker-btn-icon favorite-btn">
    <HeartOutlined />
    <span class="sneaker-badge">{{ favoriteCount }}</span>
  </router-link>

  <!-- Cart Button -->
  <router-link to="/cartPage" class="sneaker-btn-icon cart-btn">
    <ShoppingCartOutlined />
    <span class="sneaker-badge">{{ cartCount }}</span>
  </router-link>

    <!-- Notification Button -->
    <router-link to="/notificationPage" class="sneaker-btn-icon notification-btn">
    <BellOutlined />
    <span class="sneaker-badge">{{ notificationCount }}</span>
  </router-link>

  <!-- User Dropdown (Authenticated) -->
  <a-dropdown v-if="isAuthenticated" class="user-dropdown">
    <a-button class="sneaker-btn user-btn">
      <UserOutlined /> {{ userName }}
      <DownOutlined />
    </a-button>
    <a-dropdown-menu slot="overlay">
      <a-dropdown-item><ProfileOutlined /> Profile</a-dropdown-item>
      <a-dropdown-item><SettingOutlined /> Settings</a-dropdown-item>
      <a-dropdown-item @click="logout"><LogoutOutlined /> Logout</a-dropdown-item>
    </a-dropdown-menu>
  </a-dropdown>

  <!-- Login Link (Non-Authenticated) -->
  <router-link v-else to="/loginPage" class="sneaker-btn">
    <UserOutlined /> Login
  </router-link>
</div>

      <!-- Mobile Toggle Button -->
      <button class="navbar-toggler" @click="toggleNav">
        <span class="toggler-icon"></span>
      </button>

      <!-- <a-input-search 
        v-if="isSearchOpen"
        v-model="searchQuery"
        placeholder="Search sneakers..."
        @search="onSearch"
        class="search-input" 
      />

      <a-button 
        class="sneaker-btn-icon search-btn"
        shape="circle"
        @click="toggleSearch"
      >
        <a-icon type="search" />
      </a-button> -->
    </div>
  </header>
</template>

<script setup>
import { ref, onMounted, onUnmounted, computed, watch, nextTick } from 'vue';
import { 
  ShoppingCartOutlined, 
  UserOutlined, 
  DownOutlined, 
  ProfileOutlined, 
  SettingOutlined, 
  LogoutOutlined, 
  HeartOutlined,
  BellOutlined,
  ShoppingOutlined
} from '@ant-design/icons-vue';
import { useRouter } from 'vue-router';
import { useCart } from '~/composables/useCart';

const router = useRouter();
const { cartCount } = useCart();

const isAuthenticated = ref(false);
const userName = ref('SneakerFan');
const favoriteCount = ref(3);
const notificationCount = ref(1);
const isNavOpen = ref(false);
const isSearchOpen = ref(false);
const searchQuery = ref('');
const isDarkTheme = ref(true);
const isScrolled = ref(false);

const navItems = [
  { path: '/homePage', label: 'Home', icon: 'home' },
  { path: '/productPage', label: 'Products', icon: 'shop' },
  { path: '/customPage', label: 'Customize', icon: 'edit' },
  { path: '/contactPage', label: 'Contact', icon: 'mail' },
  { path: '/aboutPage', label: 'About', icon: 'info-circle' },
];

// Loại bỏ Customize khỏi danh sách menu thông thường vì đã có dropdown riêng
const filteredNavItems = computed(() => {
  return navItems.filter(item => item.label !== 'Customize');
});

// Xử lý sự kiện scroll
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50; // Thay đổi trạng thái khi scroll quá 50px
};

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
  nextTick(() => {
    initDropdowns();
  });
  const token = localStorage.getItem('username');
  userName.value = localStorage.getItem('username') || 'User';
});

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll);
  localStorage.removeItem('userToken');
  localStorage.removeItem('email');
  localStorage.removeItem('role');
  localStorage.removeItem('username');
  localStorage.removeItem('userId');
});

watch(() => {
  const token = localStorage.getItem('username');
  isAuthenticated.value = !!token;
  if (isAuthenticated.value) {
    userName.value = localStorage.getItem('username') || 'User';
  }
}, { immediate: true });

const toggleNav = () => {
  isNavOpen.value = !isNavOpen.value;
  if (isSearchOpen.value) isSearchOpen.value = false;
};

const toggleSearch = () => {
  isSearchOpen.value = !isSearchOpen.value;
  if (isSearchOpen.value) searchQuery.value = '';
};

const onSearch = (value) => {
  console.log('Searching for:', value);
  isSearchOpen.value = false;
};

const logout = () => {
  isAuthenticated.value = false;
};

const toggleTheme = () => {
  isDarkTheme.value = !isDarkTheme.value;
  document.body.classList.toggle('light-theme', !isDarkTheme.value);
};

const animateLogo = (e) => {
  e.target.style.transform = 'rotate(360deg) scale(1.1)';
};

const resetLogo = (e) => {
  e.target.style.transform = 'rotate(0deg) scale(1)';
};

const pushHomePage = () => {
  router.push('/homePage');
};

const initDropdowns = async () => {
  if (process.client) {
    try {
      const bootstrap = await import('bootstrap/dist/js/bootstrap.bundle.min.js');
      const dropdownElements = document.querySelectorAll('.dropdown-toggle');
      dropdownElements.forEach(element => {
        new bootstrap.Dropdown(element);
      });
    } catch (error) {
      console.error('Error initializing dropdowns:', error);
    }
  }
};
</script>

<style scoped>
.sneaker-header {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  padding: 1rem 0;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 1000;
  transition: all 0.3s ease;
}

.sneaker-header.scrolled {
  padding: 0.5rem 0;
  background: rgba(255, 255, 255, 0.9);
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.1);
}

.header-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.logo-wrapper {
  display: flex;
  align-items: center;
  text-decoration: none;
  transition: transform 0.3s ease;
}

.logo-wrapper:hover {
  transform: scale(1.05);
}

.logo-img {
  width: 45px;
  height: 45px;
  transition: all 0.3s ease;
  margin-right: 12px;
  border-radius: 50%;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.brand-text {
  font-size: 1.8rem;
  font-weight: 700;
  color: #555555;
  letter-spacing: 1px;
  transition: all 0.3s ease;
}

.navbar-nav {
  display: flex;
  flex-direction: row;
  gap: 1.5rem;
  align-items: center;
}

.nav-link {
  display: flex;
  align-items: center;
  color: #555555;
  font-weight: 600;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 10px;
  transition: all 0.3s ease;
  position: relative;
}

.nav-link::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 0;
  height: 2px;
  background: #555555;
  transition: all 0.3s ease;
  transform: translateX(-50%);
}

.nav-link:hover::after {
  width: 80%;
}

.nav-link:hover {
  transform: none;
  box-shadow: none;
  background: none;
}

.user-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.search-wrapper {
  display: flex;
  align-items: center;
  transition: all 0.3s ease;
}

.search-wrapper.expanded {
  width: 250px;
}

.search-input {
  width: 100%;
  margin-right: 0.5rem;
  border-radius: 24px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  padding: 0.5rem 1rem;
}

.sneaker-btn-icon {
  position: relative;
  background: linear-gradient(135deg, #555555, #555555);
  color: #fff;
  border-radius: 24px;
  padding: 0.5rem;
  transition: all 0.3s ease;
}

.sneaker-btn-icon:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  background: linear-gradient(135deg, #444444, #444444);
}

.sneaker-btn-icon.favorite-btn {
  position: relative;
  background: linear-gradient(135deg, #555555, #555555);
  color: #fff;
}

.sneaker-btn-icon.favorite-btn:hover {
  background: linear-gradient(135deg, #444444, #444444);
}

.sneaker-btn-icon.cart-btn {
  position: relative;
  background: linear-gradient(135deg, #555555, #555555);
  color: #fff;
}

.sneaker-btn-icon.cart-btn:hover {
  background: linear-gradient(135deg, #444444, #444444);
}

.sneaker-btn-icon.notification-btn {
  position: relative;
  background: linear-gradient(135deg, #555555, #555555);
  color: #fff;
}

.sneaker-btn-icon.notification-btn:hover {
  background: linear-gradient(135deg, #444444, #444444);
}

.sneaker-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  background: #555555;
  color: #fff;
  border-radius: 50%;
  padding: 0.2rem 0.5rem;
  font-size: 0.75rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.user-dropdown .sneaker-btn.user-btn,
.sneaker-btn {
  background: linear-gradient(135deg, #555555, #555555);
  color: #fff;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 24px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.3s ease;
}

.user-dropdown .sneaker-btn.user-btn:hover,
.sneaker-btn:hover {
  background: linear-gradient(135deg, #444444, #444444);
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.sneaker-btn-icon.theme-btn {
  display: none;
}

@media (max-width: 991px) {
  .header-container {
    flex-direction: row;
    align-items: center;
  }

  .navbar-nav {
    display: none;
    flex-direction: column;
    width: 100%;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    padding: 1rem;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    border-radius: 24px;
  }

  .sneaker-header.scrolled .navbar-nav {
    background: rgba(255, 255, 255, 0.9);
  }

  .nav-open {
    display: flex;
  }

  .user-actions {
    gap: 0.75rem;
  }

  .search-wrapper.expanded {
    width: 200px;
  }

  .sneaker-btn-icon {
    padding: 0.4rem;
  }
}

body.light-theme .sneaker-header {
  background: #ffffff;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

body.light-theme .sneaker-header.scrolled {
  background: rgba(255, 255, 255, 0.9);
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.2);
}

body.light-theme .sneaker-header.scrolled .logo-img {
  width: 35px;
  height: 35px;
}

body.light-theme .sneaker-header.scrolled .brand-text {
  font-size: 1.5rem;
}

body.light-theme .navbar-nav {
  display: flex;
  flex-direction: row;
  gap: 2rem;
  align-items: center;
}

body.light-theme .nav-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #555555;
  font-weight: 600;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  transition: transform 0.3s ease;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
}

body.light-theme .nav-link:hover {
  transform: scale(1.05);
  color: transparent;
}

body.light-theme .user-actions {
  display: flex;
  align-items: center;
  gap: 1rem; 
}

body.light-theme .search-wrapper {
  display: flex;
  align-items: center;
  transition: width 0.3s ease;
}

body.light-theme .search-wrapper.expanded {
  width: 200px;
}

body.light-theme .search-input {
  width: 100%;
  margin-right: 0.5rem;
}

body.light-theme .sneaker-btn-icon.cart-btn {
  background: linear-gradient(45deg, #2c3e50, #3498db);
}

body.light-theme .user-dropdown .sneaker-btn.user-btn,
body.light-theme .sneaker-btn {
  background: linear-gradient(45deg, #2c3e50, #3498db);
}

body.light-theme .sneaker-btn-icon.theme-btn {
  display: none;
}

/* Dropdown styling */
.custom-dropdown {
  position: relative;
  display: inline-block;
}

.custom-dropdown .nav-link {
  display: flex;
  align-items: center;
  color: #555555;
  font-weight: 600;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 10px;
  transition: all 0.3s ease;
  position: relative;
}

.custom-dropdown .nav-link::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 0;
  height: 2px;
  background: #555555;
  transition: all 0.3s ease;
  transform: translateX(-50%);
}

.custom-dropdown .nav-link:hover::after {
  width: 80%;
}

.dropdown-content {
  position: absolute;
  top: 100%;
  left: 0;
  display: none;
  background: rgba(255, 255, 255, 0.98);
  min-width: 180px;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
  z-index: 1;
  border-radius: 10px;
  padding: 8px 0;
  margin-top: 5px;
  padding-top: 10px;
  margin-top: -5px;
  opacity: 0;
  visibility: hidden;
  transition: opacity 0.3s, visibility 0.3s;
}

.dropdown-content:before {
  content: '';
  position: absolute;
  top: -10px;
  left: 0;
  width: 100%;
  height: 10px;
  background: transparent;
}

.dropdown-item {
  color: #555555;
  text-decoration: none;
  display: flex;
  align-items: center;
  padding: 10px 16px;
  transition: all 0.3s ease;
  font-weight: 500;
}

.dropdown-content a:hover,
.dropdown-item:hover {
  background-color: rgba(85, 85, 85, 0.08);
  transform: translateX(3px);
}

.custom-dropdown:hover .dropdown-content {
  display: block;
  opacity: 1;
  visibility: visible;
}

.custom-dropdown .nav-link:hover + .dropdown-content,
.dropdown-content:hover {
  display: block;
  opacity: 1;
  visibility: visible;
}

/* Mobile styles for dropdown */
@media (max-width: 991px) {
  .dropdown-content {
    position: static;
    display: none;
    box-shadow: none;
    margin-top: 0;
    padding-left: 1rem;
    opacity: 1;
    visibility: visible;
    transition: none;
  }
  
  .dropdown-content:before {
    display: none;
  }
  
  .custom-dropdown:hover .dropdown-content {
    display: block;
  }
}

/* Light theme support */
body.light-theme .dropdown-content {
  background: #ffffff;
}

body.light-theme .dropdown-content a {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

body.light-theme .dropdown-content a:hover {
  background-color: rgba(52, 152, 219, 0.05);
}
</style>
