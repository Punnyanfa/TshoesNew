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
        <router-link 
          v-for="item in navItems" 
          :key="item.path" 
          :to="item.path" 
          class="nav-link"
          :class="{ 'highlight-link': item.highlight }"
          @click="toggleNav"
        >
          <a-icon :type="item.icon" /> {{ item.label }}
        </router-link>
      </nav>

     <!-- User Actions -->
<div class="user-actions">
  <!-- Search Bar -->
  <div class="search-wrapper" :class="{ 'expanded': isSearchOpen }">
    <a-input-search 
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
    </a-button>
  </div>

  <!-- Cart Button -->
  <router-link to="/shoppingCartPage" class="sneaker-btn-icon cart-btn">
    <ShoppingCartOutlined />
    <span class="sneaker-badge">{{ cartCount }}</span>
  </router-link>

  <!-- User Section -->
  <template v-if="isAuthenticated">
    <div class="dropdown">
      <button 
        class="btn dropdown-toggle d-flex align-items-center" 
        type="button" 
        id="userDropdown"
        data-bs-toggle="dropdown" 
        aria-expanded="false"
      >
        <i class="bi bi-person-circle me-1"></i> {{ userName }}
      </button>
      <ul class="dropdown-menu dropdown-menu-end shadow" aria-labelledby="userDropdown">
        <li>
          <router-link class="dropdown-item" to="/profilePage">
            <i class="bi bi-person me-2"></i> Profile
          </router-link>
        </li>
        <li>
          <router-link class="dropdown-item" to="/settingsPage">
            <i class="bi bi-gear me-2"></i> Settings
          </router-link>
        </li>
        <li><hr class="dropdown-divider"></li>
        <li>
          <a class="dropdown-item text-danger" href="#" @click.prevent="logout">
            <i class="bi bi-box-arrow-right me-2"></i> Logout
          </a>
        </li>
      </ul>
    </div>
  </template>
  <template v-else>
    <router-link to="/loginPage" class="sneaker-btn">
      <UserOutlined /> Login
    </router-link>
  </template>

  <!-- Theme Toggle Button -->
  <a-button 
    class="sneaker-btn-icon theme-btn"
    shape="circle"
    @click="toggleTheme"
  >
    <a-icon :type="isDarkTheme ? 'sun' : 'moon'" />
  </a-button>
</div>

      <!-- Mobile Toggle Button -->
      <button class="navbar-toggler" @click="toggleNav">
        <span class="toggler-icon"></span>
      </button>
    </div>
  </header>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue';
import { ShoppingCartOutlined, UserOutlined } from '@ant-design/icons-vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const isAuthenticated = ref(false);
const userName = ref('');
const cartCount = ref(2);
const isNavOpen = ref(false);
const isSearchOpen = ref(false);
const searchQuery = ref('');
const isDarkTheme = ref(true);
const isScrolled = ref(false);

const navItems = [
  { path: '/homePage', label: 'Home', icon: 'home' },
  { path: '/aboutPage', label: 'About', icon: 'info-circle' },
  { path: '/productPage', label: 'Products', icon: 'shop' },
  { path: '/contactPage', label: 'Contact', icon: 'mail' },
  { path: '/customPage', label: 'Customize', icon: 'edit', highlight: true },
];

// Watch for authentication state changes
watch(() => {
  const token = localStorage.getItem('username');
  isAuthenticated.value = !!token;
  if (isAuthenticated.value) {
    userName.value = localStorage.getItem('username') || 'User';
  }
}, { immediate: true });

// Xử lý sự kiện scroll
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50;
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

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
  // Initialize dropdowns after DOM is ready
  nextTick(() => {
    initDropdowns();
  });
});

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll);
});

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
  // Clear all user data from localStorage
  localStorage.removeItem('userToken');
  localStorage.removeItem('email');
  localStorage.removeItem('role');
  localStorage.removeItem('username');
  localStorage.removeItem('userId');
  
  // Reset authentication state
  isAuthenticated.value = false;
  userName.value = '';
  
  // Redirect to home page
  router.push('/homePage');
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
</script>

<style scoped>
.sneaker-header {
  background: #1a1a1a;
  padding: 1rem 0;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  position: sticky;
  top: 0;
  z-index: 1000;
  transition: all 0.3s ease;
}

.sneaker-header.scrolled {
  padding: 0.5rem 0;
  background: rgba(26, 26, 26, 0.9);
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.5);
}

.sneaker-header.scrolled .logo-img {
  width: 35px;
  height: 35px;
}

.sneaker-header.scrolled .brand-text {
  font-size: 1.5rem;
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
}

.logo-img {
  width: 45px;
  height: 45px;
  transition: transform 0.5s ease, width 0.3s ease, height 0.3s ease;
  margin-right: 12px;
}

.brand-text {
  font-size: 1.8rem;
  font-weight: 700;
  color: #8bc34a;
  letter-spacing: 1px;
  background: #fff;
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
  transition: font-size 0.3s ease;
}

/* Các style khác giữ nguyên */
.navbar-nav {
  display: flex;
  flex-direction: row;
  gap: 2rem;
  align-items: center;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #fff;
  font-weight: 600;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  transition: transform 0.3s ease;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
}

.nav-link:hover {
  transform: scale(1.05);
  color: transparent;
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
    background: #1a1a1a;
    padding: 1rem;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  }

  .sneaker-header.scrolled .navbar-nav {
    background: rgba(26, 26, 26, 0.9);
  }

  .nav-open {
    display: flex;
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

.user-actions {
  display: flex;
  align-items: center;
  gap: 1rem; 
}

/* Search Wrapper */
.search-wrapper {
  display: flex;
  align-items: center;
  transition: width 0.3s ease;
}

.search-wrapper.expanded {
  width: 200px;
}

.search-input {
  width: 100%;
  margin-right: 0.5rem;
}

.sneaker-btn-icon.search-btn {
  background: #3498db;
  color: #fff;
  border: none;
  transition: background 0.3s ease;
}

.sneaker-btn-icon.search-btn:hover {
  background: #2980b9;
}

/* Cart Button */
.sneaker-btn-icon.cart-btn {
  position: relative;
  background: linear-gradient(45deg, #2c3e50, #3498db);;
  color: #fff;
  padding: 0.5rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.3s ease;
}

.sneaker-btn-icon.cart-btn:hover {
  background: linear-gradient(45deg, #2c3e50, #3498db);
}

.sneaker-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  background: #8bc34a;
  color: #fff;
  border-radius: 50%;
  padding: 0.2rem 0.5rem;
  font-size: 0.75rem;
}

/* User Dropdown & Login Button */
.user-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  border: none;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: #fff;
  transition: all 0.3s ease;
  border-radius: 8px;
}

.user-btn:hover {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  opacity: 0.9;
}

.dropdown {
  position: relative;
  display: inline-block;
}

.dropdown-menu {
  position: absolute;
  min-width: 200px;
  padding: 0.5rem;
  border: none;
  background: #fff;
  border-radius: 8px;
  margin-top: 0.5rem;
  z-index: 1050;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
  transform-origin: top right;
  animation: dropdownFade 0.2s ease;
}

@keyframes dropdownFade {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.dropdown-item {
  padding: 0.75rem 1rem;
  display: flex;
  align-items: center;
  color: #333;
  transition: all 0.2s ease;
  border-radius: 4px;
  font-weight: 500;
}

.dropdown-item:hover {
  background-color: #f8f9fa;
  color: #2196f3;
  transform: translateX(5px);
}

.dropdown-item i {
  width: 20px;
  text-align: center;
}

.dropdown-divider {
  margin: 0.5rem 0;
  border-color: #dee2e6;
}

.text-danger {
  color: #dc3545 !important;
}

.text-danger:hover {
  color: #fff !important;
  background-color: #dc3545 !important;
}

/* Theme Toggle Button */
.sneaker-btn-icon.theme-btn {
  background: #f1c40f;
  color: #fff;
  border: none;
  padding: 0.5rem;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.3s ease;
}

.sneaker-btn-icon.theme-btn:hover {
  background: #d4ac0d;
}

/* Responsive Adjustments */
@media (max-width: 991px) {
  .user-actions {
    gap: 0.75rem; 
  }

  .search-wrapper.expanded {
    width: 150px; 
  }

  .sneaker-btn-icon {
    padding: 0.4rem;
  }

  .dropdown-menu {
    position: fixed !important;
    top: auto !important;
    left: 0 !important;
    right: 0 !important;
    width: 100%;
    margin-top: 0.5rem;
    border-radius: 0;
  }
}

body.light-theme .sneaker-btn-icon.search-btn {
  background: #2980b9;
}

body.light-theme .sneaker-btn-icon.cart-btn {
  background: linear-gradient(45deg, #2c3e50, #3498db);
}

body.light-theme .user-btn {
  background: linear-gradient(45deg, #2c3e50, #3498db);
}

body.light-theme .sneaker-btn-icon.theme-btn {
  background: linear-gradient(45deg, #2c3e50, #3498db);
}

body.light-theme .dropdown-menu {
  background: #fff;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

body.light-theme .dropdown-item {
  color: #333;
}

body.light-theme .dropdown-item:hover {
  background-color: #f8f9fa;
  color: #2196f3;
}
</style>